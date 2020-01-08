namespace Nigel.Core.ValidationSupport
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 22:16:28
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Nigel.Core.Messages;

    public class ValidationUtils
    {
        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="isError">是否添加</param>
        /// <param name="errors">记录错误信息集合对象</param>
        /// <param name="message">错误信息</param>
        /// <returns></returns>
        public static bool Validate(bool isError, IList<string> errors, string message)
        {
            if (isError) { errors.Add(message); }
            return !isError;
        }


        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="isError">是否添加.</param>
        /// <param name="errors">记录错误信息对象</param>
        /// <param name="key">错误信息KEY</param>
        /// <param name="message">错误信息</param>
        /// <returns></returns>
        public static bool Validate(bool isError, IErrors errors, string key, string message)
        {
            if (isError)
            {
                errors.Add(key, message);
            }
            return !isError;
        }



        /// <summary>
        /// 添加错误信息。
        /// </summary>
        /// <param name="isError">是否添加</param>
        /// <param name="errors">记录错误信息对象</param>
        /// <param name="message">错误信息</param>
        /// <returns></returns>
        public static bool Validate(bool isError, IErrors errors, string message)
        {
            if (isError)
            {
                errors.Add(string.Empty, message);
            }
            return !isError;
        }


        /// <summary>
        /// 传输所有的消息
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="errors"></param>
        public static void TransferMessages(IList<string> messages, IErrors errors)
        {
            foreach (string message in messages)
            {
                errors.Add(string.Empty, message);
            }
        }


        /// <summary>
        /// 验证所有列表中的验证规则。
        /// </summary>
        /// <param name="validators">验证规则列表</param>
        /// <param name="destinationResults">验证结果</param>
        /// <returns>true(所有规则通过)/false</returns>
        public static bool Validate(IList<IValidator> validators, IValidationResults destinationResults)
        {
            if (validators == null || validators.Count == 0)
                return true;

            int initialErrorCount = destinationResults.Count;

            foreach (IValidator validator in validators)
            {
                validator.Validate(destinationResults);
            }

            return initialErrorCount == destinationResults.Count;
        }


        /// <summary>
        /// 验证规则，并返回一个boolMessage
        /// </summary>
        /// <param name="validator"></param>
        /// <returns></returns>
        public static BoolMessage Validate(IValidator validator)
        {
            IValidationResults results = validator.Validate() as IValidationResults;

            if (results.IsValid) return new BoolMessage(true, string.Empty);

            string multiLineError = results.Message();
            return new BoolMessage(false, multiLineError);
        }


        /// <summary>
        /// 验证规则，并返回一个boolMessage
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        public static bool ValidateAndCollect(IValidator validator, IValidationResults results)
        {
            IValidationResults validationResults = validator.Validate(results);
            return validationResults.IsValid;
        }
    }
}
