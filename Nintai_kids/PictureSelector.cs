using Google.Cloud.TextToSpeech.V1;
using Google.Protobuf.WellKnownTypes;
using NAudio.Wave;
using Nintai_kids.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using NAudio.Wave;

namespace Nintai_kids
{
    internal partial class PictureSelector : Form
    {
        internal string name_selected = "";
        internal string descripcion_seleccionada = "";
        internal string pitch = "0st";
        internal string audio_asset = "";
        internal GoogleSpeed speed { get; set; }
        internal SsmlVoiceGender genero { get; set; }
        internal GoogleVoices voz { get; set; }
        internal bool updated = false;
        private string element_name;
        private string element_text;
        internal PathConfig pathConfig = new PathConfig();
        private SoundPlayer player;
        private string dataBase = "";
        private bool checkJSONUnsafe = false;
        private AssetClass temporalAsset = null;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        public PictureSelector(string text, string name,PathConfig _patConfig, string _dataBase, bool _checkJSONUnsafe)
        {
            InitializeComponent();
            element_text = text;
            element_name = name;
            pathConfig = _patConfig;
            dataBase = _dataBase;
            checkJSONUnsafe = _checkJSONUnsafe;
        }
        internal void Set_AssetValues(AssetClass assetValues)
        {
            temporalAsset = assetValues;
        }
        internal void LoadUsingAsset()
        {
            if (temporalAsset != null)
            {
                txtDescripcion.Text = temporalAsset.Description;
                txtName.Text = temporalAsset.Name;
                comboSpeed.SelectedItem = temporalAsset.speed;
                comboVoces.SelectedItem = temporalAsset.voice;
                comboGenero.SelectedItem = temporalAsset.genero;
                if (temporalAsset.pitch != null && temporalAsset.pitch != "")
                {
                    trackPitchValue.Value = int.Parse(temporalAsset.pitch.Replace("st", ""));
                    lblPitchValue.Text = temporalAsset.pitch;
                    pitch = temporalAsset.pitch;
                }
                comboBoxAmbiente.SelectedItem = temporalAsset.audio_asset;
            }
        }

        private void PictureSelector_Load(object sender, EventArgs e)
        {
            CargarArchivosEnComboBox(pathConfig.Ambiente);
            LoadSelectorForm();
            LoadUsingAsset();
        }
        private void LoadSelectorForm()
        {
            lbltext.Text = element_text;

            foreach (var item in System.Enum.GetValues(typeof(GoogleSpeed)))
            {
                comboSpeed.Items.Add(item);
            }

            foreach (var item in System.Enum.GetValues(typeof(SsmlVoiceGender)))
            {
                comboGenero.Items.Add(item);
            }

            foreach (var item in System.Enum.GetValues(typeof(GoogleVoices)))
            {
                comboVoces.Items.Add(item);
            }

            //comboGenero.SelectedIndex = 0;
            comboSpeed.SelectedItem = GoogleSpeed.medium;
            comboVoces.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            submit_result();
        }
        private void submit_result()
        {
            if (txtName.Text.Trim() == "" || txtDescripcion.Text.Trim() == "")
            {
                MessageBox.Show("Porfavor asigna un nombre y una descripcion al item para poder agregarlo");

            }
            else
            {
                name_selected = txtName.Text.Trim();
                descripcion_seleccionada = txtDescripcion.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                submit_result();
            }
        }

        private void PictureSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopAmbiente();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void trackPitchValue_Scroll(object sender, EventArgs e)
        {
            lblPitchValue.Text = trackPitchValue.Value.ToString() + "st";
            pitch = lblPitchValue.Text;
        }

        private void comboSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboSpeed.SelectedItem != null)
                speed = (GoogleSpeed)comboSpeed.SelectedItem;
        }

        private void comboGenero_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboGenero.SelectedItem != null)
                genero = (SsmlVoiceGender)comboGenero.SelectedItem;
        }

        private void comboVoces_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboVoces.SelectedItem != null)
            {
                voz = (GoogleVoices)comboVoces.SelectedItem;
                if (voz.ToString().Contains("_FEMALE"))
                {
                    comboGenero.SelectedItem = SsmlVoiceGender.Female;
                }

                if (voz.ToString().Contains("_MALE"))
                {
                    comboGenero.SelectedItem = SsmlVoiceGender.Male;
                }

                if (voz.ToString().ToLower().Contains("studio"))
                {
                    Alert("Esta voz no permite \nel uso de pitch");
                }
                else
                    Alert("",true);
            }
        }

        private void Alert(string alertMessage,bool hide = false)
        {
            if(hide)
            {
                picAlert.Visible = false;
                panelAlert.Visible = false;
                lblAlert.Visible = false;
            }
            else
            {
                picAlert.Visible = true;
                panelAlert.Visible = true;
                lblAlert.Text = alertMessage;
                lblAlert.Visible = true;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (voz.ToString() != "")
            {
                string rutaArchivoAudio = Path.Combine(pathConfig.VoicesSamples, Clean_gender(voz.ToString()).Replace('_', '-'), speed.ToString(), lblPitchValue.Text.ToString() + ".wav");
                if (File.Exists(rutaArchivoAudio))
                {
                    player = new SoundPlayer(rutaArchivoAudio);
                    player.Play();
                }
                else
                {
                    if (MessageBox.Show("El archivo ejemplo de audio no existe, quieres reproducir el archivo base? (speed: medium, pitch:0st)", "Archivo no encontrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string rutaArchivoAudiobase = pathConfig.VoicesSamples + Clean_gender(voz.ToString()).Replace('_', '-') + ".wav";
                        player = new SoundPlayer(rutaArchivoAudiobase);
                        player.Play();
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona una voz primero.");
            }
        }

        private string Clean_gender(string voice_name)
        {
            return voice_name.Replace("_MALE", "").Replace("_FEMALE", "");
        }

        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            List<AssetClass> assetList;
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true // Opcional: para una salida JSON indentada
            };

            // Lee la lista existente desde el archivo JSON
            using (StreamReader reader = new StreamReader(dataBase))
            {
                string json = reader.ReadToEnd();
                assetList = System.Text.Json.JsonSerializer.Deserialize<List<AssetClass>>(json);
            }

            AssetClass result = assetList.FirstOrDefault(asset => asset.File == temporalAsset.File);
            //valida que el nombre de archivo no exista ya
            if (result != null)
            {
                // Agrega el nuevo elemento a la lista
                result.Name = txtName.Text.Trim();
                result.Description = txtDescripcion.Text.Trim();
                result.speed = (GoogleSpeed)comboSpeed.SelectedItem;
                result.voice = (GoogleVoices)comboVoces.SelectedItem;
                result.pitch = lblPitchValue.Text;
                result.genero = (SsmlVoiceGender)comboGenero.SelectedItem;
                if (comboBoxAmbiente.SelectedItem != null)
                    result.audio_asset = comboBoxAmbiente.SelectedItem.ToString();

                // Serializa la lista actualizada y escribe al archivo JSON
                string jsonString = "";
                if (checkJSONUnsafe)
                    jsonString = System.Text.Json.JsonSerializer.Serialize(assetList, options);
                else
                    jsonString = System.Text.Json.JsonSerializer.Serialize(assetList);

                System.IO.File.WriteAllText(dataBase, jsonString);

                // Ahora agregamos la imagen al catalogo
                // Construir la ruta de destino completa
                updated = true;
                MessageBox.Show("Proceso terminado!", "Proceso terminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("La imagen o asset que estas intentando actualizar no existe, por favor verifica el nombre del archivo fisicamente.", "Registro no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btPlayAmbiente_Click(object sender, EventArgs e)
        {
            if (comboBoxAmbiente.SelectedItem != null)
            {
                string rutaArchivoAudio = Path.Combine(pathConfig.Ambiente, comboBoxAmbiente.SelectedItem.ToString());
                if (File.Exists(rutaArchivoAudio))
                {
                    //player = new SoundPlayer(rutaArchivoAudio);
                    //player.Play();
                    ReproducirArchivo(rutaArchivoAudio);
                }
                else
                {
                    MessageBox.Show("El archivo ejemplo de audio no existe", "Archivo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopAmbiente();
        }

        private void StopAmbiente()
        {
            // Detener la reproducción del audio si está en curso
            if (outputDevice != null && outputDevice.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Stop();
            }
        }

        private void comboBoxAmbiente_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Realizar la acción deseada cuando se selecciona un archivo
            if (comboBoxAmbiente.SelectedItem != null)
            {
                string nombreArchivoSeleccionado = comboBoxAmbiente.SelectedItem.ToString();
                audio_asset = nombreArchivoSeleccionado;
                //temporalAsset.audio_asset = nombreArchivoSeleccionado;
            }
            //
        }

        private void ReproducirArchivo(string rutaArchivo)
        {
            if (File.Exists(rutaArchivo))
            {
                // Detener la reproducción si ya se está reproduciendo otro archivo
                if (outputDevice != null)
                {
                    outputDevice.Stop();
                    outputDevice.Dispose();
                }

                // Crear un nuevo dispositivo de salida y un lector de archivos de audio
                outputDevice = new WaveOutEvent();
                audioFile = new AudioFileReader(rutaArchivo);

                // Conectar el lector de archivos de audio al dispositivo de salida
                outputDevice.Init(audioFile);

                // Reproducir el archivo de audio
                outputDevice.Play();
            }
            else
            {
                MessageBox.Show("El archivo no existe.");
            }
        }

        private void CargarArchivosEnComboBox(string directorio)
        {
            try
            {
                // Obtener la lista de archivos en el directorio especificado
                string[] archivos = Directory.GetFiles(directorio);

                // Limpiar el ComboBox
                comboBoxAmbiente.Items.Clear();

                // Agregar los nombres de los archivos al ComboBox
                foreach (string archivo in archivos)
                {
                    // Obtener solo el nombre del archivo sin la ruta completa
                    string nombreArchivo = Path.GetFileName(archivo);
                    comboBoxAmbiente.Items.Add(nombreArchivo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los archivos: " + ex.Message);
            }
        }
    }
}
