using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrapporNet.Extensions
{
    public static class StringExtensions
    {
        public static string CleanHtml(this string stringToClean)
        {
            return stringToClean.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();
        }
    }
}
