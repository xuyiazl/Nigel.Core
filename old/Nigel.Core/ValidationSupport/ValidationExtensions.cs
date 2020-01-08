namespace Nigel.Core.ValidationSupport
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 22:16:46
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    //using Val = System.ComponentModel.DataAnnotations;
    using System.Collections;

    using Nigel.Core.Messages;

    public static partial class Validation
    {
        #region IValidatorBasic Members
        ///// <summary>
        ///// 验证对象使用System.ComponentModel.DataAnnotations，并将其放入到错误对象。
        ///// </summary>
        ///// <remarks>C# 4.0</remarks>
        ///// <param name="objectToValidate">验证对象</param>
        ///// <param name="errors">缺省错误集合/param>
        ///// <returns></returns>
        //public static bool ValidateObject(object objectToValidate, IErrors errors = null)
        //{

        //    var validationResults = new List<Val.ValidationResult>();
        //    var ctx = new Val.ValidationContext(objectToValidate, null, null);
        //    var isValid = Val.Validator.TryValidateObject(objectToValidate, ctx, validationResults, true);

        //    if (!isValid && errors != null)
        //        foreach (var result in validationResults)
        //            errors.Add(result.ErrorMessage);

        //    return isValid;
        //}


        /// <summary>
        /// 如果字符串介于最小/最大长度 则是有效的确定
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="allowNull">是否可为空</param>
        /// <param name="minLength">-1不检查，>0代表最小长度</param>
        /// <param name="maxLength">-1不检查，>0表示的最大长度。</param>
        /// <returns>true/false</returns>
        public static bool IsStringLengthMatch(string text, bool allowNull, bool checkMinLength, bool checkMaxLength, int minLength, int maxLength, IErrors errors, string errorKey)
        {
            if (string.IsNullOrEmpty(text) && allowNull) return true;

            int textLength = text == null ? 0 : text.Length;

            if (checkMinLength && minLength > 0 && textLength < minLength)
                return CheckError(false, errors, errorKey, "文本提供的最小长度不小于(" + minLength + ")");

            if (checkMaxLength && maxLength > 0 && textLength > maxLength)
                return CheckError(false, errors, errorKey, "文本提供的最大长度不大于(" + maxLength + ")");

            return true;
        }


        /// <summary>
        /// 匹配正则表达式的字符串。
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="allowNull">是否可为空</param>
        /// <param name="regEx">正则字符串</param>
        /// <returns>true/false</returns>
        public static bool IsStringRegExMatch(string text, bool allowNull, string regEx, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, regEx, errors, errorKey, "文本不匹配");
        }


        /// <summary>
        /// 文本是否[存在字符串] [指定的值]。
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="allowNull">是否可为空</param>
        /// <param name="compareCase">是否考虑大小写</param>
        /// <param name="allowedValues">包含值</param>
        /// <param name="errors">返回的错误信息</param>
        /// <param name="errorKey"></param>
        /// <returns>
        /// 	包含 true ,不包含 false
        /// </returns>
        public static bool IsStringIn(string text, bool allowNull, bool compareCase, string[] allowedValues, IErrors errors, string errorKey)
        {
            bool isEmpty = string.IsNullOrEmpty(text);
            if (isEmpty && allowNull) return true;
            if (isEmpty && !allowNull)
            {
                string vals = allowedValues.JoinDelimited(",", (val) => val);
                errors.Add(errorKey, "文本必须包含 : " + vals);
                return false;
            }
            bool isValid = false;
            foreach (string val in allowedValues)
            {
                if (string.Compare(text, val, compareCase) == 0)
                {
                    isValid = true;
                    break;
                }
            }
            if (!isValid)
            {
                string vals = allowedValues.JoinDelimited(",", (val) => val);
                errors.Add(errorKey, "文本必须包含 : " + vals);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否是数值类型
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsNumeric(string text, bool allowNull, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.Numeric, errors, errorKey, "不是有效的数值类型");
        }


        /// <summary>
        /// 是否是数值类型，并验证数值的最小/最大范围
        /// </summary>
        /// <param name="text">数值文本</param>
        /// <param name="checkMinBound">是否验证最小范围</param>
        /// <param name="checkMaxBound">是否验证最大范围</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsNumericWithinRange(string text, bool checkMinBound, bool checkMaxBound, double min, double max, IErrors errors, string tag)
        {
            bool isNumeric = Regex.IsMatch(text, RegexPatterns.Numeric);
            if (!isNumeric)
            {
                errors.Add(tag, "不是有效的数值类型");
                return false;
            }

            double num = Double.Parse(text);
            return IsNumericWithinRange(num, checkMinBound, checkMaxBound, min, max, errors, tag);
        }


        /// <summary>
        /// 验证数值的最小/最大范围
        /// </summary>
        /// <param name="num">验证的数值</param>
        /// <param name="checkMinBound">是否验证最小范围</param>
        /// <param name="checkMaxBound">是否验证最大范围</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsNumericWithinRange(double num, bool checkMinBound, bool checkMaxBound, double min, double max, IErrors errors, string errorKey)
        {
            if (checkMinBound && num < min)
            {
                errors.Add(errorKey, "数值小于 " + min + ".");
                return false;
            }

            if (checkMaxBound && num > max)
            {
                errors.Add(errorKey, "数值大于 " + max + ".");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否是数值类型
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsNumber(string text, bool allowNull, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.Number, errors, errorKey, "不是有效的正整数");
        }


        /// <summary>
        /// 是否是正整数，并验证数值的最小/最大范围
        /// </summary>
        /// <param name="text">数值文本</param>
        /// <param name="checkMinBound">是否验证最小范围</param>
        /// <param name="checkMaxBound">是否验证最大范围</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsNumberWithinRange(string text, bool checkMinBound, bool checkMaxBound, int min, int max, IErrors errors, string tag)
        {
            bool isNumeric = Regex.IsMatch(text, RegexPatterns.Number);
            if (!isNumeric)
            {
                errors.Add(tag, "不是有效的整数");
                return false;
            }

            int num = int.Parse(text);
            return IsNumberWithinRange(num, checkMinBound, checkMaxBound, min, max, errors, tag);
        }

        /// <summary>
        /// 是否是数值类型
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsNumberSign(string text, bool allowNull, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.NumberSign, errors, errorKey, "不是有效的整数");
        }


        /// <summary>
        /// 是否是正整数，并验证数值的最小/最大范围
        /// </summary>
        /// <param name="text">数值文本</param>
        /// <param name="checkMinBound">是否验证最小范围</param>
        /// <param name="checkMaxBound">是否验证最大范围</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsNumberSignWithinRange(string text, bool checkMinBound, bool checkMaxBound, int min, int max, IErrors errors, string tag)
        {
            bool isNumeric = Regex.IsMatch(text, RegexPatterns.NumberSign);
            if (!isNumeric)
            {
                errors.Add(tag, "不是有效的整数");
                return false;
            }

            int num = int.Parse(text);
            return IsNumberWithinRange(num, checkMinBound, checkMaxBound, min, max, errors, tag);
        }


        /// <summary>
        /// 验证数值的最小/最大范围
        /// </summary>
        /// <param name="num">验证的数值</param>
        /// <param name="checkMinBound">是否验证最小范围</param>
        /// <param name="checkMaxBound">是否验证最大范围</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsNumberWithinRange(int num, bool checkMinBound, bool checkMaxBound, int min, int max, IErrors errors, string errorKey)
        {
            if (checkMinBound && num < min)
            {
                errors.Add(errorKey, "整数小于 " + min + ".");
                return false;
            }

            if (checkMaxBound && num > max)
            {
                errors.Add(errorKey, "整数大于 " + max + ".");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证是否是中文
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsZHCN(string text, bool allowNull, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.ZHCN, errors, errorKey, "必须只包含中文 " + RegexPatterns.ZHCN);
        }

        /// <summary>
        /// 验证是否是有效的大小写字母
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsAlpha(string text, bool allowNull, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.Alpha, errors, errorKey, "必须只包含大小写字母 " + RegexPatterns.Alpha);
        }


        /// <summary>
        /// 验证是否是有效的大小写字母或数字
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsAlphaNumeric(string text, bool allowNull, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.AlphaNumeric, errors, errorKey, "必须只包含字母和数字字符 " + RegexPatterns.AlphaNumeric);
        }


        /// <summary>
        /// 验证是否是日期
        /// </summary>
        /// <param name="text"></param>
        /// <param name="checkBounds"></param>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        /// <returns></returns>
        public static bool IsDate(string text, IErrors errors, string errorKey)
        {
            DateTime result = DateTime.MinValue;
            return CheckError(DateTime.TryParse(text, out result), errors, errorKey, "不是一个有效的日期");
        }


        /// <summary>
        /// 验证是否是日期，并验证日期大/小范围
        /// </summary>
        /// <param name="text"></param>
        /// <param name="checkBounds"></param>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        /// <returns></returns>
        public static bool IsDateWithinRange(string text, bool checkMinBound, bool checkMaxBound, DateTime minDate, DateTime maxDate, IErrors errors, string errorKey)
        {
            DateTime result = DateTime.MinValue;
            if (!DateTime.TryParse(text, out result))
            {
                errors.Add(errorKey, "不是一个有效的日期");
                return false;
            }

            return IsDateWithinRange(result, checkMinBound, checkMaxBound, minDate, maxDate, errors, errorKey);
        }


        /// <summary>
        /// 验证日期大/小范围
        /// </summary>
        /// <param name="date"></param>
        /// <param name="checkMinBound"></param>
        /// <param name="checkMaxBound"></param>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        /// <returns></returns>
        public static bool IsDateWithinRange(DateTime date, bool checkMinBound, bool checkMaxBound, DateTime minDate, DateTime maxDate, IErrors errors, string errorKey)
        {
            if (checkMinBound && date.Date < minDate.Date)
            {
                errors.Add(errorKey, "日期不能小于： " + minDate.ToString());
                return false;
            }
            if (checkMaxBound && date.Date > maxDate.Date)
            {
                errors.Add(errorKey, "日期不能大于： " + maxDate.ToString());
                return false;
            }

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
        public static bool IsTimeOfDay(string time, IErrors errors, string errorKey)
        {
            TimeSpan span = TimeSpan.MinValue;
            bool isMatch = TimeSpan.TryParse(time, out span);
            return CheckError(isMatch, errors, errorKey, "不是一个有效的时间.");
        }


        /// <summary>
        /// 验证是否是有效的时间，并验证时间大/小范围
        /// </summary>
        /// <param name="time"></param>
        /// <param name="checkBounds"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsTimeOfDayWithinRange(string time, bool checkMinBound, bool checkMaxBound, TimeSpan min, TimeSpan max, IErrors errors, string errorKey)
        {
            TimeSpan span = TimeSpan.MinValue;
            if (!TimeSpan.TryParse(time, out span))
                return CheckError(false, errors, errorKey, "不是一个有效的时间.");

            return IsTimeOfDayWithinRange(span, checkMinBound, checkMaxBound, min, max, errors, errorKey);
        }


        /// <summary>
        /// 验证时间大/小范围
        /// </summary>
        /// <param name="time"></param>
        /// <param name="checkBounds"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsTimeOfDayWithinRange(TimeSpan time, bool checkMinBound, bool checkMaxBound, TimeSpan min, TimeSpan max, IErrors errors, string errorKey)
        {
            if (checkMinBound && time < min)
            {
                errors.Add(errorKey, "时间不能小于： " + min.ToString());
                return false;
            }
            if (checkMaxBound && time > max)
            {
                errors.Add(errorKey, "时间不能大于： " + max.ToString());
                return false;
            }
            return true;
        }


        /// <summary>
        /// 验证是否是有效的手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(string text, bool allowNull, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.MobilePhone, errors, errorKey, "不是一个有效的手机号码.");
        }


        /// <summary>
        /// 验证是否是有效的手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(int phone, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(phone.ToString(), false, RegexPatterns.TelPhone, errors, errorKey, "不是一个有效的手机号码.");
        }

        /// <summary>
        /// 验证是否是有效的电话号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsTelPhone(string text, bool allowNull, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.TelPhone, errors, errorKey, "不是一个有效的电话号码.");
        }


        /// <summary>
        /// 验证是否是有效的电话号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsTelPhone(int phone, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(phone.ToString(), false, RegexPatterns.MobilePhone, errors, errorKey, "不是一个有效的电话号码.");
        }


        /// <summary>
        /// 验证是否是有效的身份证号码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsIdentityCard(string text, bool allowNull, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.IdentityCard, errors, errorKey, "不是一个有效的身份证号码.");
        }


        /// <summary>
        /// 验证是否是有效的身份证号码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsIdentityCard(int identityCard, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(identityCard.ToString(), false, RegexPatterns.IdentityCard, errors, errorKey, "不是一个有效的身份证号码.");
        }


        /// <summary>
        /// 验证是否是有效的Email
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsEmail(string text, bool allowNull, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.Email, errors, errorKey, "不是一个有效的Email.");
        }


        /// <summary>
        /// 验证是否是有效的url
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsUrl(string text, bool allowNull, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.Url, errors, errorKey, "不是一个有效的URL.");
        }


        /// <summary>
        /// 验证邮编
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="allowNull">是否可为空</param>
        /// <param name="errors">记录错误的对象</param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public static bool IsZipCode(string text, bool allowNull, IErrors errors, string errorKey)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.ZipCode, errors, errorKey, "不是一个有效的邮政编码.");
        }
        #endregion


        /// <summary>
        /// 是否增加错误信息
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool CheckError(bool isValid, IErrors errors, string errorKey, string error)
        {
            if (!isValid)
            {
                errors.Add(errorKey, error);
            }
            return isValid;
        }


        /// <summary>
        /// 验证文本，并增加错误信息
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="allowNull"></param>
        /// <param name="regExPattern"></param>
        /// <param name="errors"></param>
        /// <param name="errorKey"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool CheckErrorRegEx(string inputText, bool allowNull, string regExPattern, IErrors errors, string errorKey, string error)
        {
            bool isEmpty = string.IsNullOrEmpty(inputText);
            if (allowNull && isEmpty)
                return true;

            if (!allowNull && isEmpty)
            {
                errors.Add(errorKey, error);
                return false;
            }

            bool isValid = Regex.IsMatch(inputText, regExPattern);
            if (!isValid) errors.Add(errorKey, error);

            return isValid;
        }

        public static string errorKey { get; set; }
    }
}
