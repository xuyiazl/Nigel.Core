namespace Nigel.Core
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 12:35:40
    *                   mailto:3624091@qq.com
    *                         
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    using Nigel.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;


    public class PagedList<T> : List<T>
    {
        public readonly static PagedList<T> Empty = new PagedList<T>(default(List<T>), 0, 1, 1);
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedList(List<T> items, int totalRecords, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            if (items != null && items.Count > 0)
            {
                this.AddRange(items);
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }

        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public PagedModel<TOut> ToMap<TOut>(Func<T, TOut> converter)
        {
            return new PagedModel<TOut>(this.ForEach(converter), TotalRecords, TotalPages, PageNumber, PageSize);
        }

        public PagedModel<TOut> ToMap<TOut>(Func<T, int, TOut> converter)
        {
            return new PagedModel<TOut>(this.ForEach(converter), TotalRecords, TotalPages, PageNumber, PageSize);
        }
    }


    [Serializable]
    public class PagedModel<T>
    {
        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public IList<T> Items { get; set; }

        public PagedModel(IList<T> items, int totalRecords, int totalPages, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = totalPages;
            if (items != null)
            {
                Items = items;
            }
        }
    }

    public class PagedSkipList<T> : List<T>
    {
        public readonly static PagedSkipList<T> Empty = new PagedSkipList<T>(default(List<T>), 0, 1, 1);

        public int Limit { get; private set; }
        public int Offset { get; private set; }
        public int TotalRecords { get; private set; }

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

        public static PagedSkipList<T> Create(IQueryable<T> source, int limit, int offset)
        {
            var count = source.Count();
            var items = source.Skip(limit).Take(offset).ToList();
            return new PagedSkipList<T>(items, limit, offset, count);
        }

        public static async Task<PagedSkipList<T>> CreateAsync(IQueryable<T> source, int limit, int offset)
        {
            var count = await source.CountAsync();
            var items = await source.Skip(limit).Take(offset).ToListAsync();
            return new PagedSkipList<T>(items, limit, offset, count);
        }

        public PagedSkipModel<TOut> ToMap<TOut>(Func<T, TOut> converter)
        {
            return new PagedSkipModel<TOut>(this.ForEach(converter), TotalRecords, Limit, Offset);
        }

        public PagedSkipModel<TOut> ToMap<TOut>(Func<T, int, TOut> converter)
        {
            return new PagedSkipModel<TOut>(this.ForEach(converter), TotalRecords, Limit, Offset);
        }
    }


    [Serializable]
    public class PagedSkipModel<T>
    {
        public int Limit { get; private set; }
        public int Offset { get; private set; }
        public int TotalRecords { get; private set; }

        public IList<T> Items { get; set; }

        public PagedSkipModel(IList<T> items, int totalRecords, int limit, int offset)
        {
            Limit = limit;
            Offset = offset;
            TotalRecords = totalRecords;
            if (items != null)
            {
                Items = items;
            }
        }
    }

}
