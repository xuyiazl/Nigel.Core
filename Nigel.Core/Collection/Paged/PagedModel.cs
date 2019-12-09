using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.Collection
{

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
}
