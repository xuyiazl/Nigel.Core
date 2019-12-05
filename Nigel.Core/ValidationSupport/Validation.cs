namespace Nigel.Core.ValidationSupport
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 22:01:50
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static partial class Validation
    {

        #region IValidatorBasic Members

        /// <summary>
        /// 如果字符串介于最小/最大长度 则是有效的确定
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <param name="checkMinLength"></param>
        /// <param name="checkMaxLength"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool IsStringLengthMatch(string text, bool allowNull, bool checkMinLength, bool checkMaxLength, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return allowNull;

            if (checkMinLength && minLength > 0 && text.Length < minLength)
                return false;
            if (checkMaxLength && maxLength > 0 && text.Length > maxLength)
                return false;

            return true;
        }


        /// <summary>
        /// 验证大/小范围
        /// </summary>
        /// <returns></returns>
        public static bool IsBetween(int val, bool checkMinLength, bool checkMaxLength, int minLength, int maxLength)
        {
            if (checkMinLength && val < minLength)
                return false;
            if (checkMaxLength && val > maxLength)
                return false;

            return true;
        }


        /// <summary>
        /// 验证千字节大/小范围
        /// </summary>
        /// <returns></returns>
        public static bool IsSizeBetween(int val, bool checkMinLength, bool checkMaxLength, int minKilobytes, int maxKilobytes)
        {
            val = val / 1000;

            if (checkMinLength && val < minKilobytes)
                return false;
            if (checkMaxLength && val > maxKilobytes)
                return false;

            return true;
        }


        /// <summary>
        /// 匹配正则表达式的字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <param name="regEx"></param>
        /// <returns></returns>
        public static bool IsStringRegExMatch(string text, bool allowNull, string regEx)
        {
            if (allowNull && string.IsNullOrEmpty(text))
                return true;

            return Regex.IsMatch(text, regEx);
        }


        /// <summary>
        /// 是否是数值类型
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNumeric(string text)
        {
            return Regex.IsMatch(text, RegexPatterns.Numeric);
        }


        /// <summary>
        /// 是否是数值类型，并验证数值的最小/最大范围
        /// </summary>
        /// <param name="text"></param>
        /// <param name="checkMinBound"></param>
        /// <param name="checkMaxBound"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsNumericWithinRange(string text, bool checkMinBound, bool checkMaxBound, double min, double max)
        {
            bool isNumeric = Regex.IsMatch(text, RegexPatterns.Numeric);
            if (!isNumeric) return false;

            double num = Double.Parse(text);
            return IsNumericWithinRange(num, checkMinBound, checkMaxBound, min, max);
        }


        /// <summary>
        /// 验证数值的最小/最大范围
        /// </summary>
        /// <param name="num"></param>
        /// <param name="checkMinBound"></param>
        /// <param name="checkMaxBound"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsNumericWithinRange(double num, bool checkMinBound, bool checkMaxBound, double min, double max)
        {
            if (checkMinBound && num < min)
                return false;

            if (checkMaxBound && num > max)
                return false;

            return true;
        }

        /// <summary>
        /// 验证是否是有效的正整数值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNumber(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.Number);
        }

        /// <summary>
        /// 是否是正整数类型，并验证数值的最小/最大范围
        /// </summary>
        /// <param name="text"></param>
        /// <param name="checkMinBound"></param>
        /// <param name="checkMaxBound"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsNumberWithinRange(string text, bool checkMinBound, bool checkMaxBound, int min, int max)
        {
            bool isNumeric = Regex.IsMatch(text, RegexPatterns.Number);
            if (!isNumeric) return false;

            int num = int.Parse(text);
            return IsNumberWithinRange(num, checkMinBound, checkMaxBound, min, max);
        }


        /// <summary>
        /// 验证数值的最小/最大范围
        /// </summary>
        /// <param name="num"></param>
        /// <param name="checkMinBound"></param>
        /// <param name="checkMaxBound"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsNumberWithinRange(int num, bool checkMinBound, bool checkMaxBound, int min, int max)
        {
            if (checkMinBound && num < min)
                return false;

            if (checkMaxBound && num > max)
                return false;

            return true;
        }

        /// <summary>
        /// 验证是否是有效的整数值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNumberSign(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.NumberSign);
        }

        /// <summary>
        /// 是否是正整数类型，并验证数值的最小/最大范围
        /// </summary>
        /// <param name="text"></param>
        /// <param name="checkMinBound"></param>
        /// <param name="checkMaxBound"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsNumberSignWithinRange(string text, bool checkMinBound, bool checkMaxBound, int min, int max)
        {
            bool isNumeric = Regex.IsMatch(text, RegexPatterns.NumberSign);
            if (!isNumeric) return false;

            int num = int.Parse(text);
            return IsNumberWithinRange(num, checkMinBound, checkMaxBound, min, max);
        }

        /// <summary>
        /// 验证是否是有效的中文
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsZHCN(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.ZHCN);
        }


        /// <summary>
        /// 验证是否是有效的大小写字母
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsAlpha(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.Alpha);
        }


        /// <summary>
        /// 验证是否是有效的大小写字母或数字
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsAlphaNumeric(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.AlphaNumeric);
        }


        /// <summary>
        /// 验证是否是日期
        /// </summary>
        /// <param name="text"></param>
        /// <param name="checkBounds"></param>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        /// <returns></returns>
        public static bool IsDate(string text)
        {
            DateTime result = DateTime.MinValue;
            return DateTime.TryParse(text, out result);
        }


        /// <summary>
        /// 验证是否是日期，并验证日期大/小范围
        /// </summary>
        /// <param name="text"></param>
        /// <param name="checkBounds"></param>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        /// <returns></returns>
        public static bool IsDateWithinRange(string text, bool checkMinBound, bool checkMaxBound, DateTime minDate, DateTime maxDate)
        {
            DateTime result = DateTime.MinValue;
            if (!DateTime.TryParse(text, out result)) return false;

            return IsDateWithinRange(result, checkMinBound, checkMaxBound, minDate, maxDate);
        }


        /// <summary>
        /// 验证是否是日期，并验证日期大/小范围
        /// </summary>
        /// <param name="date"></param>
        /// <param name="checkMinBound"></param>
        /// <param name="checkMaxBound"></param>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        /// <returns></returns>
        public static bool IsDateWithinRange(DateTime date, bool checkMinBound, bool checkMaxBound, DateTime minDate, DateTime maxDate)
        {
            if (checkMinBound && date.Date < minDate.Date) return false;
            if (checkMaxBound && date.Date > maxDate.Date) return false;

            return true;
        }


        /// <summary>
        /// 验证是否是有效的时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="checkBounds"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsTimeOfDay(string time)
        {
            TimeSpan span = TimeSpan.MinValue;
            return TimeSpan.TryParse(time, out span);
        }


        /// <summary>
        /// 验证是否是有效的时间，并验证时间大/小范围
        /// </summary>
        /// <param name="time"></param>
        /// <param name="checkBounds"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsTimeOfDayWithinRange(string time, bool checkMinBound, bool checkMaxBound, TimeSpan min, TimeSpan max)
        {
            TimeSpan span = TimeSpan.MinValue;
            if (!TimeSpan.TryParse(time, out span))
                return false;

            return IsTimeOfDayWithinRange(span, checkMinBound, checkMaxBound, min, max);
        }


        /// <summary>
        /// 验证是否是有效的时间，并验证时间大/小范围
        /// </summary>
        /// <param name="time"></param>
        /// <param name="checkBounds"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsTimeOfDayWithinRange(TimeSpan time, bool checkMinBound, bool checkMaxBound, TimeSpan min, TimeSpan max)
        {
            if (checkMinBound && time < min)
                return false;
            if (checkMaxBound && time > max)
                return false;
            return true;
        }


        /// <summary>
        /// 是否是手机号码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.MobilePhone);
        }


        /// <summary>
        /// 是否是手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(int phone)
        {
            return Regex.IsMatch(phone.ToString(), RegexPatterns.MobilePhone);
        }

        /// <summary>
        /// 是否是电话号码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        public static bool IsTelPhone(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.TelPhone);
        }


        /// <summary>
        /// 是否是电话号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsTelPhone(int phone)
        {
            return Regex.IsMatch(phone.ToString(), RegexPatterns.TelPhone);
        }


        /// <summary>
        /// 是否是身份证号码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        public static bool IsIdentityCard(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.IdentityCard);
        }


        /// <summary>
        /// 是否是身份证号码
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public static bool IsIdentityCard(int identityCard)
        {
            return Regex.IsMatch(identityCard.ToString(), RegexPatterns.IdentityCard);
        }


        /// <summary>
        /// 是否是Email
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        public static bool IsEmail(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.Email);
        }


        /// <summary>
        /// 是否是URL
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        public static bool IsUrl(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.Url);
        }


        /// <summary>
        /// 是否是邮政编码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        public static bool IsZipCode(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.ZipCode);
        }


        /// <summary>
        /// 是否等于
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool AreEqual<T>(T obj1, T obj2) where T : IComparable<T>
        {
            return obj1.CompareTo(obj2) == 0;
        }


        /// <summary>
        /// 是否不等于
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool AreNotEqual<T>(T obj1, T obj2) where T : IComparable<T>
        {
            return obj1.CompareTo(obj2) != 0;
        }
        #endregion


        public static bool IsMatchRegEx(string text, bool allowNull, string regExPattern)
        {
            bool isEmpty = string.IsNullOrEmpty(text);
            if (isEmpty && allowNull) return true;
            if (isEmpty && !allowNull) return false;

            return Regex.IsMatch(text, regExPattern);
        }
    }
}
