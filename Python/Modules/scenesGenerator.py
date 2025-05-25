from typing import List
from Enums.ElementPosition import ElementPosition
from Models.sceneElementModel import SceneElementModel
from Enums.Voices import Voices
from Enums.CharactersType import CharactersType
from shapely.geometry import Point
from Models.characterModel import CharacterModel
from Models.sceneModel import SceneModel
from Models.videoSectionModel import VideoSectionModel


class SceneGeneratorModule:
    def __init__(self):
        pass

    def GenerateScenes(self) -> List[SceneModel]:
        # crear objetos elementos para escena
        objeto_elemento_arriba = SceneElementModel("arriba")
        objeto_elemento_arriba.order = ElementPosition.UNO
        objeto_elemento_arriba.base_visual = "Assets/Images/perico.gif"
        objeto_elemento_arriba.character_type = CharactersType.OBJECT
        objeto_elemento_arriba.position = Point(1, 2)

        objeto_elemento_arriba2 = SceneElementModel("arriba")
        objeto_elemento_arriba2.order = ElementPosition.DOS
        objeto_elemento_arriba2.base_visual = "Assets/Images/perico.gif"
        objeto_elemento_arriba2.character_type = CharactersType.OBJECT
        objeto_elemento_arriba2.position = Point(1, 2)

        objeto_elemento_arriba3 = SceneElementModel("arriba")
        objeto_elemento_arriba3.order = ElementPosition.TRES
        objeto_elemento_arriba3.base_visual = "Assets/Images/perico.gif"
        objeto_elemento_arriba3.character_type = CharactersType.OBJECT
        objeto_elemento_arriba3.position = Point(1, 2)
        # ---------------------------------------
        objeto_elemento_abajo = SceneElementModel("abajo")
        objeto_elemento_abajo.order = ElementPosition.TRES
        objeto_elemento_abajo.base_visual = "Assets/Images/mariquita.gif"
        objeto_elemento_abajo.character_type = CharactersType.OBJECT
        objeto_elemento_abajo.position = Point(1, 2)
        
        objeto_elemento_abajo2 = SceneElementModel("abajo")
        objeto_elemento_abajo2.order = ElementPosition.CUATRO
        objeto_elemento_abajo2.base_visual = "Assets/Images/mariquita.gif"
        objeto_elemento_abajo2.character_type = CharactersType.OBJECT
        objeto_elemento_abajo2.position = Point(1, 2)

        objeto_elemento_abajo3 = SceneElementModel("abajo")
        objeto_elemento_abajo3.order = ElementPosition.CINCO
        objeto_elemento_abajo3.base_visual = "Assets/Images/mariquita.gif"
        objeto_elemento_abajo3.character_type = CharactersType.OBJECT
        objeto_elemento_abajo3.position = Point(1, 2)

        # crear una lista de characters
        objeto_characters = CharacterModel("luffy")
        objeto_characters.order = 1
        objeto_characters.base_visual = "Assets/Images/luffy.gif"
        objeto_characters.character_type = CharactersType.CHARACTER
        objeto_characters.position = Point(1, 2)

        objeto_character2 = CharacterModel("kirby")
        objeto_character2.order = 2
        objeto_character2.base_visual = "Assets/Images/kirby.gif"
        objeto_character2.character_type = CharactersType.CHARACTER
        objeto_character2.position = Point(1, 2)

        objeto_character3 = CharacterModel("Gato_tart")
        objeto_character3.order = 3
        objeto_character3.base_visual = "Assets/Images/cat_tart.gif"
        objeto_character3.character_type = CharactersType.CHARACTER
        objeto_character3.position = Point(1, 2)

        # crear una section
        objeto_section = VideoSectionModel()
        objeto_section.audio_asset = "Assets/Audios/Stadium.mp3"
        objeto_section.order = 1
        objeto_section.voice = Voices.VOICE1
        objeto_section.add_character(objeto_characters)
        objeto_section.add_character(objeto_character2)

        objeto_section2 = VideoSectionModel()
        objeto_section2.audio_asset = "Assets/Audios/Corniche.mp3"
        objeto_section2.order = 1
        objeto_section2.voice = Voices.VOICE1
        objeto_section2.add_character(objeto_characters)
        objeto_section2.add_character(objeto_character3)

        objeto_section3 = VideoSectionModel()
        objeto_section3.audio_asset = "Assets/Audios/Musicos.mp3"
        objeto_section3.order = 1
        objeto_section3.voice = Voices.VOICE1
        objeto_section3.add_character(objeto_character2)
        objeto_section3.add_character(objeto_character3)

        objeto_section4 = VideoSectionModel()
        objeto_section4.audio_asset = "Assets/Audios/Piloto.wav"
        objeto_section4.order = 1
        objeto_section4.voice = Voices.VOICE1
        objeto_section4.add_character(objeto_characters)
        objeto_section4.add_character(objeto_character2)
        # ----------------------------------------------------------------------------
        # crear una escenas
        # ----------------------------------------------------------------------------
        objeto_escena = SceneModel("escena 1", 1)
        objeto_escena.add_video_section(objeto_section)
        objeto_escena.base_visual = "Assets/Images/puente.jpg"
        objeto_escena.audio_asset = ""
        objeto_escena.add_scene_element(objeto_elemento_arriba)
        objeto_escena.add_scene_element(objeto_elemento_arriba2)
        objeto_escena.add_scene_element(objeto_elemento_arriba3)
        objeto_escena.add_scene_element(objeto_elemento_abajo)
        objeto_escena.add_scene_element(objeto_elemento_abajo2)
        objeto_escena.add_scene_element(objeto_elemento_abajo3)

        objeto_escena2 = SceneModel("escena 2", 2)
        objeto_escena2.add_video_section(objeto_section2)
        objeto_escena2.base_visual = "Assets/Images/cabanas.jpg"
        objeto_escena2.audio_asset = ""
        objeto_escena2.add_scene_element(objeto_elemento_arriba)
        objeto_escena2.add_scene_element(objeto_elemento_arriba2)
        objeto_escena2.add_scene_element(objeto_elemento_arriba3)
        objeto_escena2.add_scene_element(objeto_elemento_abajo)
        objeto_escena2.add_scene_element(objeto_elemento_abajo2)
        objeto_escena2.add_scene_element(objeto_elemento_abajo3)

        objeto_escena3 = SceneModel("escena 3", 3)
        objeto_escena3.add_video_section(objeto_section3)
        objeto_escena3.add_video_section(objeto_section4)
        objeto_escena3.base_visual = "Assets/Images/noche_campo.jpg"
        objeto_escena3.audio_asset = ""
        objeto_escena3.add_scene_element(objeto_elemento_arriba)
        objeto_escena3.add_scene_element(objeto_elemento_arriba2)
        objeto_escena3.add_scene_element(objeto_elemento_arriba3)
        objeto_escena3.add_scene_element(objeto_elemento_abajo)
        objeto_escena3.add_scene_element(objeto_elemento_abajo2)
        objeto_escena3.add_scene_element(objeto_elemento_abajo3)
        return [objeto_escena, objeto_escena2, objeto_escena3]
