using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Data.Collection.Paged
{

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
