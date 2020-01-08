/********************************************************************
*           Copyright:       2009-2012
*           Company:         证券之星
*           CRL Version :    4.0.30319.17929
*           Created by 徐毅 at 2012/11/12 16:22:45
*                   mailto:yi.xu@stockstar.com
*                         集团研发中心
********************************************************************/

namespace Nigel.Core
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Linq.Expressions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class JsonSerializerSettingsUtil
    {
        public JsonSerializerSettings Default
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    // EF Core中默认为驼峰样式序列化处理key
                    //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    //使用默认方式，不更改元数据的key的大小写
                    ContractResolver = new DefaultContractResolver(),
                    DateFormatString = "yyyy-MM-dd HH:mm:ss",
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
            }
        }
    }

    public static class JsonExtensions
    {
        /// <summary>
		/// 反序列化
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T ToJsonObject<T>(this string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value)) return default(T);

                return JsonConvert.DeserializeObject<T>(value);
            }
            catch { }
            return default(T);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static T ToJsonObject<T>(this string value, JsonSerializerSettings settings)
        {
            try
            {
                if (string.IsNullOrEmpty(value)) return default(T);

                return JsonConvert.DeserializeObject<T>(value, settings);
            }
            catch { }
            return default(T);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToJsonObject<T>(this object value)
        {
            try
            {
                var str = value.NullToEmpty();

                if (string.IsNullOrEmpty(str)) return default(T);

                return JsonConvert.DeserializeObject<T>(str);
            }
            catch { }
            return default(T);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static T ToJsonObject<T>(this object value, JsonSerializerSettings settings)
        {
            try
            {
                var str = value.NullToEmpty();

                if (string.IsNullOrEmpty(str)) return default(T);

                return JsonConvert.DeserializeObject<T>(str, settings);
            }
            catch { }
            return default(T);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString<T>(this T obj)
        {
            try
            {
                if (obj == null) return string.Empty;

                return JsonConvert.SerializeObject(obj);
            }
            catch { }
            return string.Empty;
        }
    }
}
