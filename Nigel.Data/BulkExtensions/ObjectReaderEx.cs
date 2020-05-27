﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FastMember;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Nigel.Data.BulkExtensions
{
    internal class ObjectReaderEx : ObjectReader // Overridden to fix ShadowProperties in FastMember library
    {
        private readonly HashSet<string> shadowProperties;
        private readonly Dictionary<string, ValueConverter> convertibleProperties;
        private readonly DbContext context;
        private readonly string[] members;
        private readonly FieldInfo current;
        private readonly IEnumerable<IProperty> allProperties;

        public ObjectReaderEx(Type type, IEnumerable source, HashSet<string> shadowProperties, Dictionary<string, ValueConverter> convertibleProperties, DbContext context, params string[] members) : base(type, source, members)
        {
            this.shadowProperties = shadowProperties;
            this.convertibleProperties = convertibleProperties;
            this.context = context;
            this.members = members;

            if (type.IsAbstract)
            {
                allProperties = context.Model.FindEntityType(type)
                    .GetDerivedTypes()
                    .SelectMany(m => m.GetProperties())
                    .Distinct();
            }

            current = typeof(ObjectReader).GetField("current", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static ObjectReader Create<T>(IEnumerable<T> source, HashSet<string> shadowProperties, Dictionary<string, ValueConverter> convertibleProperties, DbContext context, params string[] members)
        {
            bool hasShadowProp = shadowProperties.Count > 0;
            bool hasConvertibleProperties = convertibleProperties.Keys.Count > 0;
            bool isAbstractType = typeof(T).IsAbstract;
            return (hasShadowProp || hasConvertibleProperties || isAbstractType) ? new ObjectReaderEx(typeof(T), source, shadowProperties, convertibleProperties, context, members) : Create(source, members);
        }

        public override object this[string name]
        {
            get
            {
                if (shadowProperties.Contains(name))
                {
                    var current = this.current.GetValue(this);
                    return context.Entry(current).Property(name).CurrentValue;
                }
                else if (convertibleProperties.TryGetValue(name, out var converter))
                {
                    var current = this.current.GetValue(this);
                    var currentValue = context.Entry(current).Property(name).CurrentValue;
                    return converter.ConvertToProvider(currentValue);
                }
                else if (allProperties != null)
                {
                    var match = allProperties.SingleOrDefault(m => m.Name == name);
                    if (match != null)
                    {
                        var current = this.current.GetValue(this);
                        var entry = context.Entry(current);

                        if (entry.Properties.Any(m => m.Metadata.Name == name))
                        {
                            return entry.Property(name).CurrentValue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return base[name];
            }
        }

        public override object this[int i]
        {
            get
            {
                var name = members[i];
                return this[name];
            }
        }
    }
}
