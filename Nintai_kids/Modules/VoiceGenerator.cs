using Nintai_kids.Models;
using Google.Cloud.TextToSpeech.V1;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Xml;
using System.Text.RegularExpressions;
using Google.Type;

namespace Nintai_kids.Modules
{
    internal class VoiceGenerator
    {
        private PathConfig pathConfig = new PathConfig();
        public VoiceGenerator(PathConfig _pathConfig)
        {
            pathConfig = _pathConfig;
        }

        public NintaiVideo Generate_Voices(NintaiVideo gptHistory, List<AssetClass> personajes_disponibles, List<AssetClass> escenas_disponibles)
        {
            return new NintaiVideo();
        }
        public void Generate_voice(string dialog)
        {

        }

        public void SynthesizeText(string text, string outputFile, string voice_name, SsmlVoiceGender gender,string storeSamplePath = "")
        {
            // Carga las credenciales desde el archivo JSON de la Service Account
            GoogleCredential credential = GoogleCredential.FromFile(pathConfig.JSONTextToSpeechKey);

            // Crea un cliente de Text-to-Speech con las credenciales
            TextToSpeechClientBuilder builder = new TextToSpeechClientBuilder
            {
                ChannelCredentials = credential.ToChannelCredentials()
            };
            TextToSpeechClient client = builder.Build();

            // Configura la solicitud de síntesis de voz
            SynthesisInput input = new SynthesisInput
            {
                Text = text
            };
            VoiceSelectionParams voice = new VoiceSelectionParams
            {
                //"es-MX", //"es-ES", // es-US // Código del idioma
                LanguageCode = "es-US",
                SsmlGender = gender,//SsmlVoiceGender.Neutral, // Género de la voz
                Name = voice_name
                //"es-MX-Standard-A" // Nombre de la voz específica
                //es-US-Studio-B
            };
            AudioConfig audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Linear16 // Formato de audio
            };

            // Solicitud de síntesis de voz
            SynthesizeSpeechRequest request = new SynthesizeSpeechRequest
            {
                Input = new SynthesisInput
                {
                    //Text = text, // Texto que deseas convertir a voz
                    Ssml = text
                    //Ssml example : <speak><prosody rate='slow' pitch='-2st'>Hola, mundo!</prosody></speak>
                },
                Voice = voice,
                AudioConfig = audioConfig
            };


            // Realiza la solicitud de síntesis de voz
            //SynthesizeSpeechResponse response = client.SynthesizeSpeech(input, voice, audioConfig);
            SynthesizeSpeechResponse response = client.SynthesizeSpeech(request);

            // Guarda el resultado en un archivo de salida
            ByteString audioContent = response.AudioContent;
            System.IO.File.WriteAllBytes(outputFile, audioContent.ToByteArray());
            if(storeSamplePath != "")
            {
                if (!File.Exists(storeSamplePath))
                {
                    System.IO.File.WriteAllBytes(storeSamplePath, audioContent.ToByteArray());
                }
            }
        }
        public void Synthesize_Nintai_Video(NintaiVideo nintai_video)
        {
            foreach(Scene scene in nintai_video.escenas ?? Enumerable.Empty<Scene>())
            {
                foreach (VideoSection video_section in scene.video_sections ?? Enumerable.Empty<VideoSection>())
                {
                    video_section.audio_asset = "";
                    if (video_section.autor.Trim().ToLower() != "narrador")
                        video_section.dialog = Limpia_Dialogo(video_section.dialog);

                    video_section.voice = Clean_gender(video_section.voice);

                    string audio_file_name = System.DateTime.Now.ToString("yy_MM_dd_HH_mm_ss_fff") + ".wav";
                    string SsmlDialog = Covert_to_Ssml(video_section.dialog,video_section.pitch,video_section.speed);
                    SsmlVoiceGender gender = (SsmlVoiceGender)Enum.Parse(typeof(SsmlVoiceGender), video_section.gender);
                    string sample_path = Path.Combine(pathConfig.VoicesSamples, video_section.voice, video_section.speed, video_section.pitch + ".wav");
                    SynthesizeText(SsmlDialog, pathConfig.AudioAssets + audio_file_name,video_section.voice, gender, sample_path);
                    video_section.audio_asset = audio_file_name;
                }
             }
        }

        public string Limpia_Dialogo(string dialogo)
        {
            string patron = @"\((.*?)\)";
            // Usar Regex.Replace para eliminar el texto entre paréntesis
            return Regex.Replace(dialogo, patron, "");
        }

        public string Covert_to_Ssml(string dialogo, string pitch,string speed)
        {
            return $"<speak><prosody rate = '{speed}' pitch = '{pitch}' > {dialogo} </prosody></speak>";
        }

        private string Clean_gender(string voice_name)
        {
            if(voice_name != null)
            {
                return voice_name.Replace("_MALE", "").Replace("_FEMALE", "").Replace('_', '-');
            }
            else
            {
                return "";
            }
        }
    }
}
