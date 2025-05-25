from Models.triggerModel import TriggerModel
from Models.videoConfigModel import VideoConfigModel
from Modules.imageSelectorModule import ImageSelectorModule
from Modules.scenesGenerator import SceneGeneratorModule
from Modules.videoGenerator import VideoModule
import tkinter as tk
from tkinter import filedialog
from PIL import Image, ImageTk


class MainModule:

    def __init__(self):
        self._objeto_trigger = TriggerModel()
        self._sceneGen = SceneGeneratorModule()
        self._sceneList = []
        self._personajes = []
        

    def StartProcess(self):
        self.master = tk.Tk()
        self._objeto_trigger = TriggerModel()
        self.master.geometry("1000x600")
        
        self.master.title("Nintai Kids")
        # Crear un canvas dentro del frame
        self.canvas = tk.Canvas(self.master, bg="red", width=800, height=200)
        self.canvas.pack(padx=0, pady=10)
        # Botón para seleccionar imágenes
        self.select_button = tk.Button(self.master, text="Select Images", command=self.select_images)
        self.delete_button = tk.Button(self.master, text="Remove Images", command=self.remove_images)
        self.select_button.pack(pady=10)
        self.delete_button.pack(pady=100)
        self.master.mainloop()
        
        test = self.GetText(
            "Selecciona que personajes participaran en la historia: "
        )
        self._objeto_trigger.story_name = self.GetText(
            "Ingresa el nombre de la historia: "
        )
        self._objeto_trigger.length = self.GetNum(
            "Ingrese el la duracion en numero de palabras (aproximado): "
        )
        self._objeto_trigger.characters = self.GetNum(
            "Ingresa el numero de personajes: "
        )
        self._objeto_trigger.story_description = self.GetText(
            "Ingresa la descripcion de la historia: "
        )
        self._objeto_trigger.edad_meta = self.GetNum(
            "Ingresa la edad meta: "
        )

    def GetText(self, text):
        while True:
            user_input = input(text)
            if user_input:
                return user_input
            else:
                print("No se ha ingresado ningún texto. Por favor, intente nuevamente.")

    def GetNum(self, text):
        while True:
            user_input = input(text)
            if user_input.isdigit():
                return int(user_input)
            else:
                print("Por favor, ingrese solo números enteros.")

    def GenerateHistory(self):
        print("Generando historia.....")
        print("TODO: Modulo generador.....")
        self._sceneList = self._sceneGen.GenerateScenes()
        # Crear una instancia de Funcionalidad

    def CreateVideo(self, config: VideoConfigModel):
        print("Creando video.....")
        # crear video
        self._videoGen = VideoModule(self._sceneList, config)
        self._videoGen.crearVideo()

    def select_images(self):
        file_paths = filedialog.askopenfilenames(initialdir="/Assets/Images" ,title="Select Images", filetypes=(("Image files", "*.png;*.jpg;*.jpeg;*.gif"), ("All files", "*.*")))
        for file_path in file_paths:
            self._personajes.append(file_path)
            print("Selected:", file_path)

    def remove_images(self):
        file_paths = filedialog.askopenfilenames(initialdir="/Assets/Images" ,title="Remove Images", filetypes=(("Image files", "*.png;*.jpg;*.jpeg;*.gif"), ("All files", "*.*")))
        for file_path in file_paths:
            self._personajes.remove(file_path)
            print("Removed:", file_path)

    @property
    def personajes(self) -> str:
        return self._personajes

    @personajes.setter
    def personajes(self, new_voice: str):
        self._personajes = new_voice
