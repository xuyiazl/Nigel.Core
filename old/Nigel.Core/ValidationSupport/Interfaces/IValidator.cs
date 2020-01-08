namespace Nigel.Core.ValidationSupport
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 21:48:22
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IValidator : IValidatorStateful, IValidatorNonStateful
    {
    }
}
