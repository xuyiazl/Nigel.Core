namespace Nigel.Core.Messages
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 21:21:46
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 消息列表 -- 键值对，列表
    /// </summary>
    public class Messages : IMessages
    {
        protected IDictionary<string, string> _messageMap;

        protected IList<string> _messageList;


        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="error">error</param>
        public void Add(string key, string error)
        {
            if (_messageMap == null) _messageMap = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(key))
                Add(error);
            else
                _messageMap[key] = error;
        }

        public void Add(string key, string format, params object[] args)
        {
            Add(key, string.Format(format, args));
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="error">error</param>
        public void Add(string error)
        {
            if (_messageList == null) _messageList = new List<string>();
            _messageList.Add(error);
        }

        public void Add(string format, params object[] args)
        {
            Add(string.Format(format, args));
        }

        /// <summary>
        /// 遍历所有键值对消息
        /// </summary>
        /// <param name="callback">callback</param>
        public void Each(Action<string, string> callback)
        {
            if (_messageMap == null) return;

            foreach (KeyValuePair<string, string> pair in _messageMap)
            {
                callback(pair.Key, pair.Value);
            }
        }


        /// <summary>
        /// 遍历所有消息
        /// </summary>
        /// <param name="callback">callback</param>
        public void EachFull(Action<string> callback)
        {
            if (_messageMap != null)
            {
                foreach (KeyValuePair<string, string> pair in _messageMap)
                {
                    string combined = pair.Key + " " + pair.Value;
                    callback(combined);
                }
            }

            if (_messageList != null)
            {
                foreach (string error in _messageList)
                    callback(error);
            }
        }


        /// <summary>
        /// 返回一个以换行符隔开的所有消息
        /// </summary>
        /// <returns></returns>
        public string Message()
        {
            return Message(Environment.NewLine);
        }


        /// <summary>
        /// 返回一个以分隔符隔开的所有消息
        /// </summary>
        /// <returns></returns>
        public string Message(string separator)
        {
            StringBuilder buffer = new StringBuilder();
            if (_messageList != null)
            {
                foreach (string error in _messageList)
                    buffer.Append(error + separator);
            }

            if (_messageMap != null)
            {
                foreach (KeyValuePair<string, string> pair in _messageMap)
                {
                    string combined = pair.Key + " " + pair.Value;
                    buffer.Append(combined + separator);
                }
            }

            return buffer.ToString();
        }


        /// <summary>
        /// 获取消息总数
        /// </summary>
        public int Count
        {
            get
            {
                int errorCount = 0;
                if (_messageList != null) errorCount += _messageList.Count;
                if (_messageMap != null) errorCount += _messageMap.Count;

                return errorCount;
            }
        }


        /// <summary>
        /// 是否有错误消息
        /// </summary>
        /// <value><c>true</c> 如果该实例有错误消息,反之 <c>false</c>.</value>
        public bool HasAny
        {
            get { return Count > 0; }
        }


        /// <summary>
        /// 清除所有消息
        /// </summary>
        public void Clear()
        {
            if (_messageMap != null) _messageMap.Clear();
            if (_messageList != null) _messageList.Clear();
        }


        /// <summary>
        /// 根据键值获取消息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public string On(string key)
        {
            if (_messageMap != null && _messageMap.ContainsKey(key))
                return _messageMap[key];

            return string.Empty;
        }


        /// <summary>
        /// 获取所有消息
        /// </summary>
        /// <returns></returns>
        public IList<string> On()
        {
            return _messageList ?? null;
        }


        /// <summary>
        /// 获取和写入消息列表
        /// </summary>        
        public IList<string> MessageList
        {
            get { return _messageList; }
            set { _messageList = value; }
        }


        /// <summary>
        /// 获取或写入键值对消息列表
        /// </summary>
        public IDictionary<string, string> MessageMap
        {
            get { return _messageMap; }
            set { _messageMap = value; }
        }


        /// <summary>
        /// 复制消息
        /// </summary>
        public void CopyTo(IMessages messages)
        {
            if (messages == null) return;

            if (_messageList != null)
                foreach (string error in _messageList)
                    messages.Add(error);

            if (_messageMap != null)
                foreach (KeyValuePair<string, string> pair in _messageMap)
                    messages.Add(pair.Key, pair.Value);
        }
    }
}
