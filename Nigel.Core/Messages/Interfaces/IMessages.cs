namespace Nigel.Core.Messages
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 21:13:19
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 通过列表和存储键/值 存储消息
    /// </summary>
    public interface IMessages
    {
        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="message"></param>
        void Add(string message);

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void Add(string format, params object[] arguments);

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <remarks>键值对</remarks>
        /// <param name="key"></param>
        /// <param name="message"></param>
        void Add(string key, string message);

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="key">键值对</param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        void Add(string key, string format, params object[] arguments);

        /// <summary>
        /// 清除所有消息
        /// </summary>
        void Clear();


        /// <summary>
        /// 复制所有消息
        /// </summary>
        /// <param name="messages"></param>
        void CopyTo(IMessages messages);


        /// <summary>
        /// 消息总数
        /// </summary>
        int Count { get; }


        /// <summary>
        /// 遍历所有键值对消息
        /// </summary>
        /// <param name="callback"></param>
        void Each(Action<string, string> callback);


        /// <summary>
        /// 遍历所有消息
        /// </summary>
        /// <param name="callback"></param>
        void EachFull(Action<string> callback);


        /// <summary>
        /// 返回一个以换行符隔开的所有消息
        /// </summary>
        /// <returns></returns>
        string Message();


        /// <summary>
        /// 返回一个以分隔符隔开的所有消息
        /// </summary>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        string Message(string separator);


        /// <summary>
        /// 是否有消息
        /// </summary>
        bool HasAny { get; }


        /// <summary>
        /// 获取指定键的消息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string On(string key);


        /// <summary>
        /// 获取所有消息
        /// </summary>
        /// <returns></returns>
        IList<string> On();


        /// <summary>
        /// 获取所有消息
        /// </summary>
        IList<string> MessageList { get; set; }


        /// <summary>
        /// 获取所有键值对消息
        /// </summary>
        IDictionary<string, string> MessageMap { get; set; }
    }
}
