import tkinter as tk
from tkinter import filedialog

class ImageSelectorModule:
    def __init__(self, master):
        self._personajes = []
        self.master = master
        self.master.title("Select Images")

        # Botón para seleccionar imágenes
        self.select_button = tk.Button(self.master, text="Select Images", command=self.select_images)
        self.delete_button = tk.Button(self.master, text="Remove Images", command=self.remove_images)
        self.select_button.pack(pady=10)
        self.delete_button.pack(pady=100)