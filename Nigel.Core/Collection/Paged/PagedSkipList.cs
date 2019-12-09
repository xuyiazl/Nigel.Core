﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Core.Collection
{

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedSkipList<T> : List<T>
    {
        /// <summary>
        /// 初始化空对象
        /// </summary>
        public readonly static PagedSkipList<T> Empty = new PagedSkipList<T>(default(List<T>), 0, 1, 1);
        /// <summary>
        /// 获取记录数
        /// </summary>
        public int Limit { get; private set; }
        /// <summary>
        /// 偏移量
        /// </summary>
        public int Offset { get; private set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords { get; private set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items"></param>
        /// <param name="totalRecords"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        public PagedSkipList(IList<T> items, int totalRecords, int limit, int offset)
        {
            Limit = limit;
            Offset = offset;
            TotalRecords = totalRecords;
            if (items != null && items.Count > 0)
            {
                this.AddRange(items);
            }
        }
        /// <summary>
        /// 创建分页对象
        /// </summary>
        /// <param name="source"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static PagedSkipList<T> Create(IQueryable<T> source, int limit, int offset)
        {
            var count = source.Count();
            var items = source.Skip(offset).Take(limit).ToList();
            return new PagedSkipList<T>(items, limit, offset, count);
        }
        /// <summary>
        /// 异步创建分页对象
        /// </summary>
        /// <param name="source"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static async Task<PagedSkipList<T>> CreateAsync(IQueryable<T> source, int limit, int offset)
        {
            var count = await source.CountAsync();
            var items = await source.Skip(offset).Take(limit).ToListAsync();
            return new PagedSkipList<T>(items, limit, offset, count);
        }
        /// <summary>
        /// 异步创建分页对象
        /// </summary>
        /// <param name="source"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<PagedSkipList<T>> CreateAsync(IQueryable<T> source, int limit, int offset,
            CancellationToken cancellationToken = default)
        {
            var count = await source.CountAsync(cancellationToken);
            var items = await source.Skip(offset).Take(limit).ToListAsync(cancellationToken);
            return new PagedSkipList<T>(items, limit, offset, count);
        }
    }
}
