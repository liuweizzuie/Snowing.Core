using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.Collections
{
    public static class IListStringExtension
    {
        public static string Concat(this IList<string> list, char separator)
        {
            return list.Concat(separator, char.MinValue);
        }

        public static string Concat(this IList<string> list, char separator, char prefix, char suffix)
        {
            return Concat<char>(list, separator, prefix, suffix);
        }

        public static string Concat(this IList<string> list, char separator, char fix)
        {
            return list.Concat(separator, fix, fix);
        }

        public static string Concat(this IList<string> list, string separator)
        {
            return Concat<string>(list, separator, char.MinValue, char.MinValue);
        }

        private static string Concat<T>(IList<string> list, T separator, char prefix, char suffix)
        {
            StringBuilder sb = new StringBuilder();

            if (prefix != char.MinValue)
            {
                sb.Append(prefix);
            }

            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(list[i]);
                if (i != list.Count - 1)
                {
                    sb.Append(separator);
                }
            }

            if (suffix != char.MinValue)
            {
                sb.Append(suffix);
            }

            return sb.ToString();
        }
    }
}
