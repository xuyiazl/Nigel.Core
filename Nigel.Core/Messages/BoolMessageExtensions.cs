namespace Nigel.Core.Messages
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/30 16:42:14
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class BoolMessageExtensions
    {
        /// <summary>
        /// 转换 0 or 1
        /// </summary>
        /// <remarks>True:1   False:0</remarks>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int AsExitCode(this BoolMessage result)
        {
            return result.Success ? 1 : 0;
        }
    }
}
