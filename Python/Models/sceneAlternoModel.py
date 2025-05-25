from Enums import CharactersType
from shapely.geometry import Point


class SceneAlternoModel:
    def __init__(self, alterno_name):
        self._alterno_name = alterno_name

    @property
    def alterno_name(self) -> str:
        return self._alterno_name

    @alterno_name.setter
    def alterno_name(self, new_name: str):
        self._alterno_name = new_name

    @property
    def base_visual(self) -> str:
        return self._base_visual

    @base_visual.setter
    def base_visual(self, new_asset: str):
        self._base_visual = new_asset

    @property
    def order(self) -> int:
        return self._order

    @order.setter
    def order(self, new_number: int):
        self._order = new_number

    @property
    def character_type(self) -> CharactersType:
        return self._character_type

    @character_type.setter
    def character_type(self, new_type: CharactersType):
        self._character_type = new_type

    @property
    def position(self) -> Point:
        return self._position

    @position.setter
    def position(self, new_position: Point):
        self._position = new_position
