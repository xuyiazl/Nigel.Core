namespace Nigel.Core.ValidationSupport
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 21:49:23
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Nigel.Core.Messages;

    public class ValidationResults : Errors, IValidationResults
    {
        public static readonly ValidationResults Empty = new ValidationResults();

        public bool IsValid
        {
            get { return base.Count == 0; }
        }
    }
}
