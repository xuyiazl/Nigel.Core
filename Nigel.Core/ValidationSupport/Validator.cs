namespace Nigel.Core.ValidationSupport
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 21:50:35
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 验证类
    /// </summary>
    public class Validator : IValidator
    {
        public static readonly IValidator Empty = new Validator();


        protected string _message;
        protected object _target;
        protected IValidationResults _lastValidationResults;
        protected Func<ValidationEvent, bool> _validatorLamda;
        protected int _initialErrorCount;
        protected bool _creatValidationEvent;


        public Validator()
        {
        }


        public Validator(Func<ValidationEvent, bool> validator)
        {
            _validatorLamda = validator;
        }


        #region IValidator Members
        /// <summary>
        /// 验证对象
        /// </summary>
        public virtual object Target
        {
            get { return _target; }
            set { _target = value; }
        }


        /// <summary>
        /// 错误描述
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }


        /// <summary>
        /// 是否验证成功
        /// </summary>
        public bool IsValid
        {
            get { return Validate().IsValid; }
        }


        /// <summary>
        /// 验证结果
        /// </summary>
        public IValidationResults Results
        {
            get { return _lastValidationResults; }
        }


        public virtual void Clear()
        {
            _lastValidationResults = new ValidationResults();
        }

        /// <summary>
        /// 初始化信息验证
        /// </summary>
        /// <returns></returns>
        public virtual IValidationResults Validate()
        {
            _lastValidationResults = new ValidationResults() as IValidationResults;
            Validate(Target, _lastValidationResults);
            return _lastValidationResults;
        }


        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual IValidationResults ValidateTarget(object target)
        {
            _lastValidationResults = new ValidationResults() as IValidationResults;
            Validate(new ValidationEvent(target, _lastValidationResults));
            return _lastValidationResults;
        }


        /// <summary>
        /// 验证，并提供结果集
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public virtual IValidationResults Validate(IValidationResults results)
        {
            Validate(new ValidationEvent(Target, results));
            return results;
        }


        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="target"></param>
        /// <param name="results"></param>
        public bool Validate(object target, IValidationResults results)
        {
            return Validate(new ValidationEvent(target, results));
        }
        #endregion


        /// <summary>
        /// 调用<see cref="ValidateInternal"/>验证
        /// </summary>
        /// <remarks>
        /// ValidateInternal方法不能直接调用
        /// 因为CodeGenerator生成验证代码。        
        /// 如果需要重写验证，而利用自动生成器生成验证代码，可以通过重写来调用ValidateInternal方法。        
        /// </remarks>
        /// <param name="validationEvent"></param>
        public virtual bool Validate(ValidationEvent validationEvent)
        {
            return ValidateInternal(validationEvent);
        }

        /// <summary>
        /// 需要实现
        /// </summary>
        /// <param name="validationEvent"></param>
        /// <returns></returns>
        protected virtual bool ValidateInternal(ValidationEvent validationEvent)
        {
            if (_validatorLamda != null)
                return _validatorLamda(validationEvent);

            return true;
        }

        /// <summary>
        /// 添加新的验证结果
        /// </summary>
        /// <param name="results"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        protected void AddResult(IValidationResults results, string key, string message)
        {
            results.Add(key, message);
        }
    }
}
