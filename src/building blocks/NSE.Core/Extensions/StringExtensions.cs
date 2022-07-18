using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSE.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ApenasNumeros(this string str)
        {
            return Regex.Replace(str, @"\D", "");
        }
    }
}
