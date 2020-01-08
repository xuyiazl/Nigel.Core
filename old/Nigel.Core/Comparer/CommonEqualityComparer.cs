namespace Nigel.Core.Comparer
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 12:50:50
    *                   mailto:3624091@qq.com
    *                         
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    //参照 http://www.cnblogs.com/ldp615/archive/2011/08/02/quickly-create-instance-of-iequalitycomparer-and-icomparer.html

    public static class Equality<T>
    {
        public static IEqualityComparer<T> Create<V>(Func<T, V> keySelector)
        {
            return new CommonEqualityComparer<T, V>(keySelector);
        }

        public static IEqualityComparer<T> Create<V>(Func<T, V> keySelector, IEqualityComparer<V> comparer)
        {
            return new CommonEqualityComparer<T, V>(keySelector, comparer);
        }

        class CommonEqualityComparer<T, V> : IEqualityComparer<T>
        {
            private Func<T, V> keySelector;
            private IEqualityComparer<V> comparer;

            public CommonEqualityComparer(Func<T, V> keySelector, IEqualityComparer<V> comparer)
            {
                this.keySelector = keySelector;
                this.comparer = comparer;
            }

            public CommonEqualityComparer(Func<T, V> keySelector)
                : this(keySelector, EqualityComparer<V>.Default)
            { }

            public bool Equals(T x, T y)
            {
                return comparer.Equals(keySelector(x), keySelector(y));
            }

            public int GetHashCode(T obj)
            {
                return comparer.GetHashCode(keySelector(obj));
            }
        }
    }

    public static class Comparisons<T>
    {
        public static IComparer<T> Create<V>(Func<T, V> keySelector)
        {
            return new CommonComparer<V>(keySelector);
        }
        public static IComparer<T> Create<V>(Func<T, V> keySelector, IComparer<V> comparer)
        {
            return new CommonComparer<V>(keySelector, comparer);
        }

        class CommonComparer<V> : IComparer<T>
        {
            private Func<T, V> keySelector;
            private IComparer<V> comparer;

            public CommonComparer(Func<T, V> keySelector, IComparer<V> comparer)
            {
                this.keySelector = keySelector;
                this.comparer = comparer;
            }
            public CommonComparer(Func<T, V> keySelector)
                : this(keySelector, Comparer<V>.Default)
            { }

            public int Compare(T x, T y)
            {
                return comparer.Compare(keySelector(x), keySelector(y));
            }
        }
    }
}
