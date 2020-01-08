﻿namespace Nigel.Core
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/30 17:40:43
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension classes for integers.
    /// </summary>
    public static class IntegerExtensions
    {
        #region Loops
        /// <summary>
        /// Iterates the action using a for loop from 0 to ndx.
        /// </summary>
        /// <param name="ndx">The number of times to iterate ( 0 based )</param>
        /// <param name="action">The action to call</param>
        public static void Times(this int ndx, Action<int> action)
        {
            for (int i = 0; i < ndx; i++)
            {
                action(i);
            }
        }


        /// <summary>
        /// Iterates over the action from the start to the end non-inclusive.
        /// </summary>
        /// <param name="start">The starting number.</param>
        /// <param name="end">The ending number ( non-inclusive )</param>
        /// <param name="action">Action to call.</param>
        public static void Upto(this int start, int end, Action<int> action)
        {
            for (int i = start; i < end; i++)
            {
                action(i);
            }
        }


        /// <summary>
        /// Iterates over the action from start to end including the end number.
        /// </summary>
        /// <param name="start">The starting number.</param>
        /// <param name="end">The ending number ( inclusive )</param>
        /// <param name="action">Action to call.</param>
        public static void UptoIncluding(this int start, int end, Action<int> action)
        {
            for (int i = start; i <= end; i++)
            {
                action(i);
            }
        }


        /// <summary>
        /// Iterates over the action from the end to the start non-inclusive.
        /// </summary>
        /// <param name="start">The starting number  ( non-inclusive ).</param>
        /// <param name="end">The ending number</param>
        /// <param name="action">Action to call.</param>
        public static void Downto(this int end, int start, Action<int> action)
        {
            for (int i = end; i > start; i--)
            {
                action(i);
            }
        }


        /// <summary>
        /// Iterates over the action from the end to the start inclusive.
        /// </summary>
        /// <param name="start">The starting number  ( inclusive ).</param>
        /// <param name="end">The ending number</param>
        /// <param name="action">Action to call.</param>
        public static void DowntoIncluding(this int end, int start, Action<int> action)
        {
            for (int i = end; i >= start; i--)
            {
                action(i);
            }
        }
        #endregion


        #region Math
        /// <summary>
        /// Determines if the number provided is an odd number.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsOdd(this int num)
        {
            return num % 2 != 0;
        }


        /// <summary>
        /// Determines if the number provided is an even number.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsEven(this int num)
        {
            return num % 2 == 0;
        }
        #endregion


        #region Bytes
        /// <summary>
        /// Converts the number provided into kilobytes by dividing it by 1000;
        /// </summary>
        /// <param name="numberInBytes">Number in bytes</param>
        /// <returns></returns>
        public static int MegaBytes(this int numberInBytes)
        {
            return numberInBytes / 1000000;
        }


        /// <summary>
        /// Converts the number provided into kilobytes by dividing it by 1000;
        /// </summary>
        /// <param name="numberInBytes">Number in bytes</param>
        /// <returns></returns>
        public static int KiloBytes(this int numberInBytes)
        {
            return numberInBytes / 1000;
        }


        /// <summary>
        /// Converts the number provided into kilobytes by dividing it by 1000;
        /// </summary>
        /// <param name="numberInBytes">Number in bytes</param>
        /// <returns></returns>
        public static int TeraBytes(this int numberInBytes)
        {
            return numberInBytes / 1000000000;
        }
        #endregion


        #region Dates
        /// <summary>
        /// Determines whether or not year supplied is a leap year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(this int year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }


        public static DateTime DaysAgo(this int num)
        {
            return DateTime.Now.AddDays(-num);
        }


        public static DateTime MonthsAgo(this int num)
        {
            return DateTime.Now.AddMonths(-num);
        }


        public static DateTime YearsAgo(this int num)
        {
            return DateTime.Now.AddYears(-num);
        }


        public static DateTime HoursAgo(this int num)
        {
            return DateTime.Now.AddHours(-num);
        }


        public static DateTime MinutesAgo(this int num)
        {
            return DateTime.Now.AddMinutes(-num);
        }


        public static DateTime DaysFromNow(this int num)
        {
            return DateTime.Now.AddDays(num);
        }


        public static DateTime MonthsFromNow(this int num)
        {
            return DateTime.Now.AddMonths(num);
        }


        public static DateTime YearsFromNow(this int num)
        {
            return DateTime.Now.AddYears(num);
        }


        public static DateTime HoursFromNow(this int num)
        {
            return DateTime.Now.AddHours(num);
        }


        public static DateTime MinutesFromNow(this int num)
        {
            return DateTime.Now.AddMinutes(num);
        }
        #endregion


        #region Time
        /// <summary>
        /// Converts the number to days as a TimeSpan.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static TimeSpan Days(this int num)
        {
            return new TimeSpan(num, 0, 0, 0);
        }


        public static TimeSpan Hours(this int num)
        {
            return new TimeSpan(0, num, 0, 0);
        }


        public static TimeSpan Minutes(this int num)
        {
            return new TimeSpan(0, 0, num, 0);
        }


        public static TimeSpan Seconds(this int num)
        {
            return new TimeSpan(0, 0, 0, num);
        }


        public static TimeSpan Time(this int num)
        {
            return Time(num, false);
        }



        /// <summary>
        /// Converts the military time to a timespan.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="convertSingleDigitsToHours">Indicates whether to treat "9" as 9 hours instead of minutes.</param>
        /// <returns></returns>
        public static TimeSpan Time(this int num, bool convertSingleDigitsToHours)
        {
            TimeSpan time = TimeSpan.MinValue;
            if (convertSingleDigitsToHours)
            {
                if (num <= 24)
                    num *= 100;
            }
            int hours = num / 100;
            int hour = hours;
            int minutes = num % 100;

            time = new TimeSpan(hours, minutes, 0);
            return time;
        }


        /// <summary>
        /// Returns military time formatted as standard time w/ suffix am/pm.
        /// e.g. 1am 9:30pm
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string TimeWithSuffix(this int num)
        {
            TimeSpan time = Time(num, true);
            return TimeHelper.Format(time);
        }
        #endregion
    }
}
