from moviepy.editor import *
from Models.videoConfigModel import VideoConfigModel
from Models.sceneModel import SceneModel
from typing import List
from datetime import datetime
import imageio
from PIL import Image
import numpy as np


class VideoModule:
    def __init__(self, scenes: List[SceneModel], videoConfigModel: VideoConfigModel):
        self._scenes = scenes
        self._videoConfigModel = videoConfigModel

    @property
    def scenes(self):
        return self._scenes

    @scenes.setter
    def scenes(self, new_scenes: List[SceneModel]):
        self._scenes = new_scenes

    def add_scene(self, scene: SceneModel):
        self._scenes.append(scene)

    def crearVideo(self):
        scenes_clips = []
        for scene_index, scene in enumerate(self._scenes):
            # We can have any number of scenes
            print("################## SCENES")
            escene_duration = scene.get_scene_duration()
            scene_elements_clip_list = []
            scene_alternos_clip_list = []
            sections_clips = []

            print("################## SCENES - Elements")

            if scene.scene_elements:
                for scene_elements_index, scene_element in enumerate(
                    scene.scene_elements
                ):
                    scene_E_clip = self.process_Image(scene_element.base_visual)

                    self.debug_process(scene_E_clip, "INICA")
                    # scene_E_clip = scene_E_clip.loop(duration=escene_duration)
                    scene_E_clip = scene_E_clip.set_position(
                        self._videoConfigModel._element_positions[scene_element.order.value]
                    )
                    self.debug_process(scene_E_clip, "ESELEM")
                    scene_elements_clip_list.append(scene_E_clip)

            print("################## SCENES - Alternos")
            if scene.scene_alternos:
                for scene_alternos_index, scene_alterno in enumerate(
                    scene.scene_alternos
                ):
                    scene_A_clip = self.process_Image(scene_alterno.base_visual)
                    # TODO : loop should be set after audio duration, and we should use set_duration instead
                    scene_A_clip = scene_A_clip.loop(duration=escene_duration)
                    scene_A_clip = scene_A_clip.set_position(
                        self._videoConfigModel._alterno_positions[scene_alternos_index]
                    )
                    self.debug_process(scene_A_clip, "ESALT")
                    scene_alternos_clip_list.append(scene_A_clip)

            print("################## BACKGROUND")
            background_image = ImageClip(scene.base_visual)
            background_image = background_image.resize(
                width=self._videoConfigModel.width, height=self._videoConfigModel.height
            )

            for section_index, scene_section in enumerate(scene.video_sections):
                # We can have any number of sections
                print("################## SECTIONS")
                audio_clip = AudioFileClip(scene_section.audio_asset)
                duracion_audio = (
                    audio_clip.duration + self._videoConfigModel.section_offset
                )
                character_clip_list = []
                background_clip = background_image.set_duration(duracion_audio)
                scene_elements_clip_list_2 = []
                for clip in scene_elements_clip_list:
                    #clip = clip.loop(duration=duracion_audio)
                    clip = clip.set_duration(duracion_audio)
                    scene_elements_clip_list_2.append(clip)

                character_clip_list.append(background_clip)

                for character_index, character in enumerate(scene_section.characters):
                    # We just can have 2 characters that uses visual assets per section
                    print("################## CHARACTERS")
                    if character.base_visual:
                        character_clip = self.process_Image(character.base_visual)
                        # repeticiones_necesarias = (
                        #     duracion_audio / character_clip.duration
                        # )
                        character_clip = character_clip.set_duration(duracion_audio)
                        #character_clip = character_clip.loop(repeticiones_necesarias)
                        character_clip = character_clip.set_position(
                            self._videoConfigModel._character_positions[character_index]
                        )
                        self.debug_process(character_clip, "C1clip")
                        character_clip_list.append(character_clip)

                visual_element_container = character_clip_list.copy()
                visual_element_container.extend(scene_elements_clip_list_2)
                final_characters_clip = CompositeVideoClip(
                    visual_element_container,
                    size=(self._videoConfigModel.width, self._videoConfigModel.height),
                )
                self.debug_process(final_characters_clip, "S1final")
                characters_with_audio = final_characters_clip.set_audio(audio_clip)
                self.debug_process(characters_with_audio, "S2Audio")
                sections_clips.append(characters_with_audio)

            escene_clip = concatenate_videoclips(sections_clips)
            self.debug_process(escene_clip, "E1clip")
            scenes_clips.append(escene_clip)

        final_clip = concatenate_videoclips(scenes_clips)
        self.saveVideo(final_clip)

    def getIdentifier(self):
        fecha_hora_actual = datetime.now()
        return fecha_hora_actual.strftime("%y-%m-%d_%H_%M_%S_%f")

    def saveVideo(self, clip: VideoClip):
        fecha_hora_formateada = self.getIdentifier()
        outPath = (
            "Outputs/video_"
            + fecha_hora_formateada
            + str(self._videoConfigModel.output_format)
        )
        self.writeVideo(clip, outPath)

    def debug_process(self, clip: VideoClip, desc: str, bypass: bool = False):
        if self._videoConfigModel.debug or bypass:
            fecha_hora_formateada = self.getIdentifier()
            outPath = (
                "Debug/"
                + desc[:7]
                + "_"
                + fecha_hora_formateada
                + str(self._videoConfigModel.output_format)
            )
            self.writeVideo(clip, outPath)

    def writeVideo(self, clip: VideoClip, outPath: str):
        clip.write_videofile(
            outPath, codec=self._videoConfigModel.codec, fps=self._videoConfigModel.fps
        )

    def process_Image(self, path, max_duration = 180) -> ImageSequenceClip:
        gif_image = Image.open(path)
        frames_with_alpha = []

        for frame_index in range(gif_image.n_frames):
            gif_image.seek(frame_index)
            rgba_frame = np.array(gif_image.convert("RGBA"))
            frames_with_alpha.append(rgba_frame)

        durations = [0.07] * len(frames_with_alpha)
        imageClip = ImageSequenceClip(frames_with_alpha, durations=durations)
        repeticiones = max_duration / imageClip.duration
        repeated_clips = [imageClip] * (int(repeticiones) if repeticiones >= 1 else 1)
        newImageClip = concatenate_videoclips(repeated_clips)
        return newImageClip
