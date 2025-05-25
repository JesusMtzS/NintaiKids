using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nintai_kids.Modules
{
    internal static class Helper
    {
        public static string Limpiar(this string input)
        {

            // Convertir el texto a formato de caracteres Unicode
            string unicodeTexto = input.Normalize(NormalizationForm.FormD);

            // Crear un StringBuilder para construir el texto normalizado
            StringBuilder sb = new StringBuilder();

            // Recorrer cada carácter en el texto normalizado
            foreach (char c in unicodeTexto)
            {
                // Si el carácter no tiene una marca diacrítica, agregarlo al StringBuilder
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            string pattern = @"[^\w\s]";
            // Reemplaza las puntuaciones con una cadena vacía
            return Regex.Replace(sb.ToString(), pattern, "").Trim().ToLower();
        }
    }
}
