using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Google.Cloud.TextToSpeech.V1;
using Nintai_kids.Models;

namespace Nintai_kids.Modules
{
    internal class HistoryConverter
    {
        private GPTHistory gptHistory;
        private List<AssetClass> personajes_disponibles;
        private List<AssetClass> escenas_disponibles;
        private bool checkJSONUnsafe = false;
        private PathConfig pathConfig = new PathConfig();
        private VideoConfigs videoConfigs = new VideoConfigs();
        private bool introGenerated = false;
        public HistoryConverter(GPTHistory _gptHistory, VideoConfigs _videoConfigs, List<AssetClass> _personajes_disponibles, List<AssetClass> _escenas_disponibles, PathConfig _pathConfig,bool _checkJSONUnsafe)
        {
            gptHistory = _gptHistory;
            personajes_disponibles = _personajes_disponibles;
            escenas_disponibles = _escenas_disponibles;
            checkJSONUnsafe = _checkJSONUnsafe;
            pathConfig = _pathConfig;
            videoConfigs = _videoConfigs;
        }

        public NintaiVideo convert_GPT_to_Nintai(string time_name)
        {
            string Titulo = "";
            List<Scene> lista_escenas = new List<Scene>();

            //Solo deberia existir una historia
            Titulo = gptHistory.historia[0].titulo;
            bool titulo_generado = false;
            int escena_orden = 1;
            foreach (Escena escena_gpt in gptHistory.historia[0].escenas)
            {
                //-----------------------------------------------------------------------------------------
                //Buscando si hay que agregar Intro
                //-----------------------------------------------------------------------------------------
                if(videoConfigs.VideoIntroAsset != "" && videoConfigs.VideoIntroAsset != null && !introGenerated)
                {
                    Scene escena_nintai_intro = new Scene();
                    escena_nintai_intro.scene_type = "intro";
                    escena_nintai_intro.audio_asset = "";
                    escena_nintai_intro.name = "intro";
                    escena_nintai_intro.order = escena_orden;
                    escena_nintai_intro.scene_number = escena_orden;
                    escena_nintai_intro.base_visual = videoConfigs.VideoIntroAsset;
                    lista_escenas.Add(escena_nintai_intro);
                    escena_orden += 1;
                    introGenerated = true;
                }
                //-----------------------------------------------------------------------------------------
                string escena_name = escena_gpt.escenario.Trim() != "" ? escena_gpt.escenario.Trim() : escena_gpt.titulo.Trim();
                AssetClass temp = Obtener_personaje_o_escena(escena_name, escenas_disponibles);
                string escenario_escena_gpt = escena_gpt.escenario;
                Scene escena_nintai = new Scene();
                escena_nintai.scene_type = "normal";
                escena_nintai.audio_asset = temp.audio_asset;
                escena_nintai.name = escena_gpt.titulo;
                escena_nintai.order = escena_orden;
                escena_nintai.scene_number = escena_orden;
                escena_nintai.base_visual = temp.File;
                List<VideoSection> secciones_video_lista = new List<VideoSection>();
                int orden_local = 1;
                //-----------------------------------------------------------------------------------------
                //Creamos seccion de titulo - Solo en la primera escena como un video section
                //-----------------------------------------------------------------------------------------
                if (!titulo_generado)
                {
                    VideoSection videoSection_nintai = new VideoSection();
                    Obtener_voice(ref videoSection_nintai, "Narrador");
                    videoSection_nintai.audio_asset = "";
                    videoSection_nintai.dialog = Titulo;
                    videoSection_nintai.autor = "Narrador";
                    videoSection_nintai.order = orden_local;
                    videoSection_nintai.section_type = "TITLE";
                    videoSection_nintai.text = Titulo;
                    orden_local += 1;
                    List<Character> lista_personajes = new List<Character>();
                    videoSection_nintai.characters = lista_personajes;
                    secciones_video_lista.Add(videoSection_nintai);
                    titulo_generado = true;
                }
                //-----------------------------------------------------------------------------------------
                foreach (Dialogo dialogo_gpt in escena_gpt.dialogos)
                {
                    VideoSection videoSection_nintai = new VideoSection();
                    Obtener_voice(ref videoSection_nintai, dialogo_gpt.narrador_o_personaje);
                    videoSection_nintai.audio_asset = "";
                    videoSection_nintai.dialog = dialogo_gpt.dialogo;
                    videoSection_nintai.autor = dialogo_gpt.narrador_o_personaje;
                    videoSection_nintai.order = orden_local;
                    videoSection_nintai.section_type = "DIALOG";
                    videoSection_nintai.text = "";
                    orden_local += 1;
                    List<Character> lista_personajes = new List<Character>();
                    int orden_personaje = 1;
                    //El autor del dialogo tambien puede ser un character a menos que sea el narrador
                    if (dialogo_gpt.narrador_o_personaje != "" && !dialogo_gpt.narrador_o_personaje.Trim().ToLower().Contains("narrador"))
                    {

                        Character personaje = new Character();
                        AssetClass tempChar = Obtener_personaje_o_escena(dialogo_gpt.narrador_o_personaje, personajes_disponibles, true);
                        personaje.character_name = dialogo_gpt.narrador_o_personaje;
                        personaje.order = orden_personaje;
                        personaje.character_type = "CHARACTER";
                        personaje.base_visual = tempChar.File;
                        lista_personajes.Add(personaje);
                        orden_personaje += 1;
                    }

                    foreach (string escuchantes in dialogo_gpt.escuchan ?? Enumerable.Empty<string>())
                    {
                        if (orden_personaje < 3 && escuchantes.Trim() !="")
                        {
                            Character personaje = new Character();
                            AssetClass tempChar = Obtener_personaje_o_escena(escuchantes, personajes_disponibles, true);
                            personaje.character_name = escuchantes;
                            personaje.order = orden_personaje;
                            personaje.character_type = "CHARACTER";
                            personaje.base_visual = tempChar.File;
                            lista_personajes.Add(personaje);
                            orden_personaje += 1;
                        }
                    }
                    videoSection_nintai.characters = lista_personajes;
                    secciones_video_lista.Add(videoSection_nintai);
                }
                escena_nintai.video_sections = secciones_video_lista;
                escena_orden += 1;
                lista_escenas.Add(escena_nintai);
            }

            NintaiVideo NnintaiVideo = new NintaiVideo();
            NnintaiVideo.escenas = lista_escenas;
            NnintaiVideo.story_name = Titulo;
            NnintaiVideo.story_folder = time_name;

            return NnintaiVideo;
        }
        public AssetClass Obtener_personaje_o_escena(string nombre, List<AssetClass> personajes_disponibles, bool isCharacter = false)
        {
            AssetClass retorno = null;
            foreach (AssetClass personaje in personajes_disponibles)
            {
                if (personaje.Name.Limpiar().Contains(nombre.Limpiar()) ||
                    nombre.Limpiar().Contains(personaje.Name.Limpiar()) ||
                    personaje.Description.Limpiar() == nombre.Limpiar()
                    )
                {
                    retorno = personaje;//personaje.File;
                    break;
                }
            }
            if(retorno == null)
            {
                foreach (AssetClass personaje in personajes_disponibles)
                {
                    if (personaje.Description.Limpiar().Contains(nombre.Limpiar()))
                    {
                        retorno = personaje;// personaje.File;
                        break;
                    }
                }
            }
            if (retorno == null)
            {
                MessageBox.Show("No se encontro" + (isCharacter ? " el personaje " : " la escena: ") + nombre + "\n Porfavor selecciona un nuevo asset para cubrirlo");
                ExtraSelector extra_selectror = new ExtraSelector(pathConfig,isCharacter,checkJSONUnsafe, (isCharacter ? personajes_disponibles : escenas_disponibles));
                extra_selectror.ShowDialog();
                if(isCharacter)
                    personajes_disponibles.Add(extra_selectror.assetSelected);
                else
                    escenas_disponibles.Add(extra_selectror.assetSelected);
                retorno = extra_selectror.assetSelected;// extra_selectror.assetSelected.File;
            }
            return retorno;
        }

        public string Obtener_voice(ref VideoSection videoSection, string autor)
        {
            bool found = false;
            foreach (AssetClass personaje in personajes_disponibles)
            {
                if (personaje.Name.Limpiar().Contains(autor.Limpiar()) ||
                    autor.Limpiar().Contains(personaje.Name.Limpiar()) ||
                    personaje.Description.Limpiar() == autor.Limpiar()
                    )
                {
                    videoSection.voice = personaje.voice.ToString();
                    videoSection.pitch = personaje?.pitch?.ToString();
                    videoSection.speed = personaje?.speed.ToString();
                    videoSection.gender = personaje?.genero.ToString();
                    found = true;
                    break;
                }
               
            }
            if(!found && !autor.Limpiar().Contains("narrador"))
            {
                foreach (AssetClass personaje in personajes_disponibles)
                {
                    if (personaje.Description.Limpiar().Contains(autor.Limpiar()))
                    {
                        videoSection.voice = personaje.voice.ToString();
                        videoSection.pitch = personaje?.pitch?.ToString();
                        videoSection.speed = personaje?.speed.ToString();
                        videoSection.gender = personaje?.genero.ToString();
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    MessageBox.Show("No se encontro el audio del personaje " + autor + "\n Porfavor selecciona un nuevo asset para cubrirlo");
                    ExtraSelector extra_selectror = new ExtraSelector(pathConfig,true,checkJSONUnsafe,personajes_disponibles);
                    extra_selectror.ShowDialog();
                    personajes_disponibles.Add(extra_selectror.assetSelected);
                    videoSection.voice = extra_selectror.assetSelected.voice.ToString();
                    videoSection.pitch = extra_selectror.assetSelected?.pitch?.ToString();
                    videoSection.speed = extra_selectror.assetSelected?.speed.ToString();
                    videoSection.gender = extra_selectror.assetSelected?.genero.ToString();
                    videoSection.audio_asset = extra_selectror.assetSelected?.audio_asset.ToString();
                    found = true;
                }

            }
            else if(!found)
            {
                videoSection.voice = GoogleVoices.es_US_Neural2_B_MALE.ToString();
                videoSection.pitch = "0st";
                videoSection.speed = GoogleSpeed.medium.ToString();
                videoSection.gender = SsmlVoiceGender.Male.ToString();
            }
            return "";
        }
    }
}
