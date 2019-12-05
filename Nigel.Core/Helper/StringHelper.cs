namespace Nigel.Core
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 12:47:42
    *                   mailto:3624091@qq.com
    *                         
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Net;
    using System.Globalization;

    public static class StringHelper
    {
        #region LineSeparator Conversions
        /// <summary>
        /// A liberal list of line breaking characters used in unicode. Typically,
        /// LF and CR are the only characters supported in C#.
        /// </summary>
        public static readonly char[] LineBreakingCharacters = new char[]
        {
            '\x000a', // LF \n
            '\x000d', // CR \r
            '\x000c', // FF (form feed)
            '\x2028', // LS (unicode line separator)
            '\x2029', // paragraph separator
            '\x0085', // NEL (next line)
        };

        /// <summary>
        /// DOS/Windows style line breaks (CR+LF)
        /// </summary>
        public static readonly char[] DosLineSeparator = { '\x000d', '\x000a' };
        /// <summary>
        /// Unix style line breaks (LF)
        /// </summary>
        public static readonly char[] UnixLineSeparator = { '\x000a' };
        /// <summary>
        /// Commodore, TRS-80, Apple II, Apple MacOS9 style line breaks (CR)
        /// </summary>
        public static readonly char[] MacOs9Separator = { '\x000d' };
        /// <summary>
        /// Unicode line separator - not widely supported
        /// </summary>
        public static readonly char[] UnicodeSeparator = { '\x2028' };

        /// <summary>
        /// Converts from a liberal list of unicode line separators to the
        /// specified line separator.
        /// </summary>
        /// <param name="reader">TextReader to read from</param>
        /// <param name="writer">TextReader to write to</param>
        /// <param name="separator">Line break separator.</param>
        public static void ConvertLineSeparators(TextReader reader,
            TextWriter writer, char[] separator)
        {
            for (var c = reader.Read(); c != -1; c = reader.Read())
            {
                // One new line
                if (LineBreakingCharacters.Contains((char)c))
                {
                    // If a windows style new line, skip the next char
                    if (c == '\r' && reader.Peek() == '\n')
                    {
                        reader.Read();
                    }

                    writer.Write(separator);
                    continue;
                }
                writer.Write((char)c);
            }
        }

        /// <summary>
        /// Converts from a liberal list of unicode line separators to the
        /// specified line separator.
        /// </summary>
        /// <example>
        /// // Convert line breaks to current environment's default
        /// var text = "blah blah...";
        /// ConvertLineSeparators(text, Environment.NewLine.ToCharArray());
        /// </example>
        /// <param name="text">Source text</param>
        /// <param name="separator">Line break separator.</param>
        /// <returns>String with normalized line separators</returns>
        public static String ConvertLineSeparators(String text, char[] separator)
        {
            var reader = new StringReader(text);
            var writer = new StringWriter();
            ConvertLineSeparators(reader, writer, separator);
            return writer.ToString();
        }

        #endregion

        public static string GetNoceStr(int length)
        {
            string str = "0123456789";
            Random r = new Random();
            string result = string.Empty;

            //生成一个8位长的随机字符，具体长度可以自己更改
            for (int i = 0; i < length; i++)
            {
                int m = r.Next(0, str.Length);
                string s = str.Substring(m, 1);
                result += s;
            }

            return result;
        }

        public static string CreateOrderNumber(string name)
        {
            return string.Format("{0}{1}{2}", name, DateTime.Now.ToString("yyyyMMddHHmmssfff"), GetNoceStr(4));
        }
    }
}
