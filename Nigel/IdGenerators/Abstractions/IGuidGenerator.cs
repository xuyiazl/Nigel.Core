﻿using System;

namespace Nigel.IdGenerators.Abstractions
{
    /// <summary>
    /// Guid ID 生成器
    /// </summary>
    public interface IGuidGenerator:IIdGenerator<Guid>
    {
    }
}