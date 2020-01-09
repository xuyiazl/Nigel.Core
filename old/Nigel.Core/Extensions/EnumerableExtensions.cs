﻿namespace Nigel.Core
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 22:22:42
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Nigel.Core.Collection;
    using Nigel.Core.Comparer;

    public static class EnumerableExtensions
    {
        /// <summary>
        /// 模型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pagedList"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static PagedModel<TResult> ToMap<T, TResult>(this PagedList<T> pagedList, Func<T, TResult> converter)
        {
            return new PagedModel<TResult>(pagedList.ForEach(converter), pagedList.TotalRecords, pagedList.TotalPages, pagedList.PageNumber, pagedList.PageSize);
        }
        /// <summary>
        /// 模型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pagedList"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static PagedModel<TResult> ToMap<T, TResult>(this PagedList<T> pagedList, Func<T, int, TResult> converter)
        {
            return new PagedModel<TResult>(pagedList.ForEach(converter), pagedList.TotalRecords, pagedList.TotalPages, pagedList.PageNumber, pagedList.PageSize);
        }
        /// <summary>
        /// 模型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pagedList"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static PagedSkipModel<TResult> ToMap<T, TResult>(this PagedSkipList<T> pagedList, Func<T, TResult> converter)
        {
            return new PagedSkipModel<TResult>(pagedList.ForEach(converter), pagedList.Limit, pagedList.Offset, pagedList.TotalRecords);
        }
        /// <summary>
        /// 模型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pagedList"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static PagedSkipModel<TResult> ToMap<T, TResult>(this PagedSkipList<T> pagedList, Func<T, int, TResult> converter)
        {
            return new PagedSkipModel<TResult>(pagedList.ForEach(converter), pagedList.Limit, pagedList.Offset, pagedList.TotalRecords);
        }
        /// <summary>
        /// 模型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="items">源数据</param>
        /// <param name="converter">转换表达式</param>
        /// <returns></returns>
        public static IList<TResult> ToMap<T, TResult>(this IEnumerable<T> items, Func<T, TResult> converter)
        {
            return items.ForEach(converter);
        }
        /// <summary>
        /// 模型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="items">源数据</param>
        /// <param name="converter">转换表达式</param>
        /// <returns></returns>
        public static IList<TResult> ToMap<T, TResult>(this IEnumerable<T> items, Func<T, int, TResult> converter)
        {
            return items.ForEach(converter);
        }
        /// <summary>
        /// foreach扩展
        /// </summary>
        /// <param name="from">开始位置</param>
        /// <param name="to">结束位置</param>
        /// <param name="body"></param>
        public static void For(long from, long to, Action<long> body)
        {
            for (; from <= to; from++)
                body(from);
        }
        /// <summary>
        /// foreach扩展
        /// </summary>
        /// <param name="from">开始位置</param>
        /// <param name="to">结束位置</param>
        /// <param name="body"></param>
        public static void For(int from, int to, Action<int> body)
        {
            for (; from <= to; from++)
                body(from);
        }
        /// <summary>
        /// foreach扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
        {
            if (items == null) return;

            int i = 0;
            foreach (var item in items)
                action(item, i++);
        }
        /// <summary>
        /// foreach扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null) return;

            foreach (var item in items)
                action(item);
        }
        /// <summary>
        /// foreach扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">源数据</param>
        /// <param name="converter">转换表达式</param>
        /// <returns></returns>
        public static string ForEach<T>(this IEnumerable<T> items, Func<T, string> converter)
        {
            string val = string.Empty;

            items.ForEach<T>(item =>
            {
                val += converter(item);
            });

            return val;
        }
        /// <summary>
        /// foreach扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="items">源数据</param>
        /// <param name="converter">转换表达式</param>
        /// <returns></returns>
        public static IList<TResult> ForEach<T, TResult>(this IEnumerable<T> items, Func<T, TResult> converter)
        {
            var list = new List<TResult>();

            items.ForEach<T>(item =>
            {
                list.Add(converter(item));
            });

            return list;
        }
        /// <summary>
        /// foreach扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">源数据</param>
        /// <param name="converter">转换表达式</param>
        /// <returns></returns>
        public static string ForEach<T>(this IEnumerable<T> items, Func<T, int, string> converter)
        {
            string val = string.Empty;

            items.ForEach<T>((item, ndx) =>
            {
                val += converter(item, ndx);
            });

            return val;
        }
        /// <summary>
        /// foreach扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="items">源数据</param>
        /// <param name="converter">转换表达式</param>
        /// <returns></returns>
        public static IList<TResult> ForEach<T, TResult>(this IEnumerable<T> items, Func<T, int, TResult> converter)
        {
            var list = new List<TResult>();

            items.ForEach<T>((item, ndx) =>
            {
                list.Add(converter(item, ndx));
            });

            return list;
        }


        /// <summary>
        /// json列表转换为对象集合(去除空记录)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> ToObjectNotNullOrEmpty<T>(this IList<string> list)
        {
            return ToJsonNotNullOrEmpty(list).ToJsonObject<List<T>>();
        }
        /// <summary>
        /// json列表 转换为 json字符串(去除空记录)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToJsonNotNullOrEmpty(this IList<string> list)
        {
            if (list == null || list.Count() == 0)
                return string.Empty;

            return $"[{list.Where(t => !string.IsNullOrEmpty(t)).ToArray().Join(",")}]";
        }
        /// <summary>
        /// json列表转换为对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> ToJson<T>(this IList<string> list)
        {
            return ToJson(list).ToJsonObject<List<T>>();
        }
        /// <summary>
        /// json列表 转换为 json字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToJson(this IList<string> list)
        {
            if (list == null || list.Count() == 0)
                return string.Empty;

            return $"[{list.ToArray().Join(",")}]";
        }

        /// <summary>
        /// 实体深拷贝，防止部分业务需要修改实体后 自动同步到所有实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyByReflect<T>(this T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;
            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopyByReflect(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }
        /// <summary>
        /// 去重
        /// <remarks>
        /// data.Distinct(p => p.Name, StringComparer.CurrentCultureIgnoreCase);
        /// </remarks>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector,
            IEqualityComparer<V> comparer = null)
        {
            if (comparer == null)
                return source.Distinct(Equality<T>.Create<V>(keySelector));
            else
                return source.Distinct(Equality<T>.Create<V>(keySelector, comparer));
        }

        public static string Join<T>(this IList<T> items, string delimeter)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return items[0].ToString();

            StringBuilder buffer = new StringBuilder();
            buffer.Append(items[0].ToString());

            for (int ndx = 1; ndx < items.Count; ndx++)
            {
                string append = items[ndx].ToString();
                buffer.Append(delimeter + append);
            }
            return buffer.ToString();
        }

        public static string JoinDelimited<T>(this IList<T> items, string delimeter, Func<T, string> appender)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return appender(items[0]);

            StringBuilder buffer = new StringBuilder();
            string val = appender == null ? items[0].ToString() : appender(items[0]);
            buffer.Append(val);

            for (int ndx = 1; ndx < items.Count; ndx++)
            {
                T item = items[ndx];
                val = appender == null ? item.ToString() : appender(item);
                buffer.Append(delimeter + val);
            }
            return buffer.ToString();
        }


        public static string JoinDelimitedWithNewLine<T>(this IList<T> items, string delimeter, int newLineAfterCount, string newLineText, Func<T, string> appender)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return appender(items[0]);

            StringBuilder buffer = new StringBuilder();
            buffer.Append(appender(items[0]));

            for (int ndx = 1; ndx < items.Count; ndx++)
            {
                T item = items[ndx];
                string append = appender(item);
                if (ndx % newLineAfterCount == 0)
                    buffer.Append(newLineText);

                buffer.Append(delimeter + append);
            }
            return buffer.ToString();
        }

        public static string AsDelimited<T>(this IEnumerable<T> items, string delimiter)
        {
            List<string> itemList = new List<string>();
            foreach (T item in items)
            {
                itemList.Add(item.ToString());
            }
            return String.Join(delimiter, itemList.ToArray());
        }


        #region Conditional Checks

        public static Boolean IsEmpty<T>(this IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            var isEmpty = !items.GetEnumerator().MoveNext();

            try
            {
                items.GetEnumerator().Reset();
            }
            catch (NotSupportedException notSupportedException)
            {
            }

            return isEmpty;
        }

        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return items == null || items.IsEmpty();
        }

        public static bool HasAnyNulls<T>(this IEnumerable<T> items)
        {
            return IsTrueForAny<T>(items, t => t == null);
        }

        public static bool IsTrueForAny<T>(this IEnumerable<T> items, Func<T, bool> executor)
        {

            foreach (T item in items)
            {
                bool result = executor(item);
                if (result)
                    return true;
            }
            return false;
        }

        public static bool IsTrueForAll<T>(this IEnumerable<T> items, Func<T, bool> executor)
        {
            foreach (T item in items)
            {
                bool result = executor(item);
                if (!result)
                    return false;
            }
            return true;
        }

        public static IDictionary<T, T> ToDictionary<T>(this IList<T> items)
        {
            IDictionary<T, T> dict = new Dictionary<T, T>();
            foreach (T item in items)
            {
                dict[item] = item;
            }
            return dict;
        }
        #endregion
    }
}