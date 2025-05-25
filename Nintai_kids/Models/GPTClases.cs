using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nintai_kids.Models
{
    internal class Escena
    {
        public string? titulo { get; set; }
        public string? escenario { get; set; }
        public List<Dialogo>? dialogos { get; set; }
    }

    internal class Dialogo
    {
        public string? narrador_o_personaje { get; set; }
        public string? dialogo { get; set; }
        public List<string>? escuchan { get; set; }
    }

    internal class Historia
    {
        public string? titulo { get; set; }
        public List<Escena>? escenas { get; set; }
    }

    internal class GPTHistory
    {
        public List<Historia>? historia { get; set; }
    }

}
