from typing import List
from Enums import Voices
from Models.characterModel import CharacterModel


class VideoSectionModel:
    def __init__(self):
        self._characters: List[CharacterModel] = []

    @property
    def voice(self) -> Voices:
        return self._voice

    @voice.setter
    def voice(self, new_voice: Voices):
        self._voice = new_voice

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
    def dialog(self) -> str:
        return self._dialog

    @dialog.setter
    def dialog(self, new_dialog: str):
        self._dialog = new_dialog

    @property
    def characters(self) -> List[CharacterModel]:
        return self._characters

    @characters.setter
    def characters(self, new_character: List[CharacterModel]):
        self._characters = new_character

    def add_character(self, character: CharacterModel):
        self._characters.append(character)
