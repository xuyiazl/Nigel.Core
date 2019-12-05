namespace Nigel.Core.ValidationSupport
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 21:48:39
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IValidatorWithRules : IValidator
    {
        void Add(Func<ValidationEvent, bool> rule);

        void Add(string ruleName, Func<ValidationEvent, bool> rule);

        void RemoveAt(int ndx);

        void Remove(string ruleNname);

        int Count { get; }

        Func<ValidationEvent, bool> this[int ndx] { get; }
    }
}
