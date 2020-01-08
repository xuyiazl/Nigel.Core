namespace Nigel.Core.Comparer
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 10:04:38
    *                   mailto:3624091@qq.com
    *                         
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;

    /// <summary>
    /// Generic equality comparer
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        #region Functions

        public bool Equals(T x, T y)
        {
            if (!typeof(T).IsValueType
                || (typeof(T).IsGenericType
                && typeof(T).GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>))))
            {
                if (Object.Equals(x, default(T)))
                    return Object.Equals(y, default(T));
                if (Object.Equals(y, default(T)))
                    return false;
            }
            if (x.GetType() != y.GetType())
                return false;
            if (x is IEnumerable && y is IEnumerable)
            {
                GenericEqualityComparer<object> Comparer = new GenericEqualityComparer<object>();
                IEnumerator XEnumerator = ((IEnumerable)x).GetEnumerator();
                IEnumerator YEnumerator = ((IEnumerable)y).GetEnumerator();
                while (true)
                {
                    bool XFinished = !XEnumerator.MoveNext();
                    bool YFinished = !YEnumerator.MoveNext();
                    if (XFinished || YFinished)
                        return XFinished & YFinished;
                    if (!Comparer.Equals(XEnumerator.Current, YEnumerator.Current))
                        return false;
                }
            }
            if (x is IEquatable<T>)
                return ((IEquatable<T>)x).Equals(y);
            if (x is IComparable<T>)
                return ((IComparable<T>)x).CompareTo(y) == 0;
            if (x is IComparable)
                return ((IComparable)x).CompareTo(y) == 0;
            return x.Equals(y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}
