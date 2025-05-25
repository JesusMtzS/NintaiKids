using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Text.Json;
using System.IO;
using Nintai_kids.Models;
using System.Text;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Nintai_kids.Modules;
using static System.Net.Mime.MediaTypeNames;
using Google.Cloud.TextToSpeech.V1;
using System.Xml.Linq;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Xml;
using System.Text.RegularExpressions;
using System.Media;
using System.Numerics;
using NAudio.Wave;
using Vlc.DotNet.Forms;
using OPENAI = OpenAI.ObjectModels;
using static OpenAI.ObjectModels.Models;

namespace Nintai_kids
{
    public partial class Form1 : Form
    {
        private VlcControl vlcControl;
        string split_char = "^";
        int character_counter = 0;
        int scene_counter = 0;
        List<AssetClass> personajes_lista = new List<AssetClass>();
        List<AssetClass> escenarios_lista = new List<AssetClass>();
        string personajes = "";
        string escenarios = "";
        OPENAI.Models.Model model;

        private GPTAPI gptAPI;
        private NintaiVideo nintai_result = new NintaiVideo();
        private string destinationFilePath = "";
        private string newAssetfileName = "";
        private string newAssetfileNameExtension = "";
        private string selectedFilePath = "";
        private AssetClass assetToAdd = new AssetClass()
        {
            pitch = "0st"
        };

        private List<AssetClass> charactersDB = new List<AssetClass>();
        private List<AssetClass> scenesDB = new List<AssetClass>();
        internal string pitch = "0st";
        internal GoogleSpeed speed { get; set; }
        internal SsmlVoiceGender genero { get; set; }
        internal GoogleVoices voz { get; set; }
        private SoundPlayer player;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        internal VideoConfigs videoConfigs = new VideoConfigs();
        internal PathConfig pathConfig = new PathConfig();
        public Form1()
        {
            InitializeComponent();
            InitializeVlcControl();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            model = OPENAI.Models.Model.Gpt_3_5_Turbo;
            gptAPI = new GPTAPI(model);
            LoadOpenAI();
            LoadPathConfig();
            LoadPictures(pathConfig.CharactersAssets, flowPanelPersonajes, "Char");
            LoadPictures(pathConfig.ScenesAssets, flowPanelEscenas, "Scen");
            LoadAddNewAssetForm();
            LoadGoogleTest();
            readDatabase();
            CargarArchivosEnComboBox(pathConfig.Ambiente);
            LoadIntros();
            comboGPTMethod.SelectedIndex = 0;
        }

        private void LoadIntros()
        {
            try
            {
                // Obtener la lista de archivos en el directorio especificado
                string[] archivos = Directory.GetFiles(pathConfig.IntrosAssets);

                // Limpiar el ComboBox
                comboIntros.Items.Clear();

                // Agregar los nombres de los archivos al ComboBox
                foreach (string archivo in archivos)
                {
                    // Obtener solo el nombre del archivo sin la ruta completa
                    string nombreArchivo = Path.GetFileName(archivo);
                    comboIntros.Items.Add(nombreArchivo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los archivos de intros: " + ex.Message);
            }
        }
        private void LoadPathConfig()
        {
            pathConfig.PythonVideoGenerator = txtPythonPath.Text.Trim();
            pathConfig.AudioAssets = txtAudioPath.Text.Trim();
            pathConfig.CharactersAssets = txtCharactersPath.Text.Trim();
            pathConfig.VideoAssets = txtVideoPath.Text.Trim();
            pathConfig.ScenesAssets = txtScenesPath.Text.Trim();
            pathConfig.ElementAssets = txtElementsPath.Text.Trim();
            pathConfig.JSONHistoryNintai = txtJSONHistoryPath.Text.Trim();
            pathConfig.JSONOutput = txtOutputPath.Text.Trim();
            pathConfig.JSONHistoryGPT = txtGTPHistoryPath.Text.Trim();
            pathConfig.JSONTextToSpeechKey = txtAPIKeyTextToSpeech.Text.Trim();
            pathConfig.OutputVoices = txtOutputVoicesPath.Text.Trim();
            pathConfig.VoicesSamples = txtVoicesSamples.Text.Trim();
            pathConfig.StoriesOutput = txtStoriesOutputPath.Text.Trim();
            pathConfig.Ambiente = txtAmbientePath.Text.Trim();
            pathConfig.IntrosAssets = txtIntrosPath.Text.Trim();
            pathConfig.ScenesDatabase = "C:\\Users\\Zagato\\source\\repos\\Nintai_kids\\Nintai_kids\\Documents\\Escenes_DB.json";
            pathConfig.CharactesDatabase = "C:\\Users\\Zagato\\source\\repos\\Nintai_kids\\Nintai_kids\\Documents\\Characters_DB.json";
            pathConfig.GenerateHistoryPath = "C:\\Users\\Zagato\\source\\repos\\Nintai_kids\\Nintai_kids\\Output\\History_Generated.json";
        }

        private void LoadGoogleTest()
        {

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

        private void LoadOpenAI()
        {
            foreach (var item in System.Enum.GetValues(typeof(OPENAI.Models.Model)))
            {
                comboOPenAi.Items.Add(item);
            }
            comboOPenAi.SelectedItem = OPENAI.Models.Model.Gpt_3_5_Turbo;
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

        // Controlador de eventos para el clic en PictureBox
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
                PictureBox newPictureBox = ClonePictureBox(pictureBox);

                FlowLayoutPanel newPanel = new FlowLayoutPanel();
                newPanel.FlowDirection = FlowDirection.TopDown;
                newPanel.BackColor = Color.Aqua;
                newPanel.Width = 150;

                Panel newPanelInfo = new Panel();
                newPanelInfo.BackColor = Color.Yellow;
                add_removeButton(newPanelInfo);

                if (section == "Scen")
                {
                    scene_counter += 1;
                    PictureSelector picSel = new PictureSelector("Seleccionaste la escena: " + nombreArchivo, nombreArchivo, pathConfig, pathConfig.ScenesDatabase, checkJSONUnsafe.Checked);

                    AssetClass assetExists = scenesDB.FirstOrDefault(x => x.File == nombreArchivo);
                    if (assetExists != null)
                    {
                        picSel.Set_AssetValues(assetExists);
                    }
                    picSel.ShowDialog();
                    if (picSel.updated)
                    {
                        //call update database lists
                        readDatabase();
                    }
                    newPanelInfo.Name = $"panelInfo_scen_{scene_counter}";
                    newPanel.Name = $"flowPanel_scen_{scene_counter}";
                    add_label(newPanelInfo, nombreArchivo, $"labelFile_scen_{scene_counter}", 0, 20);
                    add_label(newPanelInfo, picSel.name_selected, $"labelInfo_scen_{scene_counter}", 0, 40);
                    add_label(newPanelInfo, $"Scen # {scene_counter}", $"scen_{scene_counter}", 0, 0);
                    add_label(newPanelInfo, picSel.descripcion_seleccionada, $"labelDesc_scen_{scene_counter}", 0, 60);
                    add_label(newPanelInfo, picSel.audio_asset, $"labelAudio_scen_{scene_counter}", 0, 80);
                    newPanel.Controls.Add(newPictureBox);
                    newPanel.Controls.Add(newPanelInfo);
                    flowPanelEscenasSeleccionadas.Controls.Add(newPanel);
                    flowPanelEscenasSeleccionadas.FlowDirection = FlowDirection.TopDown;
                }
                if (section == "Char")
                {
                    character_counter += 1;
                    PictureSelector picSel = new PictureSelector("Seleccionaste al personaje: " + nombreArchivo, nombreArchivo, pathConfig, pathConfig.CharactesDatabase, checkJSONUnsafe.Checked);

                    AssetClass assetExists = charactersDB.FirstOrDefault(x => x.File == nombreArchivo);
                    if (assetExists != null)
                    {
                        picSel.Set_AssetValues(assetExists);
                    }

                    picSel.ShowDialog();
                    if (picSel.updated)
                    {
                        //call update database lists
                        readDatabase();
                    }
                    newPanelInfo.Name = $"panelInfo_char_{character_counter}";
                    newPanel.Name = $"flowPanel_char_{character_counter}";
                    add_label(newPanelInfo, nombreArchivo, $"labelFile_char_{character_counter}", 0, 20);
                    add_label(newPanelInfo, picSel.name_selected, $"labelInfo_char_{character_counter}", 0, 40);
                    add_label(newPanelInfo, $"Char # {character_counter}", $"char_{character_counter}", 0, 0);
                    add_label(newPanelInfo, picSel.descripcion_seleccionada, $"labelDesc_char_{character_counter}", 0, 60);
                    add_label(newPanelInfo, picSel.speed.ToString(), $"labelSpeed_scen_{character_counter}", 0, 80);
                    add_label(newPanelInfo, picSel.pitch, $"labelPitch_scen_{character_counter}", 0, 100);
                    add_label(newPanelInfo, picSel.genero.ToString(), $"labelGenero_scen_{character_counter}", 0, 120);
                    add_label(newPanelInfo, picSel.voz.ToString(), $"labelVoz_scen_{character_counter}", 0, 140);
                    newPanel.Controls.Add(newPictureBox);
                    newPanel.Controls.Add(newPanelInfo);
                    flowPanelPersonajesSeleccionados.Controls.Add(newPanel);
                    flowPanelPersonajesSeleccionados.FlowDirection = FlowDirection.TopDown;
                }
            }
        }

        private void add_label(Panel panel, string text, string name, int xPos, int yPos)
        {
            Label newLabe = new Label();
            newLabe.Name = name;
            newLabe.Text = text;
            newLabe.Location = new Point(xPos, yPos);
            panel.Controls.Add(newLabe);
        }

        private void add_removeButton(Panel panel)
        {
            Button removeButton = new Button();
            removeButton.BackColor = Color.Red;
            removeButton.ForeColor = Color.White;
            Font fuenteNegrita = new Font(removeButton.Font, FontStyle.Bold);
            removeButton.Font = fuenteNegrita;
            removeButton.Text = "X";
            removeButton.Width = 25;
            removeButton.Location = new Point(67, 0);
            removeButton.Click += remove_Picture;
            panel.Controls.Add(removeButton);
        }
        private void remove_Picture(object sender, EventArgs e)
        {
            Control padre = ((Control)sender).Parent;
            if (padre != null)
            {
                Control padreDelPadre = padre.Parent;
                string section = padreDelPadre.Name.Split("_")[1];
                if (section == "char")
                {
                    flowPanelPersonajesSeleccionados.Controls.Remove(padreDelPadre);
                }
                if (section == "scen")
                {
                    flowPanelEscenasSeleccionadas.Controls.Remove(padreDelPadre);
                }
            }
        }
        private PictureBox ClonePictureBox(PictureBox originalPictureBox)
        {
            PictureBox newPictureBox = new PictureBox();
            newPictureBox.Image = originalPictureBox.Image;
            newPictureBox.SizeMode = originalPictureBox.SizeMode;
            newPictureBox.Width = 50;
            newPictureBox.Height = 50;
            // Copiar otras propiedades según sea necesario

            return newPictureBox;
        }

        private async void btnProcessCuento_Click(object sender, EventArgs e)
        {
            if (comboGPTMethod.SelectedItem == "Tradicional")
                await GenerateHistory_GPTTradicional();
            else if (comboGPTMethod.SelectedItem == "Funciones")
                await GenerateHistory_GPTFunciones();
        }

        private async Task<bool> GenerateHistory_GPTFunciones()
        {
            progressMain.Value = 0;
            string time_name = DateTime.Now.ToString("yy_MM_dd_HH_mm_ss");
            if (Crea_directorio_Historia(pathConfig.StoriesOutput + time_name))
            {
                Crear_assets_seleccionados();
                progressMain.Value = 5;
                string primerPrompt = Crear_primer_Prompt_Funciones();
                progressMain.Value = 10;
                progressMain.Value = 25;
                progressMain.Value = 36;
                string promptEstructuraJSONSystem = $"Desde ahora eres un escritor famoso de cuentos infantiles y en tus cuentos siempre dejas una buena enseñanza";
                var response2 = await gptAPI.Generate_History_Format(primerPrompt, promptEstructuraJSONSystem, personajes_lista, escenarios_lista);
                progressMain.Value = 56;
                Escribe_GTP_JSON(response2, time_name);
                progressMain.Value = 69;
                Genera_Json_Nintai(personajes_lista, escenarios_lista, time_name);
                progressMain.Value = 100;
                MessageBox.Show("Proceso terminado!", "Proceso terminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            else
            {
                MessageBox.Show("No se pudo crear el directorio de historia", "Error al generar directorio de historia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        private async Task<bool> GenerateHistory_GPTTradicional()
        {
            progressMain.Value = 0;
            string time_name = DateTime.Now.ToString("yy_MM_dd_HH_mm_ss");
            if (Crea_directorio_Historia(pathConfig.StoriesOutput + time_name))
            {
                Crear_assets_seleccionados();
                progressMain.Value = 5;
                string primerPrompt = Crear_primer_Prompt();
                progressMain.Value = 10;
                var response = await gptAPI.Obtener_Respuesta_GPT(primerPrompt, "Desde ahora eres un escritor famoso de cuentos infantiles y en tus cuentos siempre dejas una buena enseñanza");
                progressMain.Value = 25;
                if (response != "")
                {
                    string promptFinal = Crear_segundo_Prompt(response);
                    progressMain.Value = 36;
                    string promptEstructuraJSONSystem = $"Se te dara una historya y la convertiras en formato JSON intentando no perder ninguna parte de la historia, este es el formato JSON: \n{{\r\n  \"historia\": [\r\n    {{\r\n      \"titulo\": \"\",\r\n      \"escenas\": [\r\n        {{\r\n          \"titulo\": \"\",\r\n          \"escenario\": \"\",\r\n          \"dialogos\": [\r\n            {{\r\n              \"narrador_o_personaje\": \"\",\r\n              \"dialogo\": \"\",\r\n              \"escuchan\": [\"\"]\r\n            }}\r\n          ]\r\n        }}\r\n      ]\r\n    }}\r\n  ]\r\n}} \n";
                    var response2 = await gptAPI.Obtener_Respuesta_GPT(promptFinal, promptEstructuraJSONSystem);
                    progressMain.Value = 56;
                    Escribe_GTP_JSON(response2, time_name);
                    progressMain.Value = 69;
                    Genera_Json_Nintai(personajes_lista, escenarios_lista, time_name);
                    progressMain.Value = 100;
                    MessageBox.Show("Proceso terminado!", "Proceso terminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("No se encontro respuesta de primer promt al intentar generar historia", "No respuesta encontrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("No se pudo crear el directorio de historia", "Error al generar directorio de historia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        private bool Crea_directorio_Historia(string rutaDirectorio)
        {
            try
            {
                // Verifica si el directorio no existe
                if (!Directory.Exists(rutaDirectorio))
                {
                    // Crea el directorio
                    Directory.CreateDirectory(rutaDirectorio);

                    Console.WriteLine("Directorio creado exitosamente.");
                }
                else
                {
                    Console.WriteLine("El directorio ya existe.");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrió un error al crear el directorio: " + e.Message);
                return false;
            }
        }

        private string Crear_segundo_Prompt(string historia)
        {
            string promptEstructuraJSON1 = $"Necesito que la siguiente historia me la conviertas en formato JSON.\nEsta es la historia:\n";
            string promptHistoriaGPT = $"\n{historia}\n";
            string promptEstructuraJSON2 = $"Este es el formato JSON: \n{{\r\n  \"historia\": [\r\n    {{\r\n      \"titulo\": \"\",\r\n      \"escenas\": [\r\n        {{\r\n          \"titulo\": \"\",\r\n          \"escenario\": \"\",\r\n          \"dialogos\": [\r\n            {{\r\n              \"narrador_o_personaje\": \"\",\r\n              \"dialogo\": \"\",\r\n              \"escuchan\": [\"\"]\r\n            }}\r\n          ]\r\n        }}\r\n      ]\r\n    }}\r\n  ]\r\n}} \n";
            string promptFinal = $"{promptEstructuraJSON1}{promptHistoriaGPT}{promptEstructuraJSON2}";

            richTextBox1.AppendText("Segundo prompt para obtener estructura de historia:");
            richTextBox1.AppendText("...");
            richTextBox1.AppendText(promptFinal);

            return promptFinal;
        }

        private string Crear_primer_Prompt()
        {
            string edadMeta = numEdadMeta.Value.ToString();
            string longitudPalabras = numDuracion.Value.ToString();
            string personajesQty = flowPanelPersonajesSeleccionados.Controls.Count.ToString();
            string escenasQty = flowPanelEscenasSeleccionadas.Controls.Count.ToString();

            string promptInicio = $"Ayúdame a crear un cuento infantil llamado: {txtTitle.Text.Trim()}.\n Orientado a niños de {edadMeta} años " +
                $"que tenga una longitud de {longitudPalabras} palabras, en donde participaran exactamente {personajesQty} personajes " +
                $"y todo sucedera en {escenasQty} escenarios.\n";
            string promptEscenarios = $"Los escenarios y sus descripciones son: \n {escenarios} \n";
            string promptPersonajes = $"Los personajes y sus descripciones son: \n {personajes} \n";

            string promptEstructura = $"No es necesario que presentes a los personajes detallando su descripcion, con solo el nombre es suficiente.\nNecesito que la historia este dividida en escenas donde me indiques que escenario está usando y que dividas toda la historia en diálogos donde " +
                $"cada dialogo es parte de un personaje o del narrador. \n Esta es la parte más importante, antes de cada dialogo del narrador y de los personajes pon el nombre del personaje o la palabra narrador, además de indicar en cada dialogo quienes están escuchando usando como máximo 2 personajes que están escuchando.";

            string promptHistoria = $"La descripción de la historia es la siguiente:\n {txtDescripcion.Text} ";
            string primpt_regreso = $"{promptInicio}{promptEscenarios}{promptPersonajes}{promptEstructura}{promptHistoria}";

            richTextBox1.AppendText("Primer prompt para obtener historia:");
            richTextBox1.AppendText("...");
            richTextBox1.AppendText(primpt_regreso);

            return primpt_regreso;
        }

        private string Crear_primer_Prompt_Funciones()
        {
            string edadMeta = numEdadMeta.Value.ToString();
            string longitudPalabras = numDuracion.Value.ToString();
            string personajesQty = flowPanelPersonajesSeleccionados.Controls.Count.ToString();
            string escenasQty = flowPanelEscenasSeleccionadas.Controls.Count.ToString();

            string promptInicio = $"Ayúdame a crear un cuento infantil llamado: {txtTitle.Text.Trim()}.\n Orientado a niños de {edadMeta} años " +
                $"que tenga una longitud de {longitudPalabras} palabras, en donde participaran exactamente {personajesQty} personajes " +
                $"y todo sucedera en {escenasQty} escenarios.\n";
            string promptEscenarios = $"Los escenarios y sus descripciones son: \n {escenarios} \n";
            string promptPersonajes = $"Los personajes y sus descripciones son: \n {personajes} \n";

            string promptEstructura = $"No es necesario que presentes a los personajes detallando su descripcion, con solo el nombre es suficiente.\n ";
            string promptHistoria = $"La descripción de la historia es la siguiente:\n {txtDescripcion.Text} ";
            string primpt_regreso = $"{promptInicio}{promptEscenarios}{promptPersonajes}{promptEstructura}{promptHistoria}";

            richTextBox1.AppendText("Primer prompt para obtener historia:");
            richTextBox1.AppendText("...");
            richTextBox1.AppendText(primpt_regreso);

            return primpt_regreso;
        }

        private void Escribe_GTP_JSON(string jsonOutput, string time_name)
        {
            string fileHistorypath = pathConfig.JSONHistoryGPT + time_name + ".json";
            string fileOutputpath = pathConfig.JSONOutput + "History_Generated" + ".json";
            string fileOutputpath_Story = pathConfig.StoriesOutput + time_name + "\\History_Generated" + ".json";
            using (StreamWriter writer = new StreamWriter(fileHistorypath, false, Encoding.GetEncoding("ISO-8859-1")))
            {
                writer.Write(jsonOutput);
            }
            using (StreamWriter writer = new StreamWriter(fileOutputpath, false, Encoding.GetEncoding("ISO-8859-1")))
            {
                writer.Write(jsonOutput);
            }
            using (StreamWriter writer = new StreamWriter(fileOutputpath_Story, false, Encoding.GetEncoding("ISO-8859-1")))
            {
                writer.Write(jsonOutput);
            }
        }
        private void Crear_assets_seleccionados()
        {
            escenarios_lista.Clear();
            personajes_lista.Clear();
            foreach (Control con in flowPanelPersonajesSeleccionados.Controls)
            {
                Control infoPanel = con.Controls[1];
                string personaje = "";
                string descripcion = "";
                AssetClass asset_local = new AssetClass();
                foreach (Control infoCon in infoPanel.Controls)
                {
                    if (infoCon.Name.Contains("labelInfo"))
                    {
                        personaje = infoCon.Text;
                        asset_local.Name = infoCon.Text;

                    }
                    if (infoCon.Name.Contains("labelDesc"))
                    {
                        asset_local.Description = infoCon.Text;
                        descripcion = infoCon.Text;
                    }
                    if (infoCon.Name.Contains("labelFile"))
                    {
                        asset_local.File = infoCon.Text;
                    }
                    if (infoCon.Name.Contains("labelVoz"))
                    {
                        asset_local.voice = (GoogleVoices)Enum.Parse(typeof(GoogleVoices), infoCon.Text);
                    }
                    if (infoCon.Name.Contains("labelGenero"))
                    {
                        asset_local.genero = (SsmlVoiceGender)Enum.Parse(typeof(SsmlVoiceGender), infoCon.Text);
                    }
                    if (infoCon.Name.Contains("labelSpeed"))
                    {
                        asset_local.speed = (GoogleSpeed)Enum.Parse(typeof(GoogleSpeed), infoCon.Text);
                    }
                    if (infoCon.Name.Contains("labelPitch"))
                    {
                        asset_local.pitch = infoCon.Text;
                    }
                }
                personajes_lista.Add(asset_local);
                personajes += $"Nombre de personaje: {personaje}, descripcion: {descripcion}. \n";
            }

            foreach (Control con in flowPanelEscenasSeleccionadas.Controls)
            {
                Control infoPanel = con.Controls[1];
                string escenario = "";
                string descripcion = "";
                AssetClass asset_local = new AssetClass();
                foreach (Control infoCon in infoPanel.Controls)
                {
                    if (infoCon.Name.Contains("labelInfo"))
                    {
                        escenario = infoCon.Text;
                        asset_local.Name = infoCon.Text;
                    }
                    if (infoCon.Name.Contains("labelDesc"))
                    {
                        descripcion = infoCon.Text;
                        asset_local.Description = infoCon.Text;
                    }
                    if (infoCon.Name.Contains("labelFile"))
                    {
                        asset_local.File = infoCon.Text;
                    }
                    if (infoCon.Name.Contains("labelAudio"))
                    {
                        asset_local.audio_asset = infoCon.Text;
                    }
                }
                escenarios_lista.Add(asset_local);
                escenarios += $"escenario: {escenario}, descripcion: {descripcion}. \n";
            }
        }
        private void Genera_Json_Nintai(List<AssetClass> personajes_lista, List<AssetClass> escenarios_lista, string time_name)
        {
            try
            {
                using (StreamReader sr = new StreamReader(pathConfig.GenerateHistoryPath, Encoding.Latin1))
                {
                    string json = sr.ReadToEnd();
                    //GPTHistory datos = System.Text.Json.JsonSerializer.Deserialize<GPTHistory>(json,
                    //     new JsonSerializerOptions
                    //     {
                    //         Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
                    //     });

                    // Deserializar el JSON a un objeto C#
                    GPTHistory datos = System.Text.Json.JsonSerializer.Deserialize<GPTHistory>(json);

                    HistoryConverter historyConverter = new HistoryConverter(datos, videoConfigs, personajes_lista, escenarios_lista, pathConfig, checkJSONUnsafe.Checked);
                    nintai_result = historyConverter.convert_GPT_to_Nintai(time_name);

                    Console.WriteLine("Creando audios...");
                    VoiceGenerator voiceGenerator = new VoiceGenerator(pathConfig);
                    voiceGenerator.Synthesize_Nintai_Video(nintai_result);

                    var options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true // Opcional: para una salida JSON indentada
                    };

                    // Serializar el objeto a JSON utilizando las opciones configuradas
                    string jsonOutput = "";
                    if (checkJSONUnsafe.Checked)
                        jsonOutput = System.Text.Json.JsonSerializer.Serialize(nintai_result, options);
                    else
                        jsonOutput = System.Text.Json.JsonSerializer.Serialize(nintai_result);



                    Console.WriteLine(jsonOutput);
                    string fileHistorypath = pathConfig.JSONHistoryNintai + DateTime.Now.ToString("yy_MM_dd_HH_mm_ss") + ".json";
                    string fileOutputpath = pathConfig.JSONOutput + "Video_Generated" + ".json";
                    string fileOutputpath_Story = pathConfig.StoriesOutput + time_name + "\\Video_Generated" + ".json";
                    //FileStreamOptions fileOptions = new FileStreamOptions()
                    //{
                    //    Share = FileShare.None,
                    //    BufferSize=4096,
                    //    Access = FileAccess.Write,
                    //    Mode =FileMode.CreateNew,
                    //    PreallocationSize = 0,
                    //    Options =  FileOptions.None,
                    //};
                    //using (StreamWriter writer = new StreamWriter(fileOutputpath,Encoding.Latin1, fileOptions))
                    //{
                    //    writer.Write(jsonString);
                    //}
                    using (StreamWriter writer = new StreamWriter(fileHistorypath, false, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        writer.Write(jsonOutput);
                    }
                    using (StreamWriter writer = new StreamWriter(fileOutputpath, false, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        writer.Write(jsonOutput);
                    }
                    using (StreamWriter writer = new StreamWriter(fileOutputpath_Story, false, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        writer.Write(jsonOutput);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("El archivo no existe.");
            }
            catch (System.Text.Json.JsonException)
            {
                Console.WriteLine("El archivo JSON está mal formado.");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string time_name = DateTime.Now.ToString("yy_MM_dd_HH_mm_ss");
            Crear_assets_seleccionados();
            Genera_Json_Nintai(personajes_lista, escenarios_lista, time_name);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            GPTAPI gptAPI = new GPTAPI(model);
            var response = await gptAPI.Obtener_Respuesta_GPT(txtTestUserMessage.Text.Trim(), txtTestSystemMessage.Text.Trim());
            richTextBox1.AppendText("-----------------------------------------------------");
            richTextBox1.AppendText(response);
            richTextBox1.AppendText("-----------------------------------------------------");
        }

        private void btProcesaVoice_Click(object sender, EventArgs e)
        {
            if (txtNombreAudio.Text.Trim() != "" && txtTextToVoiceTest.Text.Trim() != "")
            {
                VoiceGenerator voiceGenerator = new VoiceGenerator(pathConfig);
                //voiceGenerator.Generate_voice(txtTextToVoiceTest.Text.Trim());
                string Ssmldialog = voiceGenerator.Covert_to_Ssml(txtTextToVoiceTest.Text.Trim(), pitch, speed.ToString());
                string sample_path = Path.Combine(pathConfig.VoicesSamples, Clean_gender(voz.ToString()).Replace('_', '-'), speed.ToString(), lblPitchValue.Text.Trim() + ".wav");
                voiceGenerator.SynthesizeText(Ssmldialog, pathConfig.OutputVoices + txtNombreAudio.Text.Trim() + ".wav", Clean_gender(voz.ToString()).Replace('_', '-'), genero, sample_path);
            }
            else
            {
                MessageBox.Show("Especifica un nombre de Audio y y un texto a convertir para poder continuar", "Requerido:");
            }
        }

        private void trackPitchValue_Scroll(object sender, EventArgs e)
        {
            lblPitchValue.Text = trackPitchValue.Value.ToString() + "st";
            pitch = lblPitchValue.Text;
        }

        private void comboGenero_SelectedIndexChanged(object sender, EventArgs e)
        {
            genero = (SsmlVoiceGender)comboGenero.SelectedItem;
        }

        private void comboVoces_SelectedIndexChanged(object sender, EventArgs e)
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
        }

        private void comboSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            speed = (GoogleSpeed)comboSpeed.SelectedItem;
        }

        private string Clean_gender(string voice_name)
        {
            return voice_name.Replace("_MALE", "").Replace("_FEMALE", "");
        }

        private void BtnAddCharacter_Click(object sender, EventArgs e)
        {
            AddNewAsset_Process(pathConfig.CharactesDatabase, pathConfig.CharactersAssets);
        }

        private void AddNewAsset_Process(string database, string path_to_store_images)
        {
            if (IsValid_Asset())
            {
                List<AssetClass> assetList;
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true // Opcional: para una salida JSON indentada
                };

                // Lee la lista existente desde el archivo JSON
                using (StreamReader reader = new StreamReader(database))
                {
                    string json = reader.ReadToEnd();
                    assetList = System.Text.Json.JsonSerializer.Deserialize<List<AssetClass>>(json);
                }

                //valida que el nombre de archivo no exista ya
                if (!fileNameExiste(assetList))
                {
                    if (txtFileName.Text.Trim() != "")
                    {
                        // Agrega el nuevo elemento a la lista
                        assetList.Add(assetToAdd);

                        // Serializa la lista actualizada y escribe al archivo JSON
                        string jsonString = "";
                        if (checkJSONUnsafe.Checked)
                            jsonString = System.Text.Json.JsonSerializer.Serialize(assetList, options);
                        else
                            jsonString = System.Text.Json.JsonSerializer.Serialize(assetList);

                        System.IO.File.WriteAllText(database, jsonString);

                        // Ahora agregamos la imagen al catalogo
                        // Construir la ruta de destino completa
                        destinationFilePath = Path.Combine(path_to_store_images, txtFileName.Text.Trim() + newAssetfileNameExtension);

                        // Copiar el archivo al directorio de destino
                        File.Copy(selectedFilePath, destinationFilePath, true);

                        MessageBox.Show("Proceso terminado!", "Proceso terminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Especifica un nombre de archivo.", "No hay nombre de archivo seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("La imagen o asset que estas intentando agregar ya existe, por favor verifica el nombre del archivo fisicamente.", "YA existe el asset file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Asegurate de que todos los datos requeridos del asset han sido especificados!", "Datos faltantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool IsValid_Asset()
        {
            return (txtName.Text.Trim() != "" &&
                txtDescription.Text.Trim() != "" &&
                comboGeneroAdd.SelectedIndex >= 0 &&
                comboSpeedAdd.SelectedIndex >= 0 &&
                comboVoz.SelectedIndex >= 0 &&
                pictureBoxAsset.Image != null
                );
        }

        private bool fileNameExiste(List<AssetClass> assetList)
        {
            foreach (AssetClass element in assetList)
            {
                if (element.File.Trim() == newAssetfileName.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        private void LoadAddNewAssetForm()
        {

            foreach (var item in System.Enum.GetValues(typeof(GoogleSpeed)))
            {
                comboSpeedAdd.Items.Add(item);
            }

            foreach (var item in System.Enum.GetValues(typeof(SsmlVoiceGender)))
            {
                comboGeneroAdd.Items.Add(item);
            }

            foreach (var item in System.Enum.GetValues(typeof(GoogleVoices)))
            {
                comboVoz.Items.Add(item);
            }

            comboGeneroAdd.SelectedIndex = 0;
            comboSpeedAdd.SelectedItem = GoogleSpeed.medium;
            comboVoz.SelectedIndex = 0;
        }

        private void BtAddPicture_Click(object sender, EventArgs e)
        {
            // Crear un OpenFileDialog
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Establecer propiedades del OpenFileDialog
            openFileDialog1.Title = "Seleccionar imagen";
            openFileDialog1.Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif";
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            // Mostrar el diálogo de selección de archivo
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Asignar la imagen seleccionada al PictureBox
                    pictureBoxAsset.Image = new System.Drawing.Bitmap(openFileDialog1.FileName);
                    // Obtener la ruta del archivo seleccionado
                    selectedFilePath = openFileDialog1.FileName;

                    // Obtener el nombre del archivo de la ruta seleccionada
                    newAssetfileName = Path.GetFileName(selectedFilePath);
                    txtFileName.Text = newAssetfileName.Split(".")[0];
                    newAssetfileNameExtension = Path.GetExtension(selectedFilePath);
                    lblExtension.Text = newAssetfileNameExtension;
                    assetToAdd.File = newAssetfileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar la imagen: " + ex.Message);
                }
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            assetToAdd.Name = txtName.Text.Trim();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            assetToAdd.Description = txtDescription.Text.Trim();
        }

        private void comboVoz_SelectedIndexChanged(object sender, EventArgs e)
        {
            assetToAdd.voice = (GoogleVoices)comboVoz.SelectedItem;
            if (assetToAdd.voice.ToString().Contains("_FEMALE"))
            {
                comboGeneroAdd.SelectedItem = SsmlVoiceGender.Female;
            }

            if (assetToAdd.voice.ToString().Contains("_MALE"))
            {
                comboGeneroAdd.SelectedItem = SsmlVoiceGender.Male;
            }
        }

        private void comboSpeedAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            assetToAdd.speed = (GoogleSpeed)comboSpeedAdd.SelectedItem;
        }

        private void comboGeneroAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            assetToAdd.genero = (SsmlVoiceGender)comboGeneroAdd.SelectedItem;
        }

        private void trackAdd_Scroll(object sender, EventArgs e)
        {
            lblPitchAdd.Text = trackAdd.Value.ToString() + "st";
            assetToAdd.pitch = lblPitchAdd.Text;
        }

        private void btnAddNewScene_Click(object sender, EventArgs e)
        {
            AddNewAsset_Process(pathConfig.ScenesDatabase, pathConfig.ScenesAssets);
        }

        private void refreshCatalogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flowPanelPersonajes.Controls.Clear();
            flowPanelEscenas.Controls.Clear();
            LoadPictures(pathConfig.CharactersAssets, flowPanelPersonajes, "Char");
            LoadPictures(pathConfig.ScenesAssets, flowPanelEscenas, "Scen");
            readDatabase();
            CargarArchivosEnComboBox(pathConfig.Ambiente);

        }

        private void readDatabase()
        {
            // Lee la lista existente desde el archivo JSON
            using (StreamReader reader = new StreamReader(pathConfig.CharactesDatabase))
            {
                string json = reader.ReadToEnd();
                charactersDB = System.Text.Json.JsonSerializer.Deserialize<List<AssetClass>>(json);
            }
            // Lee la lista existente desde el archivo JSON
            using (StreamReader reader = new StreamReader(pathConfig.ScenesDatabase))
            {
                string json = reader.ReadToEnd();
                scenesDB = System.Text.Json.JsonSerializer.Deserialize<List<AssetClass>>(json);
            }
        }

        private void btResetName_Click(object sender, EventArgs e)
        {
            txtFileName.Text = newAssetfileName;
        }

        private void btnPlay_Click(object sender, EventArgs e)
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

        private void comboBoxAmbiente_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Realizar la acción deseada cuando se selecciona un archivo
            string nombreArchivoSeleccionado = comboBoxAmbiente.SelectedItem.ToString();
            assetToAdd.audio_asset = nombreArchivoSeleccionado;
        }

        private void btnPlayAdd_Click(object sender, EventArgs e)
        {
            string rutaArchivoAudio = Path.Combine(pathConfig.VoicesSamples, Clean_gender(assetToAdd.voice.ToString()).Replace('_', '-'), assetToAdd.speed.ToString(), assetToAdd.pitch.ToString() + ".wav");
            if (File.Exists(rutaArchivoAudio))
            {
                player = new SoundPlayer(rutaArchivoAudio);
                player.Play();
            }
            else
            {
                if (MessageBox.Show("El archivo ejemplo de audio no existe, quieres reproducir el archivo base? (speed: medium, pitch:0st)", "Archivo no encontrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string rutaArchivoAudiobase = pathConfig.VoicesSamples + Clean_gender(assetToAdd.voice.ToString()).Replace('_', '-') + ".wav";
                    player = new SoundPlayer(rutaArchivoAudiobase);
                    player.Play();
                }
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

        private void btnStop_Click(object sender, EventArgs e)
        {
            // Detener la reproducción del audio si está en curso
            if (outputDevice != null && outputDevice.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Stop();
            }
        }
        private void txtAudioPath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.AudioAssets = txtAudioPath.Text.Trim();
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            videoConfigs.VideoTitle = txtTitle.Text.Trim();
        }

        private void txtPythonPath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.PythonVideoGenerator = txtPythonPath.Text.Trim();
        }

        private void txtCharactersPath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.CharactersAssets = txtCharactersPath.Text.Trim();
        }

        private void txtVideoPath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.VideoAssets = txtVideoPath.Text.Trim();
        }

        private void txtScenesPath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.ScenesAssets = txtScenesPath.Text.Trim();
        }

        private void txtElementsPath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.ElementAssets = txtElementsPath.Text.Trim();
        }

        private void txtJSONHistoryPath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.JSONHistoryNintai = txtJSONHistoryPath.Text.Trim();
        }

        private void txtOutputPath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.JSONOutput = txtOutputPath.Text.Trim();
        }

        private void txtGTPHistoryPath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.JSONHistoryGPT = txtGTPHistoryPath.Text.Trim();
        }

        private void txtAPIKeyTextToSpeech_TextChanged(object sender, EventArgs e)
        {
            pathConfig.JSONTextToSpeechKey = txtAPIKeyTextToSpeech.Text.Trim();
        }

        private void txtOutputVoicesPath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.OutputVoices = txtOutputVoicesPath.Text.Trim();
        }

        private void txtVoicesSamples_TextChanged(object sender, EventArgs e)
        {
            pathConfig.VoicesSamples = txtVoicesSamples.Text.Trim();
        }

        private void txtStoriesOutputPath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.StoriesOutput = txtStoriesOutputPath.Text.Trim();
        }

        private void txtAmbientePath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.Ambiente = txtAmbientePath.Text.Trim();
        }

        private void txtIntrosPath_TextChanged(object sender, EventArgs e)
        {
            pathConfig.IntrosAssets = txtIntrosPath.Text.Trim();
        }

        private void checkIntro_CheckedChanged(object sender, EventArgs e)
        {
            comboIntros.Enabled = checkIntro.Checked;
            btPlayIntro.Enabled = checkIntro.Checked;
            if (!checkIntro.Checked)
                videoConfigs.VideoIntroAsset = "";
        }

        private void comboIntros_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSelectedIntro.Text = comboIntros.SelectedItem.ToString();
            videoConfigs.VideoIntroAsset = comboIntros.SelectedItem.ToString();
        }

        private void InitializeVlcControl()
        {
            vlcControl = new VlcControl();
            vlcControl.Dock = DockStyle.Fill;
            vlcControl.VlcLibDirectoryNeeded += OnVlcControlVlcLibDirectoryNeeded;
            vlcControl.VlcMediaplayerOptions = new[] { "--no-osd" }; // Opciones adicionales si es necesario
            Controls.Add(vlcControl);
        }
        private void btPlayIntro_Click(object sender, EventArgs e)
        {
            if (videoConfigs.VideoIntroAsset != "" && videoConfigs.VideoIntroAsset != null)
                ReproducirVideo(Path.Combine(pathConfig.IntrosAssets, videoConfigs.VideoIntroAsset));
        }
        private void OnVlcControlVlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new DirectoryInfo(@"C:\Program Files\VideoLAN\VLC\");
        }

        private void ReproducirVideo(string rutaVideo)
        {
            try
            {
                vlcControl.SetMedia(new Uri(rutaVideo));
                vlcControl.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al reproducir el video: " + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            vlcControl.Dispose();
        }

        private async void pictureBox3_Click(object sender, EventArgs e)
        {
            await executeChat();
        }
        private async Task<bool> executeChat()
        {
            if (txtUserChat.Text.Trim() != "")
            {
                //Submit chat
                GPTAPI gptAPI = new GPTAPI(model);
                string systemPrompt = "";
                //Get system prompt
                systemPrompt = getSystemPrompt();

                var response = await gptAPI.Obtener_Respuesta_GPT(txtUserChat.Text.Trim(), systemPrompt);
                txtBotChat.SelectionFont = new Font(txtBotChat.Font, FontStyle.Bold);
                txtBotChat.SelectionColor = Color.Blue;
                txtBotChat.AppendText($"\nYou: {txtUserChat.Text.Trim()}");
                txtBotChat.SelectionFont = txtBotChat.Font;
                txtBotChat.SelectionColor = txtBotChat.ForeColor;
                txtBotChat.AppendText($"\nChuyGPT: {response}");
                txtBotChat.AppendText("\n-----------------------------------------------------");
                txtUserChat.Text = "";
                txtBotChat.ScrollToCaret();
                txtUserChat.Focus();
            }
            return true;
        }
        private string getSystemPrompt()
        {
            string temp = "";
            bool modified = false;
            if (spCategory1.Text.Trim() != "")
            {
                if (spDescription1.Text.Trim() != "")
                {
                    temp = $"\n\"{spCategory1.Text.Trim()}\" : \"{spDescription1.Text.Trim()}\",";
                    modified = true;
                }
            }
            if (spCategory2.Text.Trim() != "")
            {
                if (spDescription2.Text.Trim() != "")
                {
                    temp = $"\n\"{spCategory2.Text.Trim()}\" : \"{spDescription2.Text.Trim()}\",";
                    modified = true;
                }
            }
            if (spCategory3.Text.Trim() != "")
            {
                if (spDescription3.Text.Trim() != "")
                {
                    temp = $"\n\"{spCategory3.Text.Trim()}\" : \"{spDescription3.Text.Trim()}\",";
                    modified = true;
                }
            }
            if (spCategory4.Text.Trim() != "")
            {
                if (spDescription4.Text.Trim() != "")
                {
                    temp = $"\n\"{spCategory4.Text.Trim()}\" : \"{spDescription4.Text.Trim()}\",";
                    modified = true;
                }
            }
            if (modified)
            {
                if (temp.EndsWith(","))
                {
                    temp = temp.TrimEnd(',');
                }
                temp = $"{{ {temp} \n }}";
            }
            return temp;
        }

        private async void txtUserChat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                await executeChat();
            }
        }

        private void picClear_Click(object sender, EventArgs e)
        {
            txtBotChat.Text = "";
        }

        private void comboOPenAi_SelectedIndexChanged(object sender, EventArgs e)
        {
            model = (OPENAI.Models.Model)comboOPenAi.SelectedItem;
        }
    }
}