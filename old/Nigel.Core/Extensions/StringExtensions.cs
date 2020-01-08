namespace Nigel.Core
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/30 17:43:23
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public static class StringExtensions
    {
        #region Appending
        /// <summary>
        /// 将一个字符串乘N次
        /// </summary>
        /// <param name="str"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static string Times(this string str, int times)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            if (times <= 1) return str;

            string strfinal = string.Empty;
            for (int ndx = 0; ndx < times; ndx++)
                strfinal += str;

            return strfinal;
        }


        /// <summary>
        /// 将字符串增加到指定的最大长度。
        /// 如果字符串已经大于maxlength，那么如果truncate为true，那么它将被截断。
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="maxLength">Length of the max.</param>
        /// <param name="truncate">if set to <c>true</c> [truncate].</param>
        /// <returns></returns>
        public static string IncreaseTo(this string str, int maxLength, bool truncate)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (str.Length == maxLength) return str;
            if (str.Length > maxLength && truncate) return str.Truncate(maxLength);

            string original = str;

            while (str.Length < maxLength)
            {
                // Still less after appending by original string.
                if (str.Length + original.Length < maxLength)
                {
                    str += original;
                }
                else // Append partial.
                {
                    str += str.Substring(0, maxLength - str.Length);
                }
            }
            return str;
        }


        /// <summary>
        /// 将字符串增加到指定的最大长度。
        /// 如果字符串已经大于maxlength，那么如果truncate为true，那么它将被截断。
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="minLength"></param>
        /// <param name="maxLength">Length of the max.</param>
        /// <param name="truncate">if set to <c>true</c> [truncate].</param>
        /// <returns></returns>
        public static string IncreaseRandomly(this string str, int minLength, int maxLength, bool truncate)
        {
            Random random = new Random(minLength);
            int randomMaxLength = random.Next(minLength, maxLength);
            return IncreaseTo(str, randomMaxLength, truncate);
        }
        #endregion


        #region Conversion
        /// <summary>
        /// Convert the text  to bytes.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static byte[] ToUTF8Bytes(this string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return new byte[] { };

            return Encoding.UTF8.GetBytes(txt);
        }


        /// <summary>
        /// Converts "yes/no/true/false/0/1"
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static object ToBoolObject(this string txt)
        {
            return ToBool(txt) as object;
        }


        /// <summary>
        /// Converts "yes/no/true/false/0/1"
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool ToBool(this string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return false;

            string trimed = txt.Trim().ToLower();
            if (trimed == "yes" || trimed == "true" || trimed == "1")
                return true;

            return false;
        }


        /// <summary>
        /// Converts "($100、￥100) or 100"
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static object ToIntObject(this string txt)
        {
            return ToInt(txt) as object;
        }


        /// <summary>
        /// Converts "($100、￥100) or 100"
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static int ToInt(this string txt)
        {
            return ToNumber<int>(txt, (s) => Convert.ToInt32(Convert.ToDouble(s)), 0);
        }


        /// <summary>
        /// Converts "($100、￥100) or ($100.5、￥100.5) or 100 or 100.5" to 100.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static object ToLongObject(this string txt)
        {
            return ToLong(txt) as object;
        }


        /// <summary>
        /// Converts "($100、￥100) or ($100.50、￥100.50) or 100 or 100.5" to 100.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static double ToLong(this string txt)
        {
            return ToNumber<long>(txt, (s) => Convert.ToInt64(s), 0);
        }


        /// <summary>
        /// Converts "($100、￥100) or ($100.50、￥100.50) or 100 or 100.5" to 100.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static object ToDoubleObject(this string txt)
        {
            return ToDouble(txt) as object;
        }


        /// <summary>
        /// Converts "($100、￥100) or ($100.50、￥100.50)  or 100 or 100.5" to 100.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static double ToDouble(this string txt)
        {
            return ToNumber<double>(txt, (s) => Convert.ToDouble(s), 0);
        }


        /// <summary>
        /// Converts "($100、￥100) or ($100.50、￥100.50)  or 100 or 100.5" to 100.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static object ToFloatObject(this string txt)
        {
            return ToFloat(txt) as object;
        }


        /// <summary>
        /// Converts "($100、￥100) or ($100.50、￥100.50) or 100 or 100.5" to 100.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static double ToFloat(this string txt)
        {
            return ToNumber<float>(txt, (s) => Convert.ToSingle(s), 0);
        }


        /// <summary>
        /// Converts to a number using the callback.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="txt"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static T ToNumber<T>(string txt, Func<string, T> callback, T defaultValue)
        {
            if (string.IsNullOrEmpty(txt))
                return defaultValue;

            string trimed = txt.Trim().ToLower();
            if (trimed.StartsWith("$") || trimed.StartsWith("￥"))
            {
                trimed = trimed.Substring(1);
            }
            return callback(trimed);
        }


        /// <summary>
        /// Converts "$100 or $100.50 or 100 or 100.5" to 100. Does not round up.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static object ToTimeObject(this string txt)
        {
            return ToTime(txt) as object;
        }


        /// <summary>
        /// Converts "$100 or $100.50 or 100 or 100.5" to 100. Does not round up.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static TimeSpan ToTime(this string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return TimeSpan.MinValue;

            string trimmed = txt.Trim().ToLower();
            return TimeHelper.Parse(trimmed).Item;
        }


        /// <summary>
        /// Converts "$100 or $100.50 or 100 or 100.5" to 100. Does not round up.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static object ToDateTimeObject(this string txt)
        {
            return ToDateTime(txt) as object;
        }


        /// <summary>
        /// Converts "$100 or $100.50 or 100 or 100.5" to 100. Does not round up.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return DateTime.MinValue;

            string trimmed = txt.Trim().ToLower();
            if (trimmed.StartsWith("$"))
            {
                if (trimmed == "${today}") return DateTime.Today;
                if (trimmed == "${yesterday}") return DateTime.Today.AddDays(-1);
                if (trimmed == "${tommorrow}") return DateTime.Today.AddDays(1);
                if (trimmed == "${t}") return DateTime.Today;
                if (trimmed == "${t-1}") return DateTime.Today.AddDays(-1);
                if (trimmed == "${t+1}") return DateTime.Today.AddDays(1);
                if (trimmed == "${today+1}") return DateTime.Today.AddDays(1);
                if (trimmed == "${today-1}") return DateTime.Today.AddDays(-1);

                // Handles ${t+4} or ${t-9}
                string internalVal = trimmed.Substring(2, (trimmed.Length - 1) - 2);
                DateTime result = DateParser.ParseTPlusMinusX(internalVal);
                return result;
            }
            DateTime parsed = DateTime.Parse(trimmed);
            return parsed;
        }
        #endregion


        #region Lists
        /// <summary>
        /// Prefixes all items in the list w/ the prefix value.
        /// </summary>
        /// <typeparam name="string">The type of the tring.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="prefix">The prefix.</param>
        public static List<string> PreFixWith(this List<string> items, string prefix)
        {
            for (int ndx = 0; ndx < items.Count; ndx++)
            {
                items[ndx] = prefix + items[ndx];
            }
            return items;
        }
        #endregion


        #region Matching
        /// <summary>
        /// Determines whether or not the string value supplied represents a "not applicable" string value by matching on na, n.a., n/a etc.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="useNullOrEmptyStringAsNotApplicable"></param>
        /// <returns></returns>
        public static bool IsNotApplicableValue(this string val, bool useNullOrEmptyStringAsNotApplicable = false)
        {
            bool isEmpty = string.IsNullOrEmpty(val);
            if (isEmpty && useNullOrEmptyStringAsNotApplicable) return true;
            if (isEmpty && !useNullOrEmptyStringAsNotApplicable) return false;
            val = val.Trim().ToLower();

            if (val == "na" || val == "n.a." || val == "n/a" || val == "n\\a" || val == "n.a" || val == "not applicable")
                return true;
            return false;
        }


        /// <summary>
        /// Use the Levenshtein algorithm to determine the similarity between
        /// two strings. The higher the number, the more different the two
        /// strings are.
        /// TODO: This method needs to be rewritten to handle very large strings
        /// </summary>
        /// <param name="source">Source string to compare</param>
        /// <param name="comparison">Comparison string</param>
        /// <returns>0 if both strings are identical, otherwise a number indicating the level of difference</returns>
        /// <see cref="http://www.merriampark.com/ld.htm"/>
        /// <see cref="http://en.wikipedia.org/wiki/Levenshtein_distance"/>
        public static int Levenshtein(this string source, string comparison)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "Can't parse null string");
            }
            if (comparison == null)
            {
                throw new ArgumentNullException("comparison", "Can't parse null string");
            }

            var s = source.ToCharArray();
            var t = comparison.ToCharArray();
            var n = source.Length;
            var m = comparison.Length;
            var d = new int[n + 1, m + 1];

            // shortcut calculation for zero-length strings
            if (n == 0) { return m; }
            if (m == 0) { return n; }

            for (var i = 0; i <= n; d[i, 0] = i++) { }
            for (var j = 0; j <= m; d[0, j] = j++) { }

            for (var i = 1; i <= n; i++)
            {
                for (var j = 1; j <= m; j++)
                {
                    var cost = t[j - 1].Equals(s[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(Math.Min(
                        d[i - 1, j] + 1,
                        d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }


        /// <summary>
        /// Calculate the simplified soundex value for the specified string.
        /// </summary>
        /// <see cref="http://en.wikipedia.org/wiki/Soundex"/>
        /// <see cref="http://west-penwith.org.uk/misc/soundex.htm"/>
        /// <param name="source">String to calculate</param>
        /// <returns>Soundex value of string</returns>
        public static string SimplifiedSoundex(this string source)
        {
            return source.SimplifiedSoundex(4);
        }

        /// <summary>
        /// Calculate the simplified soundex value for the specified string.
        /// </summary>
        /// <see cref="http://en.wikipedia.org/wiki/Soundex"/>
        /// <see cref="http://west-penwith.org.uk/misc/soundex.htm"/>
        /// <param name="source">String to calculate</param>
        /// <param name="length">Length of soundex value (typically 4)</param>
        /// <returns>Soundex value of string</returns>
        public static string SimplifiedSoundex(this string source, int length)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (source.Length < 3)
            {
                throw new ArgumentException(
                    "Source string must be at least two characters", "source");
            }

            var t = source.ToUpper().ToCharArray();
            var buffer = new StringBuilder();

            short prev = -1;

            foreach (var c in t)
            {
                short curr = 0;
                switch (c)
                {
                    case 'A':
                    case 'E':
                    case 'I':
                    case 'O':
                    case 'U':
                    case 'H':
                    case 'W':
                    case 'Y':
                        curr = 0;
                        break;
                    case 'B':
                    case 'F':
                    case 'P':
                    case 'V':
                        curr = 1;
                        break;
                    case 'C':
                    case 'G':
                    case 'J':
                    case 'K':
                    case 'Q':
                    case 'S':
                    case 'X':
                    case 'Z':
                        curr = 2;
                        break;
                    case 'D':
                    case 'T':
                        curr = 3;
                        break;
                    case 'L':
                        curr = 4;
                        break;
                    case 'M':
                    case 'N':
                        curr = 5;
                        break;
                    case 'R':
                        curr = 6;
                        break;
                    default:
                        throw new ApplicationException(
                            "Invalid state in switch statement");
                }

                /* Change all consecutive duplicate digits to a single digit
                 * by not processing duplicate values. 
                 * Ignore vowels (i.e. zeros). */
                if (curr != prev)
                {
                    buffer.Append(curr);
                }

                prev = curr;
            }

            // Prefix value with first character
            buffer.Remove(0, 1).Insert(0, t.First());

            // Remove all vowels (i.e. zeros) from value
            buffer.Replace("0", "");

            // Pad soundex value with zeros until output string equals length))))
            while (buffer.Length < length) { buffer.Append('0'); }

            // Truncate values that are longer than the supplied length
            return buffer.ToString().Substring(0, length);
        }

        #endregion


        #region [ Extensions ]


        #region 字符串转换
        /// <summary>
        /// 字符串转换，null返回空字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NullToEmpty(this string value)
        {
            return value != null ? value : string.Empty;
        }
        /// <summary>
        /// 字符串转换，null返回空字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NullToEmpty(this object value)
        {
            return value != null ? value.ToString() : string.Empty;
        }

        #endregion

        #region 身份证号码格式验证
        /// <summary>
        /// 验证字符串是否严格是一个身份证号码
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsStrictIDNumber(this string src)
        {
            if (src.Length == 15)
            {
                return checkIDNumber15(src);
            }
            else if (src.Length == 18)
            {
                return checkIDNumber18(src);
            }
            return false;
        }
        //验证15位身份证号码
        private static bool checkIDNumber15(string src)
        {
            long n = 0;
            if (long.TryParse(src, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(src.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = src.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }
        //验证18位身份证号码
        private static bool checkIDNumber18(string src)
        {
            long n = 0;
            if (long.TryParse(src.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(src.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(src.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = src.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = src.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != src.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }
        #endregion


        /// <summary>
        /// 将源字符串中包含的数字转换为手机拨号链接
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertToDialLink(this string source)
        {
            Regex reg = new Regex(@"(0{1}[1-9]{1}[0-9]{1,2}\-[1-9]{1}[0-9]{6,7}|\(0{1}[1-9]{1}[0-9]{1,2}\)[1-9]{1}[0-9]{6,7}|0{1}[1-9]{1}[0-9]{1,2}[1-9]{1}[0-9]{6,7}|400[0123456789-]+|1[3584]{1}[0-9]{9})(?=\D|$)?");
            MatchCollection collect = reg.Matches(source);
            if (collect != null)
            {
                foreach (Match m in collect)
                {
                    string number = m.ToString();
                    string link = string.Format("<a href=\"tel:{0}\">{1}</a>", Regex.Replace(number, @"\D", ""), number);
                    source = source.Replace(number, link);
                }
            }
            return source;
        }

        /// <summary>
        /// 将字符串按车牌号码的要求格式化
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string FormatAsLicensePlate(this string src)
        {
            if (string.IsNullOrWhiteSpace(src) || string.IsNullOrEmpty(src))
                return src;
            src = src.ClearWriteSpace().ToUpper();
            if (src.Length > 2)
                src = src.Insert(2, " ");
            return src;
        }

        /// <summary>
        /// 验证字符串是否严格是一个车牌号码
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsStrictLicensePlate(this string src)
        {
            return Regex.IsMatch(src, @"^[\u4E00-\u9FA5]{1}[A-Z]{1} ?[a-zA-Z0-9]{5}$");
        }
        /// <summary>
        /// 验证字符串是否严格是银行帐号格式
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsBankCard(this string src)
        {
            if (string.IsNullOrWhiteSpace(src)) return false;
            //清除空格
            src = Regex.Replace(src, @" ", "");
            //去掉检验位
            string code = src.Substring(0, src.Length - 1);
            if (!Regex.IsMatch(code, @"^\d+$")) return false;
            char[] chs = code.ToCharArray();
            int luhmSum = 0;
            for (int i = chs.Length - 1, j = 0; i >= 0; i--, j++)
            {
                int k = chs[i] - '0';
                if (j % 2 == 0)
                {
                    k *= 2;
                    k = k / 10 + k % 10;
                }
                luhmSum += k;
            }
            char r = (luhmSum % 10 == 0) ? '0' : (char)((10 - luhmSum % 10) + '0');
            return r.ToString() == src.Substring(src.Length - 1, 1);
        }

        /// <summary>
        /// 去掉字符串中间与两端的全部全角和半角空格
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string ClearWriteSpace(this string src)
        {
            if (string.IsNullOrWhiteSpace(src))
                return src;
            return Regex.Replace(src, @"( |　)+", "");
        }
        /// <summary>
        /// 读取字符串 转换成集合
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IList<string> ReadLines(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return new List<string>();

            StringReader reader = new StringReader(text);
            string currentLine = reader.ReadLine();
            IList<string> lines = new List<string>();

            while (currentLine != null)
            {
                lines.Add(currentLine);
                currentLine = reader.ReadLine();
            }

            reader.Close();
            reader.Dispose();

            return lines;
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public static string Truncate(this string txt, int maxChars)
        {
            if (string.IsNullOrEmpty(txt))
                return txt;

            if (txt.Length <= maxChars)
                return txt;

            return txt.Substring(0, maxChars);
        }

        /// <summary>
        /// 截取字符串 并追加指定字符串
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="maxChars"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string TruncateWithText(this string txt, int maxChars, string suffix)
        {
            if (string.IsNullOrEmpty(txt))
                return txt;

            if (txt.Length <= maxChars)
                return txt;

            string partial = txt.Substring(0, maxChars);
            return partial + suffix;
        }

        /// <summary>
        /// 过滤HTML 截取字符串 并追加...
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public static string TruncateWithText(this string txt, int maxChars)
        {
            if (String.IsNullOrEmpty(txt))
                return txt;

            txt = Regex.Replace(txt, "<[^>]+>", "");
            txt = txt.Trim().Replace("\r\n", "").Replace("\t", "").Replace(" ", "");

            return TruncateWithText(txt, maxChars, "...");
        }

        /// <summary>
        /// 字典扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static T Get<T>(this IDictionary<string, string> items, string Key)
        {
            if (items == null)
                return default(T);
            if (!items.ContainsKey(Key))
                return default(T);
            return items[Key].Convert<T>();
        }

        /// <summary>
        /// 字典转换字符串
        /// </summary>
        /// <param name="items">字典</param>
        /// <param name="delimeter">分隔符</param>
        /// <param name="appender">func</param>
        /// <returns></returns>
        public static string Join(this IDictionary<string, string> items, char delimeter,
         Func<KeyValuePair<string, string>, string> appender)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            StringBuilder buffer = new StringBuilder();

            foreach (var item in items)
            {
                string val = appender == null ? item.ToString() : appender(item);
                buffer.Append(delimeter + val);
            }
            if (buffer.Length > 0)
                buffer = buffer.Remove(0, 1);
            return buffer.ToString();
        }

        /// <summary>
        /// join 字符串
        /// </summary>
        /// <param name="items"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static string Join(this IList<string> items, string delimeter)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            string joined = "";
            int ndx;
            for (ndx = 0; ndx < items.Count - 1; ndx++)
            {
                joined += items[ndx] + delimeter;
            }
            joined += items[ndx];
            return joined;
        }

        /// <summary>
        /// Convert the word(s) in the sentence to sentence case.
        /// UPPER = Upper
        /// lower = Lower
        /// MiXEd = Mixed
        /// </summary>
        /// <param name="s"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string ConvertToSentanceCase(this string s, char delimiter)
        {
            // Check null/empty
            if (string.IsNullOrEmpty(s))
                return s;

            s = s.Trim();
            if (string.IsNullOrEmpty(s))
                return s;

            // Only 1 token
            if (s.IndexOf(delimiter) < 0)
            {
                s = s.ToLower();
                s = s[0].ToString().ToUpper() + s.Substring(1);
                return s;
            }

            // More than 1 token.
            string[] tokens = s.Split(delimiter);
            StringBuilder buffer = new StringBuilder();

            foreach (string token in tokens)
            {
                string currentToken = token.ToLower();
                currentToken = currentToken[0].ToString().ToUpper() + currentToken.Substring(1);
                buffer.Append(currentToken + delimiter);
            }

            s = buffer.ToString();
            return s.TrimEnd(delimiter);
        }


        /// <summary>
        /// Get the index of a spacer ( space" " or newline )
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="currentPosition"></param>
        /// <returns></returns>
        public static int GetIndexOfSpacer(this string txt, int currentPosition, ref bool isNewLine)
        {
            // Take the first spacer that you find. it could be eithr
            // space or newline, if space is before the newline take space
            // otherwise newline.            
            int ndxSpace = txt.IndexOf(" ", currentPosition);
            int ndxNewLine = txt.IndexOf(Environment.NewLine, currentPosition);
            bool hasSpace = ndxSpace > -1;
            bool hasNewLine = ndxNewLine > -1;
            isNewLine = false;

            // Found both space and newline.
            if (hasSpace && hasNewLine)
            {
                if (ndxSpace < ndxNewLine) { return ndxSpace; }
                isNewLine = true;
                return ndxNewLine;
            }

            // Found space only.
            if (hasSpace && !hasNewLine) { return ndxSpace; }

            // Found newline only.
            if (!hasSpace && hasNewLine) { isNewLine = true; return ndxNewLine; }

            // no space or newline.
            return -1;
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string ConvertToString(this object[] args)
        {
            if (args == null || args.Length == 0)
                return string.Empty;

            StringBuilder buffer = new StringBuilder();
            foreach (object arg in args)
            {
                if (arg != null)
                    buffer.Append(arg.ToString());
            }
            return buffer.ToString();
        }
        /// <summary>
        /// 字符串转换为int数组，默认分隔符(,)
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this string delimitedText)
        {
            return ToIntArray(delimitedText, new string[] { "," });
        }

        /// <summary>
        /// 字符串转换为int数组
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <param name="delimeter">分隔符</param>
        /// <returns></returns>
        public static int[] ToIntArray(this string delimitedText, params string[] delimeter)
        {
            return Array.ConvertAll(ToStringArray(delimitedText, delimeter), c => c.Convert<int>());
        }

        /// <summary>
        /// 字符串转换为long数组，默认分隔符(,)
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <returns></returns>
        public static long[] ToLongArray(this string delimitedText)
        {
            return ToLongArray(delimitedText, new string[] { "," });
        }

        /// <summary>
        /// 字符串转换为long数组
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <param name="delimeter">分隔符</param>
        /// <returns></returns>
        public static long[] ToLongArray(this string delimitedText, params string[] delimeter)
        {
            return Array.ConvertAll(ToStringArray(delimitedText, delimeter), c => c.Convert<long>());
        }
        /// <summary>
        /// 字符串转换为double数组，默认分隔符(,)
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <returns></returns>
        public static double[] ToDoubleArray(this string delimitedText)
        {
            return ToDoubleArray(delimitedText, new string[] { "," });
        }
        /// <summary>
        /// 字符串转换为double数组
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <param name="delimeter">分隔符</param>
        /// <returns></returns>
        public static double[] ToDoubleArray(this string delimitedText, params string[] delimeter)
        {
            return Array.ConvertAll(ToStringArray(delimitedText, delimeter), c => c.Convert<double>());
        }
        /// <summary>
        /// 字符串转换为short数组，默认分隔符(,)
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <returns></returns>
        public static short[] ToShortArray(this string delimitedText)
        {
            return ToShortArray(delimitedText, new string[] { "," });
        }
        /// <summary>
        /// 字符串转换为short数组
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <param name="delimeter">分隔符</param>
        /// <returns></returns>
        public static short[] ToShortArray(this string delimitedText, params string[] delimeter)
        {
            return Array.ConvertAll(ToStringArray(delimitedText, delimeter), c => c.Convert<short>());
        }

        /// <summary>
        /// 字符拆分 不包含空数据项
        /// </summary>
        /// <param name="delimitedText">"1,2,3,4,5,6"</param>
        /// <param name="delimeter">","</param>
        /// <returns></returns>
        public static string[] ToStringArray(this string delimitedText, params string[] delimeter)
        {
            if (string.IsNullOrEmpty(delimitedText))
                return new string[] { };

            string[] tokens = delimitedText.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
            return tokens;
        }


        /// <summary>
        /// 字符串分 包含空数据项
        /// </summary>
        /// <param name="delimitedText">"1,2,3,4,5,6"</param>
        /// <param name="delimeter">','</param>
        /// <returns></returns>
        public static string[] ToStringArray(this string delimitedText, char delimeter)
        {
            if (string.IsNullOrEmpty(delimitedText))
                return null;

            string[] tokens = delimitedText.Split(delimeter);
            return tokens;
        }

        /// <summary>
        /// 截取固定字符
        /// </summary>
        /// <param name="rawText">search-classes-workshops-4-1-1-6</param>
        /// <param name="excludeText">search-classes-workshops</param>
        /// <param name="delimiter">-</param>
        /// <returns>"4","1","1","6"</returns>
        public static string[] Split(this string rawText, string excludeText, char delimiter)
        {
            int indexOfDelimitedData = rawText.IndexOf(excludeText);
            string delimitedData = rawText.Substring(indexOfDelimitedData + excludeText.Length);

            string[] separatedChars = delimitedData.Split(new string[] { delimiter.ToString() }, StringSplitOptions.RemoveEmptyEntries);
            return separatedChars;
        }

        /// <summary>
        /// 替换占位符${name}的其中name，用键关联的值将其取代。
        /// </summary>
        /// <param name="subsitutions">占位符集合</param>
        /// <param name="contentPlaceholders">拥有占位符的文本内容</param>
        /// <returns></returns>
        public static string Substitute(this IDictionary<string, string> subsitutions, string contentPlaceholders)
        {
            if (string.IsNullOrEmpty(contentPlaceholders))
                return contentPlaceholders;

            if (subsitutions == null || subsitutions.Count == 0)
                return contentPlaceholders;

            string replacedValues = contentPlaceholders;
            subsitutions.ForEach<KeyValuePair<string, string>>(kv => replacedValues = replacedValues.Replace("${" + kv.Key + "}", kv.Value));

            return replacedValues;
        }

        /// <summary>
        /// txt..GetDelimitedChars("", '/');
        /// </summary>
        /// <param name="rawText"></param>
        /// <param name="excludeText"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string[] GetDelimitedChars(this string rawText, string excludeText, char delimiter)
        {
            int index = rawText.IndexOf(excludeText);
            return rawText.Substring(index + excludeText.Length).Split(new string[] { delimiter.ToString() }, StringSplitOptions.RemoveEmptyEntries);
        }


        /// <summary>
        /// 字符串转字典
        /// <example>Split id=1,name=zhangsan to "id":"1","name":"zhangsan"</example>
        /// </summary>
        /// <param name="delimitedText">文本</param>
        /// <param name="keyValuePairDelimiter">KeyValePair分隔符<see cref=","/></param>
        /// <param name="keyValueDelimeter">KeyValue分隔符<see cref="="/></param>
        /// <param name="makeKeysCaseSensitive">是否转换为小写副本</param>
        /// <param name="trimValues">是否去除空格</param>
        /// <returns></returns>
        public static IDictionary<string, string> ToMap(this string delimitedText, char keyValuePairDelimiter,
            char keyValueDelimeter, bool makeKeysCaseSensitive, bool trimValues)
        {
            IDictionary<string, string> map = new Dictionary<string, string>();
            string[] tokens = delimitedText.Split(keyValuePairDelimiter);

            if (tokens == null) return map;

            foreach (string token in tokens)
            {
                if (string.IsNullOrEmpty(token))
                    continue;
                // Split city=Queens to "city", "queens"
                string[] pair = token.Split(keyValueDelimeter);

                if (pair.Length < 2)
                    continue;

                string key = pair[0];
                string value = pair[1];

                if (makeKeysCaseSensitive)
                {
                    key = key.ToLower();
                    value = value.ToLower();
                }
                if (trimValues)
                {
                    key = key.Trim();
                    value = value.Trim();
                }
                map[key] = value;
            }
            return map;
        }
        /// <summary>
        /// HTML过滤成普通文本
        /// </summary>
        /// <param name="strHtml">HTML文本</param>
        /// <returns></returns>
        public static string HtmlFilter(this string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml))
                return strHtml;
            string tmpHtml = strHtml.Trim();
            tmpHtml = tmpHtml.Replace("&ldquo;", "“");
            tmpHtml = tmpHtml.Replace("&rdquo;", "”");
            tmpHtml = Regex.Replace(tmpHtml, @"<!\[CDATA\[(.*)\]\]>", "$1");
            tmpHtml = Regex.Replace(tmpHtml, @"<.+?>", "");
            tmpHtml = Regex.Replace(tmpHtml, @"<!--/*[^<>]*-->", "");
            tmpHtml = Regex.Replace(tmpHtml, @"(?:&nbsp;?)+", " ");
            tmpHtml = Regex.Replace(tmpHtml, @"&\w+?;", "");
            tmpHtml = Regex.Replace(tmpHtml, @"\s+", " ");
            return tmpHtml;
        }

        /// <summary>
        /// 将纯文本编码为HTML文本 编码
        /// </summary>
        /// <param name="strSourceText"></param>
        /// <returns></returns>
        public static string TextHTMLEncode(this string strSourceText)
        {
            if (string.IsNullOrEmpty(strSourceText))
                return strSourceText;
            string tmpReturnHTML = strSourceText;
            tmpReturnHTML = tmpReturnHTML.Replace("&", "&#38");
            tmpReturnHTML = tmpReturnHTML.Replace("'", "&#39");
            tmpReturnHTML = tmpReturnHTML.Replace("\"", "&#34");
            tmpReturnHTML = tmpReturnHTML.Replace("<", "&#60");
            tmpReturnHTML = tmpReturnHTML.Replace(">", "&#62");
            tmpReturnHTML = tmpReturnHTML.Replace(" ", "&nbsp;");
            tmpReturnHTML = tmpReturnHTML.Replace("\n", "<br/>");
            tmpReturnHTML = tmpReturnHTML.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
            return tmpReturnHTML;
        }
        /// <summary>
        /// 将HTML文本编码为纯文本 解码
        /// </summary>
        /// <param name="strSourceText"></param>
        /// <returns></returns>
        public static string TextHTMLDecode(this string strSourceText)
        {
            if (string.IsNullOrEmpty(strSourceText))
                return strSourceText;
            string tmpReturnHTML = strSourceText;
            tmpReturnHTML = tmpReturnHTML.Replace("&#38", "&");
            tmpReturnHTML = tmpReturnHTML.Replace("&#39", "'");
            tmpReturnHTML = tmpReturnHTML.Replace("&#34", "\"");
            tmpReturnHTML = tmpReturnHTML.Replace("&#60", "<");
            tmpReturnHTML = tmpReturnHTML.Replace("&#62", ">");
            tmpReturnHTML = tmpReturnHTML.Replace("&nbsp;", " ");
            tmpReturnHTML = tmpReturnHTML.Replace("<br/>", "\n");
            tmpReturnHTML = tmpReturnHTML.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "\t");
            return tmpReturnHTML;
        }
        /// <summary>
        /// 过滤脚本、HTML、转义符
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string Filtrate(this string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring))
                return Htmlstring;
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring = Htmlstring.HtmlEncode();

            Htmlstring = Htmlstring.Replace("\"", "\\\"");
            Htmlstring = Htmlstring.Replace("\\", "\\\\");
            Htmlstring = Htmlstring.Replace("/", "\\/");
            Htmlstring = Htmlstring.Replace("\b", "\\b");
            Htmlstring = Htmlstring.Replace("\f", "\\f");
            Htmlstring = Htmlstring.Replace("\n", "\\n");
            Htmlstring = Htmlstring.Replace("\r", "\\r");
            Htmlstring = Htmlstring.Replace("\t", "\\t");
            return Htmlstring;
        }
        /// <summary>
        /// 字符串编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string str)
        {
            return HttpUtility.HtmlEncode(str);
        }
        /// <summary>
        /// 字符串解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string str)
        {
            return HttpUtility.HtmlDecode(str);
        }
        /// <summary>
        /// 过滤 Sql 语句字符串中的注入脚本
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string SqlFilter(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            //单引号替换成两个单引号
            str = str.Replace("'", "''");
            //半角封号替换为全角封号，防止多语句执行
            str = str.Replace(";", "；");
            //半角括号替换为全角括号
            str = str.Replace("(", "（");
            str = str.Replace(")", "）");
            ///////////////要用正则表达式替换，防止字母大小写得情况////////////////////
            //去除执行存储过程的命令关键字
            str = str.Replace("Exec", "");
            str = str.Replace("Execute", "");
            //去除系统存储过程或扩展存储过程关键字
            str = str.Replace("xp_", "x p_");
            str = str.Replace("sp_", "s p_");
            //防止16进制注入
            str = str.Replace("0x", "0 x");
            return str;
        }
        /// <summary>
        /// 获取简体中文字符串拼音首字母
        /// </summary>
        /// <param name="input">简体中文字符串</param>
        /// <returns>拼音首字母</returns>
        public static string GetSpells(this string input)
        {
            int len = input.Length;
            string reVal = "";
            for (int i = 0; i < len; i++)
            {
                reVal += GetSpell(input.Substring(i, 1));
            }
            return reVal;
        }
        /// <summary>
        /// 获取一个简体中文字的拼音首字母
        /// </summary>
        /// <param name="cn">一个简体中文字</param>
        /// <returns>拼音首字母</returns>
        public static string GetSpell(this string cn)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cn);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "?";
            }
            else return cn;
        }

        /// <summary>
        /// 汉字转换为Unicode编码
        /// </summary>
        /// <param name="str">要编码的汉字字符串</param>
        /// <returns>Unicode编码的的字符串</returns>
        public static string ToUnicode(this string str)
        {
            byte[] bts = Encoding.Unicode.GetBytes(str);
            string r = "";
            for (int i = 0; i < bts.Length; i += 2) r += "\\u" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
            return r;
        }
        /// <summary>
        /// 将Unicode编码转换为汉字字符串
        /// </summary>
        /// <param name="str">Unicode编码字符串</param>
        /// <returns>汉字字符串</returns>
        public static string ToGB2312(this string str)
        {
            string r = "";
            MatchCollection mc = Regex.Matches(str, @"\\u([\w]{2})([\w]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            byte[] bts = new byte[2];
            foreach (Match m in mc)
            {
                bts[0] = (byte)int.Parse(m.Groups[2].Value, NumberStyles.HexNumber);
                bts[1] = (byte)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber);
                r += Encoding.Unicode.GetString(bts);
            }
            return r;
        }

        #endregion

        public static List<string> GetImgUrl(this string html)
        {
            return GetImgUrl(html, @"<IMG[^>]+src=\s*(?:'(?<src>[^']+)'|""(?<src>[^""]+)""|(?<src>[^>\s]+))\s*[^>]*>", "src");
        }

        public static bool IsMatchImages(this string html)
        {
            Regex r = new Regex(@"<IMG[^>]+src=\s*(?:'(?<src>[^']+)'|""(?<src>[^""]+)""|(?<src>[^>\s]+))\s*[^>]*>", RegexOptions.IgnoreCase);
            return r.IsMatch(html);
        }

        /// <summary>
        /// 获取文章中图片地址的方法1
        /// </summary>
        /// <param name="html">文章内容</param>
        /// <param name="regstr">正则表达式</param>
        /// <param name="keyname">关键属性名</param>
        /// <returns></returns>
        public static List<string> GetImgUrl(this string html, string regstr, string keyname)
        {
            List<string> resultStr = new List<string>();
            Regex r = new Regex(regstr, RegexOptions.IgnoreCase);
            MatchCollection mc = r.Matches(html);

            foreach (Match m in mc)
            {
                resultStr.Add(m.Groups[keyname].Value);
            }
            return resultStr;
        }

    }
}
