using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for StringTool
/// </summary>
namespace FCore.Foundations
{
    public static class StringTools
    {
        /// <summary>
        /// Will capitalize (if not already) only the first character of the input string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        
        /// <summary>
        /// Uncapitalizes just the first character of the input string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Uncapitalize(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToLower(a[0]);
            return new string(a);
        }

        // Will only capitalize first letter of each word.
        
        /// <summary>
        /// Capitalizes every first letter of every word. That is, the resulting string will always
        /// have a capital character after a space.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CapitalizeAll(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] array = s.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);

        }

        /// <summary>
        /// Will uncapitalize every word, or every character that follows a space.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UncapitalizeAll(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] array = s.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsUpper(array[0]))
                {
                    array[0] = char.ToLower(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsUpper(array[i]))
                    {
                        array[i] = char.ToLower(array[i]);
                    }
                }
            }
            return new string(array);
        }
                
        /// <summary>
        /// First splits the string by the delimiting character, then returns the first item.
        /// </summary>
        /// <param name="delimiter"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetFirst(char delimiter, string text)
        {
            string[] split = text.Split(delimiter);
            return split[0];
        }
        /// <summary>
        /// First splits the string by the delimiting character, then returns the last item.
        /// </summary>
        /// <param name="delimiter"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetLast(char delimiter, string text)
        {
            string[] split = text.Split(delimiter);
            return split[split.Length - 1];
        }

        /// <summary>
        /// Converts an array of any object, and produces a comma-space-separated list into one string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string ToStringList<T>(T[] items)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < items.Length; i++)
            {
                sb.Append(items[i].ToString());

                if (i < items.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts an array of an IList of any object, and produces a comma-space-separated list into one string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string ToStringList<T>(IList<T> items)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < items.Count; i++)
            {
                sb.Append(items[i].ToString());

                if (i < items.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Takes in a string and grabs a list of all whole number occurences. A whole number will be picked
        /// out only between non-numeric "delimiters". For example, for the input string abc123def456, it will return
        /// a list of 123 and 456, not 1, 2, 3, 4, 5, 6.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<int> GrabWholeNumbers(string s)
        {           
            List<int> integers = new List<int>();
            string[] numbers = Regex.Split(s, @"\D+");

            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    integers.Add(int.Parse(value));                    
                }
            }
            return integers;
        }

        /// <summary>
        /// Filename only, without extension! Note that this will transform the incoming string!
        /// </summary>
        /// <param name="f">Should be the file name without extension. If you have a period, it will
        /// be turned into an underscore.</param>
        /// <returns></returns>
        public static string ToValidFilename(string f)
        {
            // replace all the whitespace characters in a row with underscore.
            f = Regex.Replace(f, @"\s+", "_");

            // stage 2, replace all the illegal characters with "_"
            char[] illegalFilenameChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (char c in illegalFilenameChars)
            {
                f = f.Replace(c, '_');
            }

            return f;
        }

        /// <summary>
        /// Takes an array of strings and fuses each item together with a single character.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="glue"></param>
        /// <returns></returns>
        public static string Join(string[] s, char glue)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                sb.Append(s[i]);

                if (i < s.Length - 1)
                {
                    sb.Append(glue);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns an entirely new array that is identical, except without the first item.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string[] ExcludeFirst(string[] s)
        {
            string[] noFirst = new string[s.Length - 1];

            for (int i = 1; i < s.Length; i++)
            {
                noFirst[i - 1] = s[i];
            }

            return noFirst;
        }

        /// <summary>
        /// "Illegal" means can't be part of a filename. If it finds them, it will replace them with an empty string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StripIllegalCharacters(string value)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()) + "&";

            foreach (char c in invalid)
            {
                value = value.Replace(c.ToString(), "");
            }

            return value;
        }

        // Tested 5/21/2013, Working.
        /// <summary>
        /// Returns which of character a and b appears first in the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static char WhichListedFirst(string str, char a, char b)
        {
            if (a.Equals(b))
            {
                throw new Exception("Characters a and b cannot be equal since we require that only one of them be returned.");
            }

            char toReturn = a;
            bool aOrBAreInStr = false;
            foreach (char c in str)
            {
                if ((c.Equals(a)) && (!c.Equals(b)))
                {
                    aOrBAreInStr = true;
                    break;
                }
                else if (c.Equals(b))
                {
                    aOrBAreInStr = true;
                    toReturn = b;
                    break;
                }
            }

            if (aOrBAreInStr == false)
            {
                throw new Exception("Neither a or b show up in the given string to search for either one.");
            }

            return toReturn;
        }

        /// <summary>
        /// String helper function to tell whether any (one or more) of the strings listed in searchTerms
        /// exists in the string in question.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="searchTerms"></param>
        /// <returns></returns>
        public static bool ContainsAny(this string str, IEnumerable<string> searchTerms)
        {
            return searchTerms.Any(searchTerm => str.ToLower().Contains(searchTerm.ToLower()));
        }

        /// <summary>
        /// String helper function to tell whether every item in searchTerms exists in the string in question.
        /// This function will most likely return false because it's a more stringent check.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="searchTerms"></param>
        /// <returns></returns>
        public static bool ContainsAll(this string str, IEnumerable<string> searchTerms)
        {
            return searchTerms.All(searchTerm => str.ToLower().Contains(searchTerm.ToLower()));
        }

        public static string ToCamelCase(this string the_string)
        {
            if (the_string == null || the_string.Length < 2)
                return the_string;

            // Split the string into words.
            string[] words = the_string.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = words[0].Substring(0,1).ToLower() + words[0].Substring(1); //.ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result +=
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            return result;   
        }

        public static string StripPunctuation(this string text)
        {
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public static string ToKebabCase(this string text)
        {
            return Regex.Replace(text.ToLower().StripPunctuation(), @"\s+", "-");
        }
    }
}