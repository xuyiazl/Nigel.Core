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

    using Nigel.Core.Paging;
    using System.Linq.Expressions;

    [Serializable]
    public class PagedList<T> : List<T>
    {
        public readonly static PagedList<T> Empty = new PagedList<T>(1, 1, 0, null);

        public int PageSize { get; private set; }
        public int PageNumber { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public PagedList(int pageNumber, int pageSize, int totalRecords, IList<T> items)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalRecords;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            if (items != null && items.Count > 0)
            {
                this.AddRange(items);
            }
        }

        public string ToPageHtml(Func<int, string> urlBuilder)
        {
            return ToPageHtml(7, "current", string.Empty, true, PagerLanguage.Default, urlBuilder);
        }

        public string ToPageHtml(bool showFirstAndLastPage, Func<int, string> urlBuilder)
        {
            return ToPageHtml(7, "current", string.Empty, showFirstAndLastPage, PagerLanguage.Default, urlBuilder);
        }

        public string ToPageHtml(int numberPagesToDisplay, bool showFirstAndLastPage, PagerLanguage language, Func<int, string> urlBuilder)
        {
            return ToPageHtml(numberPagesToDisplay, "current", string.Empty, showFirstAndLastPage, language, urlBuilder);
        }

        public string ToPageHtml(int numberPagesToDisplay,
            string cssClassForCurrentPage, string cssClassForPage, bool showFirstAndLastPage,
            PagerLanguage language, Func<int, string> urlBuilder)
        {
            Pager pager = new Pager(PageNumber, TotalPages,
                new PagerSettings(numberPagesToDisplay, cssClassForCurrentPage,
                    cssClassForPage, showFirstAndLastPage, language));
            return pager.ToHtml(urlBuilder);
        }
    }

    [Serializable]
    public class PagedList1<T> : List<T>
    {
        public readonly static PagedList1<T> Empty = new PagedList1<T>(1, 1, 0, null);

        public int Limit { get; private set; }
        public int Offset { get; private set; }
        public int TotalCount { get; private set; }

        public PagedList1(int limit, int offset, int totalRecords, IList<T> items)
        {
            Limit = limit;
            Offset = offset;
            TotalCount = totalRecords;
            if (items != null && items.Count > 0)
            {
                this.AddRange(items);
            }
        }
    }

    [Serializable]
    public class Paged<T>
    {
        public readonly static Paged<T> Empty = new Paged<T>(1, 1, 0, default(T));

        public int PageSize { get; private set; }
        public int PageNumber { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public T Items { get; private set; }

        public Paged(int pageNumber, int pageSize, int totalRecords, T items)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalRecords;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            if (items != null)
            {
                Items = items;
            }
        }

        public string ToPageHtml(Func<int, string> urlBuilder)
        {
            return ToPageHtml(7, "current", string.Empty, true, PagerLanguage.Default, urlBuilder);
        }
        public string ToPageHtml(bool showFirstAndLastPage, Func<int, string> urlBuilder)
        {
            return ToPageHtml(7, "current", string.Empty, showFirstAndLastPage, PagerLanguage.Default, urlBuilder);
        }

        public string ToPageHtml(int numberPagesToDisplay, bool showFirstAndLastPage, PagerLanguage language, Func<int, string> urlBuilder)
        {
            return ToPageHtml(numberPagesToDisplay, "current", string.Empty, showFirstAndLastPage, language, urlBuilder);
        }

        public string ToPageHtml(int numberPagesToDisplay,
            string cssClassForCurrentPage, string cssClassForPage, bool showFirstAndLastPage,
            PagerLanguage language, Func<int, string> urlBuilder)
        {
            Pager pager = new Pager(PageNumber, TotalPages,
                new PagerSettings(numberPagesToDisplay, cssClassForCurrentPage, cssClassForPage, showFirstAndLastPage, language));
            return pager.ToHtml(urlBuilder);
        }
    }
}
