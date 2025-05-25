using Nintai_kids.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nintai_kids
{
    public partial class ExtraSelector : Form
    {
        private string split_char = "^";
        private bool isCharacter = false;
        internal AssetClass assetSelected = new AssetClass();
        private string dataBase = "";
        private bool checkJSONUnsafe = false;
        internal List<AssetClass> databaseAvailable;
        private PathConfig pathConfig = new PathConfig();

        internal ExtraSelector(PathConfig _pathConfig, bool _isCharacter, bool _checkJSONUnsafe, List<AssetClass> _escenas_disponibles)
        {
            InitializeComponent();
            isCharacter = _isCharacter;
            dataBase = (isCharacter ? _pathConfig.CharactesDatabase : _pathConfig.ScenesDatabase);
            checkJSONUnsafe = _checkJSONUnsafe;
            databaseAvailable = _escenas_disponibles;
            pathConfig = _pathConfig;
        }

        private void ExtraSelector_Load(object sender, EventArgs e)
        {
            if(isCharacter)
                LoadPictures(pathConfig.CharactersAssets, flowPanelPersonajes, "Char");
            else
                LoadPictures(pathConfig.ScenesAssets, flowPanelPersonajes, "Scen");
        }

        private void LoadPictures(string directorio, FlowLayoutPanel panel, string section)
        {
            Console.WriteLine("Loading...");

            // Leer archivos de imagen del directorio
            string[] archivos = Directory.GetFiles(directorio, "*.jpg")
                                .Concat(Directory.GetFiles(directorio, "*.png"))
                                .Concat(Directory.GetFiles(directorio, "*.gif"))
                                .ToArray();

            // Crear un PictureBox para cada imagen
            foreach (string archivo in archivos)
            {
                // Crear PictureBox
                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = System.Drawing.Image.FromFile(archivo);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Width = 100;
                pictureBox.Height = 100;

                // Obtener el nombre del archivo y eliminar los espacios en blanco
                string nombreArchivo = Path.GetFileName(archivo).Replace(" ", "");

                // Agregar el nombre del archivo como texto en el PictureBox
                pictureBox.Name = section + split_char + nombreArchivo;

                // Agregar un evento de clic al PictureBox
                pictureBox.Click += PictureBox_Click;

                // Agregar PictureBox al formulario
                panel.Controls.Add(pictureBox);
            }
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            // Obtener el PictureBox que generó el evento
            PictureBox? pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Width = 100;
                pictureBox.Height = 100;
                string section = pictureBox.Name.Split(split_char)[0];
                string nombreArchivo = pictureBox.Name.Split(split_char)[1];

                PictureSelector picSel = new PictureSelector("Seleccionaste" + (isCharacter ? " al personaje: " : " la escena: ") + nombreArchivo, nombreArchivo, pathConfig,dataBase,checkJSONUnsafe);
                AssetClass assetExists = databaseAvailable.FirstOrDefault(x => x.File == nombreArchivo);
                if (assetExists != null)
                {
                    picSel.Set_AssetValues(assetExists);
                }
                picSel.ShowDialog();
                assetSelected.File = nombreArchivo;
                assetSelected.Name = picSel.name_selected;
                assetSelected.Description = picSel.descripcion_seleccionada;
                assetSelected.speed = picSel.speed;
                assetSelected.pitch = picSel.pitch;
                assetSelected.genero = picSel.genero;
                assetSelected.voice = picSel.voz;
                assetSelected.audio_asset = picSel.audio_asset;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
