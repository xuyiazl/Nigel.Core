﻿using Microsoft.AspNetCore.Authentication;
using System;

namespace Nigel.Core.Jwt.Internal
{
    internal sealed class SystemClockDatetimeProvider : IDateTimeProvider
    {
        private readonly ISystemClock _clock;

        public SystemClockDatetimeProvider(ISystemClock clock) =>
            _clock = clock;

        public DateTimeOffset GetNow() =>
            _clock.UtcNow;
    }
}