namespace Nigel.Core
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 11:34:17
    *                   mailto:3624091@qq.com
    *                         
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Security;

    public static class TypeConversionExtensions
    {
        /// <summary>
        /// 泛型类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="convertibleValue"></param>
        /// <returns></returns>
        public static T Convert<T>(this IConvertible convertibleValue, T defaultValue = default(T))
        {
            if (convertibleValue == null) return defaultValue;

            return (T)ConvertValue(convertibleValue, typeof(T));
        }
        /// <summary>
        /// 泛型类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Convert<T>(this object value, T defaultValue = default(T))
        {
            if (value == null) return defaultValue;

            return (T)ConvertValue(value, typeof(T));
        }

        public static object ConvertValue(object value, Type type)
        {
            if (!type.IsGenericType)
            {
                return System.Convert.ChangeType(value, type);
            }
            else
            {
                Type genericTypeDefinition  = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                    return System.Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            throw new InvalidCastException(
                string.Format("Invalid cast from type \"{0}\" to type \"{1}\".",
                value.GetType().FullName, type.FullName));
        }
    }
}
