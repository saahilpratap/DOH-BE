using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Extensions
{
    public static class StringExtensions
    {
        public static string GetUntil(this string str, char stopCharacter)
        {
            var stopCharacterIndex = str.IndexOf(stopCharacter);
            if (stopCharacterIndex == -1)
                return str;
            return str.Substring(0, stopCharacterIndex);
        }

        public static string ReverseChars(this string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string GetCodeEnding(this int id)
        {
            var idAsString = id.ToString();
            if (idAsString.Length == 1)
            {
                return "00" + idAsString;
            }
            else if (idAsString.Length == 2)
            {
                return "0" + idAsString;
            }
            else
                return idAsString;
        }

        public static string GetCodeEnding(this long id)
        {
            var idAsString = id.ToString();
            if (idAsString.Length == 1)
            {
                return "00" + idAsString;
            }
            else if (idAsString.Length == 2)
            {
                return "0" + idAsString;
            }
            else
                return idAsString;
        }
    }
}
