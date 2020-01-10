﻿using System;

namespace Nigel.Core.Jwt
{
    /// <summary>
    /// Provider for UTC DateTime.
    /// </summary>
    public sealed class UtcDateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// Retuns the current time (UTC).
        /// </summary>
        /// <returns></returns>
        public DateTimeOffset GetNow()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}