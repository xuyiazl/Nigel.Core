using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.AccessControl
{
    /// <summary>
    /// view component access strategy
    /// </summary>
    public interface IControlAccessStrategy
    {
        /// <summary>
        /// view component access strategy
        /// </summary>
        bool IsControlCanAccess(string accessKey);
    }
}
