using System;
using System.Collections.Generic;

namespace Nigel.Paging
{
    [Serializable]
    public class PagedSkipModel<T>
    {
        public int Limit { get; private set; }
        public int Skip { get; private set; }
        public int TotalRecords { get; private set; }

        public IList<T> Items { get; set; }

        public PagedSkipModel(IList<T> items, int totalRecords, int skip, int limit)
        {
            Limit = limit;
            Skip = skip;
            TotalRecords = totalRecords;

            if (items != null && items.Count > 0)
                Items = items;
            else
                Items = new List<T>();
        }
    }
}