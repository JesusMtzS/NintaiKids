using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nintai_kids.Models
{
    public class Position
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Character
    {
        public string? character_name { get; set; }
        public string? base_visual { get; set; }
        public int order { get; set; }
        public string? character_type { get; set; }
        public Position? position { get; set; }
    }

    public class VideoSection
    {
        public string? voice { get; set; }
        public string? pitch { get; set; }
        public string? speed { get; set; }
        public string? gender { get; set; }
        public int order { get; set; }
        public string? audio_asset { get; set; }
        public string? dialog { get; set; }
        public string? autor { get; set; }
        public string? text { get; set; }
        public string? section_type { get; set; }
        public List<Character>? characters { get; set; }
    }

    public class SceneElement
    {
        public string? element_name { get; set; }
        public string? base_visual { get; set; }
        public int order { get; set; }
        public string? character_type { get; set; }
        public Position? position { get; set; }
    }

    public class Scene
    {
        public string? name { get; set; }
        public int scene_number { get; set; }
        public int order { get; set; }
        public string? audio_asset { get; set; }
        public string? base_visual { get; set; }
        public List<VideoSection>? video_sections { get; set; }
        public List<SceneElement>? scene_elements { get; set; }
        public string? scene_type { get; set; }
    }

    public class NintaiVideo
    {
        public List<Scene>? escenas { get; set; }
        public string? story_name { get; set; }
        public string? story_folder { get; set; }
    }
}
