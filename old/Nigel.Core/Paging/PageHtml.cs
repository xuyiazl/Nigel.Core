using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.Paging
{
    /// <summary>
    /// page分页html
    /// </summary>
    public static class PageHtml
    {
        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string Build(int pageNumber, int totalPages, Func<int, string> urlBuilder)
        {
            return Build(pageNumber, totalPages, 7, "current", string.Empty, true, PagerLanguage.Default, urlBuilder);
        }
        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string Build(int pageNumber, int totalPages, bool showFirstAndLastPage, Func<int, string> urlBuilder)
        {
            return Build(pageNumber, totalPages, 7, "current", string.Empty, showFirstAndLastPage, PagerLanguage.Default, urlBuilder);
        }
        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="numberPagesToDisplay">显示几页</param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="language">语言</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string Build(int pageNumber, int totalPages, int numberPagesToDisplay, bool showFirstAndLastPage, PagerLanguage language, Func<int, string> urlBuilder)
        {
            return Build(pageNumber, totalPages, numberPagesToDisplay, "current", string.Empty, showFirstAndLastPage, language, urlBuilder);
        }
        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="numberPagesToDisplay">显示几页</param>
        /// <param name="cssClassForCurrentPage">显示当前页的css（current）</param>
        /// <param name="cssClassForPage">显示费当前页的css</param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="language">语言</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string Build(int pageNumber, int totalPages, int numberPagesToDisplay,
            string cssClassForCurrentPage, string cssClassForPage, bool showFirstAndLastPage,
            PagerLanguage language, Func<int, string> urlBuilder)
        {
            Pager pager = new Pager(pageNumber, totalPages,
                new PagerSettings(numberPagesToDisplay, cssClassForCurrentPage,
                    cssClassForPage, showFirstAndLastPage, language));
            return pager.ToHtml(urlBuilder);
        }
    }
}
