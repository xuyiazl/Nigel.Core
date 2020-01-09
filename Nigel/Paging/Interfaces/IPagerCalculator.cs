﻿namespace Nigel.Paging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/12 12:44:54
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPagerCalculator
    {
        /// <summary>
        /// Calculate pages
        /// </summary>
        /// <param name="pagerData"></param>
        /// <param name="settings"></param>
        void Calculate(Pager pager, PagerSettings settings);
    }
}
