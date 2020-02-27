using System;
using System.Collections.Generic;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Nigel.Extensions
{
    /// <summary>
    /// 系统扩展 - 反射
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 获取实例上的属性值
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <param name="instance">成员所在的类实例</param>
        /// <returns></returns>
        public static object GetPropertyValue(this MemberInfo member, object instance)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return instance.GetType().GetProperty(member.Name)?.GetValue(instance);
        }

        /// <summary>
        /// 实体深拷贝，防止部分业务需要修改实体后 自动同步到所有实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyByReflect<T>(this T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;
            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopyByReflect(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }

        /// <summary>
        /// 反射复制相同属性值
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="Target">目标类型</typeparam>
        /// <param name="sources">源数据</param>
        /// <returns></returns>
        public static IList<Target> CopyByReflect<TSource, Target>(this IList<TSource> sources)
        {
            IList<Target> targetList = new List<Target>();

            if (sources == null || sources.Count == 0)
                return targetList;

            PropertyInfo[] _targetProperties = typeof(Target).GetProperties();
            PropertyInfo[] _sourceProperties = typeof(TSource).GetProperties();

            foreach (var source in sources)
            {
                Target model = Activator.CreateInstance<Target>();

                foreach (var _target in _targetProperties)
                {
                    foreach (var _source in _sourceProperties)
                    {
                        if (_target.Name == _source.Name && _target.PropertyType == _source.PropertyType)
                        {
                            _target.SetValue(model, _source.GetValue(source, null), null);
                            break;
                        }
                    }
                }

                targetList.Add(model);
            }

            return targetList;
        }

        /// <summary>
        /// 反射复制相同属性值
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="Target">目标类型</typeparam>
        /// <param name="source">源数据</param>
        /// <returns></returns>
        public static Target CopyByReflect<TSource, Target>(this TSource source)
        {
            Target model = default(Target);
            if (source == null) return model;

            PropertyInfo[] _targetProperties = typeof(Target).GetProperties();
            PropertyInfo[] _sourceProperties = typeof(TSource).GetProperties();

            model = Activator.CreateInstance<Target>();

            foreach (var _target in _targetProperties)
            {
                foreach (var _source in _sourceProperties)
                {
                    if (_target.Name == _source.Name && _target.PropertyType == _source.PropertyType)
                    {
                        _target.SetValue(model, _source.GetValue(source, null), null);
                        break;
                    }
                }
            }
            return model;
        }
    }
}