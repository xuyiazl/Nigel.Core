namespace Nigel.Core
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 12:38:29
    *                   mailto:3624091@qq.com
    *                         
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.IO.Compression;
    using System.Collections;

    public static class GZip
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] GZipCompress(this byte[] bytes)
        {
            return Compress(bytes);
        }
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] bytes)
        {
            using (MemoryStream mm = new MemoryStream(bytes))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Compress(mm, ms);
                    return ms.ToArray();
                }
            }
        }
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void GZipCompress(this Stream source, Stream dest)
        {
            Compress(source, dest);
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="source">源流</param>
        /// <param name="dest">压缩后的流</param>
        public static void Compress(Stream source, Stream dest)
        {
            using (GZipStream zipStream = new GZipStream(dest, CompressionMode.Compress, true))
            {
                byte[] buf = new byte[1024];
                int len;
                while ((len = source.Read(buf, 0, buf.Length)) > 0)
                {
                    zipStream.Write(buf, 0, len);
                }
            }
        }
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] GZipDecompress(this byte[] bytes)
        {
            return Decompress(bytes);
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] bytes)
        {
            using (MemoryStream mm = new MemoryStream(bytes))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Decompress(mm, ms);
                    return ms.ToArray();
                }
            }
        }
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void GZipDecompress(this Stream source, Stream dest)
        {
            Decompress(source, dest);
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="source">源流</param>
        /// <param name="dest">解压后的流</param>
        public static void Decompress(Stream source, Stream dest)
        {
            using (GZipStream zipStream = new GZipStream(source, CompressionMode.Decompress, true))
            {
                byte[] buf = new byte[1024];
                int len;
                while ((len = zipStream.Read(buf, 0, buf.Length)) > 0)
                {
                    dest.Write(buf, 0, len);
                }
            }
        }
    }
}
