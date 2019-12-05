namespace Nigel.Core
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 12:43:15
    *                   mailto:3624091@qq.com
    *                         
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class RMB
    {
        /// <summary>
        /// 把阿拉伯数字的金额转换为中文大写数字
        ///<example>Console.WriteLine("{0,14:N2}: {1}", x, ConvertToChinese(x));</example>        
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ToChinese(this double x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", delegate(Match m) { return "负元 零壹贰叁肆伍陆柒捌玖       分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString(); });
        }
        /// <summary>
        /// 把阿拉伯数字的金额转换为中文大写数字
        /// </summary>
        ///<example>Console.WriteLine("{0,14:N2}: {1}", x, ConvertToChinese(x));</example>  
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ToChinese(this string x)
        {
            double money = x.Convert<double>();
            return ToChinese(money);
        }
    }
}
