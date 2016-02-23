using System.Collections.Generic;
using System.Text;


namespace Toolbox
{
    public static class StringExtensions
    {
        /// <summary>
        /// Capitalizes the first character of the string
        /// </summary>
        /// <param name="original"></param>
        /// <returns>A copy of the string with the first char in caps</returns>
        public static string Ucfirst(this string original)
        {
            if (original.Length < 1)
            {
                return original;
            }

            char[] elements = original.ToCharArray();
            elements[0] = char.ToUpper(elements[0]);

            return new string(elements);
        }

        /// <summary>
        /// Splits the string along capital characters
        /// </summary>
        /// <param name="original"></param>
        /// <returns>A list of splitted strings</returns>
        public static List<string> CamelCaseSplit(this string original)
        {
            List<string> result = new List<string>();

            if (original.Length < 2)
            {
                result.Add(original);
            }
            else
            {
                char[] elements = original.ToCharArray();
                StringBuilder builder = new StringBuilder();
                builder.Append(elements[0]);

                for (int index = 1; index < elements.Length; ++index)
                {
                    char current = elements[index];

                    if (char.IsUpper(current) && !char.IsUpper(elements[index - 1]))
                    // Last was lowercase and this one is uppercase. We should split here.
                    {
                        result.Add(builder.ToString());
                        builder.Length = 0;
                        builder.Append(current);
                    }
                    else
                    {
                        builder.Append(current);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a string of a number with the specified amount of decimal places
        /// </summary>
        /// <param name="original"></param>
        /// <param name="val"></param>
        /// <param name="decimals"></param>
        /// <returns>A string with the formatted number</returns>
        public static string FormatDecimal(this string original, float val, int decimals)
        {
            if (decimals <= 0)
            {
                return ((int)val) + "";

            }
            else
            {
                string formatString = "#.";
                for (int i = 0; i < decimals; i++)
                {
                    formatString += "#";
                }
                return (val).ToString(formatString);
            }
        }
    }
}