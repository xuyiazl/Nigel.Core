using Nigel.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.Collection.Paged
{
    public static class PagedExtensions
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
    }
}
