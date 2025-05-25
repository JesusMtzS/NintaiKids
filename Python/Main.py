from Models.videoConfigModel import VideoConfigModel
from Modules.mainModule import MainModule

objeto_main = MainModule()
videoConfigModel = VideoConfigModel()

#objeto_main.StartProcess() Main will be executed on C#
objeto_main.GenerateHistory()
objeto_main.CreateVideo(videoConfigModel)
