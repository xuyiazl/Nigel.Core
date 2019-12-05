namespace Nigel.Core.ValidationSupport
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 22:11:46
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ValidatorWithRules : Validator, IValidatorWithRules
    {
        protected List<ValidationRuleDef> _rules;

        public ValidatorWithRules()
            : this(null)
        {
        }


        /// <summary>
        /// 初始化 lamba 验证
        /// </summary>
        /// <param name="validator"></param>
        public ValidatorWithRules(Func<ValidationEvent, bool> validator)
            : base(validator)
        {
            _rules = new List<ValidationRuleDef>();
        }


        /// <summary>
        /// 新加一个验证规则
        /// </summary>
        /// <param name="rule"></param>
        public void Add(Func<ValidationEvent, bool> rule)
        {
            Add(string.Empty, rule);
        }


        /// <summary>
        /// 新加一个验证规则
        /// </summary>
        /// <param name="ruleName"></param>
        public void Add(string ruleName, Func<ValidationEvent, bool> rule)
        {
            var ruleDef = new ValidationRuleDef() { Name = ruleName, Rule = rule };
            _rules.Add(ruleDef);
        }


        /// <summary>
        /// 移除验证规则
        /// </summary>
        /// <param name="ndx">Key</param>
        public void RemoveAt(int ndx)
        {
            if (ndx < 0 || ndx >= _rules.Count) return;

            _rules.RemoveAt(ndx);
        }


        /// <summary>
        /// 移除验证规则
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            List<int> indexesToRemove = new List<int>();
            for (int ndx = 0; ndx < _rules.Count; ndx++)
            {
                var ruleDef = _rules[ndx];
                if (string.Compare(ruleDef.Name, name, true) == 0)
                    indexesToRemove.Add(ndx);
            }
            if (indexesToRemove.Count > 0)
            {
                indexesToRemove.Reverse();
                foreach (int ndx in indexesToRemove)
                    _rules.RemoveAt(ndx);
            }
        }


        /// <summary>
        /// 获取验证规则
        /// </summary>
        /// <value></value>
        public Func<ValidationEvent, bool> this[int ndx]
        {
            get
            {
                if (ndx < 0 || ndx >= _rules.Count)
                    return null;

                return _rules[ndx].Rule;
            }
        }


        /// <summary>
        /// 清空所有验证规则
        /// </summary>
        public override void Clear()
        {
            _lastValidationResults = new ValidationResults();
            _rules.Clear();
        }


        /// <summary>
        /// 验证规则数量
        /// </summary>
        public int Count
        {
            get { return _rules.Count; }
        }


        /// <summary>
        /// 验证所有规则的规则
        /// </summary>
        /// <param name="validationEvent"></param>
        /// <returns></returns>
        protected override bool ValidateInternal(ValidationEvent validationEvent)
        {
            if (_validatorLamda != null)
                return _validatorLamda(validationEvent);

            int initialErrorCount = validationEvent.Results.Count;
            foreach (var rule in _rules)
            {
                rule.Rule(validationEvent);
            }
            return validationEvent.Results.Count == initialErrorCount;
        }
    }
}
