﻿using Nigel.Core.Modularity;

namespace Nigel.Core.Builders
{
    /// <summary>
    /// Bing构建器(<see cref="IBingBuilder"/>) 扩展
    /// </summary>
    public static class BingBuilderExtensions
    {
        /// <summary>
        /// 添加核心模块
        /// </summary>
        /// <param name="builder">Bing构建器</param>
        public static IBingBuilder AddCoreModule(this IBingBuilder builder) => builder.AddModule<BingCoreModule>();
    }
}
