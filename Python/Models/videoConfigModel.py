from Enums.OutputFormat import OutputFormat


class VideoConfigModel:
    def __init__(self):
        self._debug = False
        self._codec = "libx264"
        self._fps = 24
        self._section_offset = 1
        self._output_format = OutputFormat.MP4
        self._width = 1920
        self._height = 1080
        self._character_positions = {0: (225, 315), 1: (1185, 315)}
        self._element_positions = {
            0: (0, 0),
            1: (704, 0),
            2: (1408, 0),
            3: (0, 880),
            4: (704, 880),
            5: (1408, 880),
        }
        self._alterno_positions = {0: (0, 0), 1: (0, 880)}

    @property
    def debug(self) -> bool:
        return self._debug

    @debug.setter
    def debug(self, flag: bool):
        self._debug = flag

    @property
    def codec(self) -> str:
        return self._codec

    @codec.setter
    def codec(self, new_codec: str):
        self._codec = new_codec

    @property
    def fps(self) -> int:
        return self._fps

    @fps.setter
    def fps(self, new_fps: int):
        self._fps = new_fps

    @property
    def section_offset(self) -> int:
        return self._section_offset

    @section_offset.setter
    def section_offset(self, new_section_offset: int):
        self._section_offset = new_section_offset

    @property
    def output_format(self) -> OutputFormat:
        return self._output_format

    @output_format.setter
    def output_format(self, new_output_format: OutputFormat):
        self._output_format = new_output_format

    @property
    def width(self) -> int:
        return self._width

    @width.setter
    def width(self, new_width: int):
        self._width = new_width

    @property
    def height(self) -> int:
        return self._height

    @height.setter
    def height(self, new_height: int):
        self._height = new_height
