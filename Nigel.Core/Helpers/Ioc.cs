using System;
using System.Collections.Generic;
using System.Linq;
using Nigel.Core.Dependency;

namespace Nigel.Core.Helpers
{
    /// <summary>
    /// 容器操作
    /// </summary>
    public static class Ioc
    {
        /// <summary>
        /// 创建集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        public static List<T> CreateList<T>() => ServiceLocator.Instance.GetServices<T>().ToList();

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        public static T Create<T>() => ServiceLocator.Instance.GetService<T>();

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="type">对象类型</param>
        public static object Create(Type type) => ServiceLocator.Instance.GetService(type);
    }
}
