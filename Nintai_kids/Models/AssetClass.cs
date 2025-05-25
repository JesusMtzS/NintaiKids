using Google.Cloud.TextToSpeech.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nintai_kids.Models
{
    /// <summary>
    /// Specify an asset and his properties.
    /// </summary>
    internal class AssetClass
    {
        public string? Name { get; set; }
        public string? File { get; set; }
        public string? Description { get; set; }
        public GoogleVoices? voice { get; set; }
        public string? pitch { get; set; }
        public SsmlVoiceGender? genero { get; set; }
        public GoogleSpeed? speed { get; set; }
        public string? audio_asset { get; set; }
    }
}
