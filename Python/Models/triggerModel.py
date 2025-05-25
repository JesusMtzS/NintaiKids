class TriggerModel:
    def __init__(self):
        pass

    @property
    def story_name(self) -> str:
        return self._story_name

    @story_name.setter
    def story_name(self, new_name: str):
        self._story_name = new_name

    @property
    def length(self) -> int:
        return self._length

    @length.setter
    def length(self, new_length: int):
        self._length = new_length

    @property
    def characters(self) -> int:
        return self._characters

    @characters.setter
    def characters(self, new_qty: int):
        self._characters = new_qty

    @property
    def story_description(self) -> str:
        return self._story_description

    @story_description.setter
    def story_description(self, new_desc: str):
        self._story_description = new_desc

    @property
    def edad_meta(self) -> int:
        return self._edad_meta

    @edad_meta.setter
    def edad_meta(self, new_edad: int):
        self._edad_meta = new_edad
