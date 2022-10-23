using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodaHora.Models.Utils
{
    public static class FormatTelefone
    {
        /// <summary>
        /// Formatar uma string de Telefone
        /// </summary>
        /// <param name="Telefone">string CNPJ sem formatacao</param>
        /// <returns>string CNPJ formatada</returns>
        /// <example>Recebe '00000000000' Devolve '(00) 00000-0000'</example>
        public static string FormatTel(string Telefone)
        {
            return Convert.ToUInt64(Telefone).ToString(@"(00) 00000-0000");
        }

        public static string SemFormatacao(string Telefone)
        {
            return Telefone.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);
        }
    }
}