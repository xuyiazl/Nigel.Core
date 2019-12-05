namespace Nigel.Core
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/30 17:42:49
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections.Specialized;

    /// <summary>
    /// Extension classes for NameValueCollection.
    /// </summary>
    public class NameValueExtensions
    {
        /// <summary>
        /// Gets the value associated w/ the key, if it's empty returns the default value.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetOrDefault(NameValueCollection collection, string key, string defaultValue)
        {
            if (collection == null) return defaultValue;

            string val = collection[key];
            if (string.IsNullOrEmpty(val))
                return defaultValue;

            return val;
        }


        /// <summary>
        /// Gets the value associated w/ the key and convert it to the correct Type, if empty returns the default value.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="collection">Collection.</param>
        /// <param name="key">The key representing the value to get.</param>
        /// <param name="defaultValue">Value to return if the key has an empty value.</param>
        /// <returns></returns>
        public static T GetOrDefault<T>(NameValueCollection collection, string key, T defaultValue)
        {
            if (collection == null) return defaultValue;

            string val = collection[key];
            if (string.IsNullOrEmpty(val))
                return defaultValue;

            return val.Convert<T>();
        }
    }
}
