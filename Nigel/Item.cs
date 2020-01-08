﻿using System;
using Newtonsoft.Json;

namespace Nigel
{
    /// <summary>
    /// 列表项
    /// </summary>
    public class Item : IComparable<Item>
    {
        /// <summary>
        /// 初始化一个<see cref="Item"/>类型的实例
        /// </summary>
        public Item() { }

        /// <summary>
        /// 初始化一个<see cref="Item"/>类型的实例
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="value">值</param>
        /// <param name="sortId">排序号</param>
        /// <param name="group">组</param>
        /// <param name="disabled">禁用</param>
        public Item(string text, object value, int? sortId = null, string group = null, bool? disabled = null)
        {
            Text = text;
            Value = value;
            SortId = sortId;
            Group = group;
            Disabled = disabled;
        }

        /// <summary>
        /// 文本
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Value { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? SortId { get; set; }

        /// <summary>
        /// 组
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Group { get; set; }

        /// <summary>
        /// 禁用
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? Disabled { get; set; }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other">其他列表项</param>
        public int CompareTo(Item other) => string.Compare(Text, other.Text, StringComparison.CurrentCulture);
    }
}
