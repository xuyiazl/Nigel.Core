using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Nigel.Core.Helper
{
    /// <summary>
    /// BinaryFormatterUtils 序列化，反序列化消息的帮助类
    /// </summary>
    public static class BinaryFormatterHelper
    {
        /// <summary>
        /// BinaryFormatter 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToBinary(this object obj)
        {
            return Serialize(obj);
        }

        public static byte[] Serialize(object obj)
        {
            BinaryFormatter binaryF = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(1024 * 10);
            try
            {
                binaryF.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
            finally
            {
                ms.Close();
            }
        }
        /// <summary>
        /// BinaryFormatter 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T ToObject<T>(this byte[] buffer)
        {
            return Deserialize<T>(buffer);
        }

        public static T Deserialize<T>(byte[] buffer)
        {
            BinaryFormatter binaryF = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer, 0, buffer.Length, false);
            try
            {
                object obj = binaryF.Deserialize(ms);
                return (T)obj;
            }
            finally
            {
                ms.Close();
            }
        }
    }
}
