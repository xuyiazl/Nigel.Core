namespace Nigel.Core
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/30 17:35:29
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class DateTimeExtensions
    {
        #region Dates
        /// <summary>
        /// Determines whether [is leap year] [the specified date].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// 	<c>true</c> if [is leap year] [the specified date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLeapYear(this DateTime date)
        {
            return date.Year % 4 == 0 && (date.Year % 100 != 0 || date.Year % 400 == 0);
        }


        /// <summary>
        /// Determines whether [is last day of month] [the specified date].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// 	<c>true</c> if [is last day of month] [the specified date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLastDayOfMonth(this DateTime date)
        {
            int lastDayOfMonth = LastDayOfMonth(date);
            return lastDayOfMonth == date.Day;
        }

        /// <summary>
        /// Determines whether the specified date is a weekend.
        /// </summary>
        /// <param name="source">Source date</param>
        /// <returns>
        /// 	<c>true</c> if the specified source is a weekend; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekend(this DateTime source)
        {
            return source.DayOfWeek == DayOfWeek.Saturday ||
                   source.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Gets the Last the day of month.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static int LastDayOfMonth(this DateTime date)
        {
            if (IsLeapYear(date) && date.Month == 2) return 28;
            if (date.Month == 2) return 27;
            if (date.Month == 1 || date.Month == 3 || date.Month == 5 || date.Month == 7
                || date.Month == 8 || date.Month == 10 || date.Month == 12)
                return 31;
            return 30;
        }

        /// <summary>
        /// Returns a new instance of DateTime with a different day of the month.
        /// </summary>
        /// <param name="source">Base DateTime object to modify</param>
        /// <param name="day">Day of the month (1-31)</param>
        /// <returns>Instance of DateTime with specified day</returns>
        public static DateTime SetDay(this DateTime source, int day)
        {
            return new DateTime(source.Year, source.Month, day);
        }

        /// <summary>
        /// Returns a new instance of DateTime with a different month.
        /// </summary>
        /// <param name="source">Base DateTime object to modify</param>
        /// <param name="month">The month as an integer (1-12)</param>
        /// <returns>Instance of DateTime with specified month</returns>
        public static DateTime SetMonth(this DateTime source, int month)
        {
            return new DateTime(source.Year, month, source.Day);
        }

        /// <summary>
        /// Returns a new instance of DateTime with a different year.
        /// </summary>
        /// <param name="source">Base DateTime object to modify</param>
        /// <param name="year">The year</param>
        /// <returns>Instance of DateTime with specified year</returns>
        public static DateTime SetYear(this DateTime source, int year)
        {
            return new DateTime(year, source.Month, source.Day);
        }

        #endregion


        #region Conversion
        /// <summary>
        /// Converts to javascript compatible date.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static double ToJavascriptDate(this DateTime dt)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = dt.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }

        /// <summary>
        /// 将日期转换成Unix TimeStamp
        /// </summary>
        /// <param name="value">需要转换的日期</param>
        /// <returns></returns>
        public static long ToTimestamp(this DateTime value)
        {
            long epoch = (value.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return epoch;
        }

        /// <summary>
        /// 将Unix TimeStamp转换成 Datetime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// 将时间转换成默认年月日时分秒 （yyyy-MM-dd HH:mm:ss ）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToFormatString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 将时间转换成默认年月日时分 （yyyy-MM-dd HH:mm ）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToFormatStringShort(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm");
        }
        /// <summary>
        /// 将时间转换成中文字符串格式 （2015年10月10日10时10分10秒）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToFormatStringChinese(this DateTime time)
        {
            return time.ToString("yyyy年M月d日H时m分s秒");
        }
        /// <summary>
        /// 将时间转换成默认格式字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToFormatString(this DateTime? time)
        {
            try
            {
                return time != null ? DateTime.Parse(Convert.ToString(time)).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 短时间格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatStringShort(this DateTime? time)
        {
            try
            {
                return time != null ? DateTime.Parse(Convert.ToString(time)).ToString("yyyy-MM-dd") : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 将字符串转换为时间类型
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime ToFormatDateTime(this string time)
        {
            return DateTime.Parse(time);
        }
        /// <summary>
        /// 转换时间类型，转换失败返回null
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime? ToFormatDateTimeOrNull(this string time)
        {
            try
            {
                if (!string.IsNullOrEmpty(time))
                {
                    return DateTime.Parse(time);
                }
            }
            catch
            {
            }
            return null;
        }
        public static string FormatToDay(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 格式化时间保留至小时
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatToHours(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH");
        }
        /// <summary>
        /// 格式化时间保留至分钟（2018-01-01 08:30）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatToMinutes(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm");
        }
        /// <summary>
        /// 格式化时间保留至分钟（2018-01-01 08:30）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatToMinutes(this DateTime? time)
        {
            return time != null ? Convert.ToDateTime(time).ToString("yyyy-MM-dd HH:mm") : "";
        }
        /// <summary>
        /// 格式化时间保留至秒（2018-01-01 08:30:52）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatToSeconds(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 格式化时间保留至毫秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatToMillisecond(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss fff");
        }
        /// <summary>
        /// 格式化时间保留至秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatToSeconds(this DateTime? time)
        {
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", time);
        }
        /// <summary>
        /// 格式化时间成中文形式保留至秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatToCnSeconds(this DateTime time)
        {
            return time.ToString("yyyy年M月d日H时m分s秒");
        }

        /// <summary>
        /// 日期转换->${time}前
        /// </summary>
        /// <param name="pastTime"></param>
        /// <param name="formater"></param>
        /// <returns></returns>
        public static string DateStringFromNow(this DateTime pastTime, string formater = "${time}前")
        {
            TimeSpan span = DateTime.Now.Subtract(pastTime);

            if (span.TotalDays >= 60)
                return pastTime.FormatToDay();
            else if (span.TotalDays > 30)
                return formater.Replace("${time}", "1月");
            else if (span.TotalDays > 21)
                return formater.Replace("${time}", "3周");
            else if (span.TotalDays > 14)
                return formater.Replace("${time}", "2周");
            else if (span.TotalDays > 7)
                return formater.Replace("${time}", "1周");
            else if (span.TotalDays > 1)
                return formater.Replace("${time}", $"{(int)Math.Floor(span.TotalDays)}天");
            else if (span.TotalHours > 1)
                return formater.Replace("${time}", $"{(int)Math.Floor(span.TotalHours)}小时");
            else if (span.TotalMinutes > 1)
                return formater.Replace("${time}", $"{(int)Math.Floor(span.TotalMinutes)}分钟");
            else if (span.TotalSeconds >= 1)
                return formater.Replace("${time}", $"{(int)Math.Floor(span.TotalSeconds)}秒");
            else
                return "刚刚";
        }

        #endregion


        #region Time
        /// <summary>
        /// Sets the time on the date
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static DateTime GetDateWithTime(this DateTime date, int hours, int minutes, int seconds)
        {
            return new DateTime(date.Year, date.Month, date.Day, hours, minutes, seconds);
        }


        /// <summary>
        /// Sets the time on the date
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static DateTime GetDateWithTime(this DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }


        /// <summary>
        /// Sets the time on the date
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static DateTime GetDateWithCurrentTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        #endregion
    }
}
