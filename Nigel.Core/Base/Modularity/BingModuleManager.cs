using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Nigel.Core.Builders;
using Nigel.Extensions;

namespace Nigel.Core.Modularity
{
    /// <summary>
    /// Bing 模块管理器
    /// </summary>
    public class BingModuleManager : IBingModuleManager
    {
        /// <summary>
        /// 所有模块信息集合
        /// </summary>
        private readonly List<BingModule> _sourceModules;

        /// <summary>
        /// 自动检索到的所有模块信息
        /// </summary>
        public virtual IEnumerable<BingModule> SourceModules => _sourceModules;

        /// <summary>
        /// 最终加载的模块信息集合
        /// </summary>
        public virtual IEnumerable<BingModule> LoadedModules { get; private set; }

        /// <summary>
        /// 初始化一个<see cref="BingModuleManager"/>类型的实例
        /// </summary>
        public BingModuleManager()
        {
            _sourceModules = new List<BingModule>();
            LoadedModules = new List<BingModule>();
        }

        /// <summary>
        /// 加载模块服务
        /// </summary>
        /// <param name="services">服务集合</param>
        public virtual IServiceCollection LoadModules(IServiceCollection services)
        {
            var moduleTypeFinder =
                services.GetOrAddTypeFinder<IBingModuleTypeFinder>(assemblyFinder =>
                    new BingModuleTypeFinder(assemblyFinder));
            var moduleTypes = moduleTypeFinder.FindAll();
            _sourceModules.Clear();
            _sourceModules.AddRange(moduleTypes.Select(x => (BingModule)Activator.CreateInstance(x)));
            var builder = services.GetSingletonInstance<IBingBuilder>();
            List<BingModule> modules;
            if (builder.AddModules.Any())
            {
                modules = _sourceModules.Where(x => x.Level == ModuleLevel.Core)
                    .Union(_sourceModules.Where(x => builder.AddModules.Contains(x.GetType())))
                    .Distinct()
                    .OrderBy(x => x.Level)
                    .ThenBy(x => x.Order)
                    .ToList();
                var dependModules = new List<BingModule>();
                foreach (var module in modules)
                {
                    var dependModuleTypes = module.GetDependModuleTypes();
                    foreach (var dependModuleType in dependModuleTypes)
                    {
                        var dependModule = _sourceModules.Find(x => x.GetType() == dependModuleType);
                        if (dependModule == null)
                            throw new Exception($"加载模块{module.GetType().FullName}时无法找到依赖模块{dependModuleType.FullName}");
                        dependModules.AddIfNotContains(dependModule);
                    }
                }

                modules = modules.Union(dependModules).Distinct().ToList();
            }
            else
            {
                modules = _sourceModules.ToList();
                modules.RemoveAll(x => builder.ExceptModules.Contains(x.GetType()));
            }

            // 按先层级后顺序的规则进行排序
            modules = modules.OrderBy(x => x.Level).ThenBy(x => x.Order).ToList();
            LoadedModules = modules;
            foreach (var module in LoadedModules)
                services = module.AddServices(services);
            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        public virtual void UseModule(IServiceProvider provider)
        {
            foreach (var module in LoadedModules)
                module.UseModule(provider);
        }
    }
}
