from Models.videoSectionModel import VideoSectionModel
from Models.sceneAlternoModel import SceneAlternoModel
from Models.sceneElementModel import SceneElementModel
from moviepy.editor import AudioFileClip
from typing import List


class SceneModel:
    def __init__(self, scene_name, scene_number):
        self._scene_name = scene_name
        self._scene_number = scene_number
        self._video_sections: List[VideoSectionModel] = []
        self._scene_elements: List[SceneElementModel] = []
        self._scene_alternos: List[SceneAlternoModel] = []

    @property
    def scene_name(self) -> str:
        return self._scene_name

    @scene_name.setter
    def scene_name(self, new_name: str):
        self._scene_name = new_name

    @property
    def scene_number(self) -> int:
        return self._scene_number

    @scene_number.setter
    def scene_number(self, new_number: int):
        self._scene_number = new_number

    @property
    def order(self) -> int:
        return self._order

    @order.setter
    def order(self, new_number: int):
        self._order = new_number

    @property
    def audio_asset(self) -> str:
        return self._audio_asset

    @audio_asset.setter
    def audio_asset(self, new_asset: str):
        self._audio_asset = new_asset

    @property
    def video_sections(self) -> List[VideoSectionModel]:
        return self._video_sections

    @video_sections.setter
    def video_sections(self, new_video_sections: List[VideoSectionModel]):
        self._video_sections = new_video_sections

    def add_video_section(self, section: VideoSectionModel):
        self._video_sections.append(section)

    @property
    def base_visual(self) -> str:
        return self._base_visual

    @base_visual.setter
    def base_visual(self, new_asset: str):
        self._base_visual = new_asset

    def get_scene_duration(self):
        scene_duration: float = 0
        for section in self._video_sections:
            audio_clip = AudioFileClip(section.audio_asset)
            scene_duration += audio_clip.duration

        return scene_duration

    @property
    def scene_elements(self) -> List[SceneElementModel]:
        return self._scene_elements

    @scene_elements.setter
    def scene_elements(self, new_scene_elements: List[SceneElementModel]):
        self._scene_elements = new_scene_elements

    def add_scene_element(self, element: SceneElementModel):
        self._scene_elements.append(element)

    @property
    def scene_alternos(self) -> List[SceneAlternoModel]:
        return self._scene_alternos

    @scene_alternos.setter
    def scene_alternos(self, new_scene_alterno: List[SceneAlternoModel]):
        self._scene_alternos = new_scene_alterno

    def add_scene_alterno(self, alterno: SceneAlternoModel):
        self._scene_alternos.append(alterno)
