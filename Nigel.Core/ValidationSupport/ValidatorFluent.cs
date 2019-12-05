namespace Nigel.Core.ValidationSupport
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 21:59:54
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Linq.Expressions;

    public class ValidatorFluent
    {
        private object _target;
        private string _objectName;
        private bool _appendObjectNameToError;
        private string _propertyName;
        private bool _checkCondition;
        private IValidationResults _errors;
        private int _initialErrorCount;


        public ValidatorFluent(Type typeToCheck)
            : this(typeToCheck, false, null)
        {
        }


        public ValidatorFluent(Type typeToCheck, IValidationResults errors)
            : this(typeToCheck, false, errors)
        {
        }


        public ValidatorFluent(Type typeToCheck, bool appendTypeToError, IValidationResults errors)
        {
            _objectName = typeToCheck.Name;
            _appendObjectNameToError = appendTypeToError;
            _errors = errors == null ? new ValidationResults() : errors;
            _initialErrorCount = _errors.Count;
        }


        public bool HasErrors
        {
            get { return _errors.Count > _initialErrorCount; }
        }


        public IValidationResults Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }


        public ValidatorFluent Check(object target)
        {
            _checkCondition = true;
            _propertyName = string.Empty;
            _target = target;
            return this;
        }


        public ValidatorFluent Check(Expression<Func<object>> exp)
        {
            _target = ExpressionHelper.GetPropertyNameAndValue(exp, ref _propertyName);
            _checkCondition = true;
            return this;
        }


        public ValidatorFluent Check(string propName, object target)
        {
            _checkCondition = true;
            _propertyName = propName;
            _target = target;
            return this;
        }


        public ValidatorFluent If(bool isOkToCheckNext)
        {
            _checkCondition = isOkToCheckNext;
            return this;
        }


        public ValidatorFluent Is(object val)
        {
            if (!_checkCondition) return this;

            if (_target == null && val == null)
                return this;

            if (_target == null && val != null)
                return IsValid(false, "必须等于 : " + val.ToString());

            return IsValid(val.Equals(_target), "必须等于 : " + val.ToString());
        }


        public ValidatorFluent IsNot(object val)
        {
            if (!_checkCondition) return this;

            if (_target == null && val == null)
                return this;

            if (_target == null && val != null)
                return this;

            return IsValid(!val.Equals(_target), "必须不等于 : " + val.ToString());
        }

        public ValidatorFluent IsNull()
        {
            if (!_checkCondition) return this;

            return IsValid(_target == null, "必须为空");
        }


        public ValidatorFluent IsNotNull()
        {
            if (!_checkCondition) return this;

            return IsValid(_target != null, "不能为空");
        }


        public ValidatorFluent In<T>(params object[] vals)
        {
            if (!_checkCondition) return this;

            if (vals == null || vals.Length == 0)
                return this;

            T checkVal = _target.Convert<T>();
            bool isValid = false;
            foreach (object val in vals)
            {
                T validVal = val.Convert<T>();
                if (checkVal.Equals(validVal))
                {
                    isValid = true;
                    break;
                }
            }
            return IsValid(isValid, "不是一个有效值");
        }


        public ValidatorFluent NotIn<T>(params object[] vals)
        {
            if (!_checkCondition) return this;

            if (vals == null || vals.Length == 0)
                return this;

            T checkVal = _target.Convert<T>();
            bool isValid = true;
            foreach (object val in vals)
            {
                T validVal = val.Convert<T>();
                if (checkVal.Equals(validVal))
                {
                    isValid = false;
                    break;
                }
            }
            return IsValid(isValid, "不是一个有效值");
        }


        public ValidatorFluent Matches(string regex)
        {
            if (!_checkCondition) return this;

            return IsValid(Regex.IsMatch((string)_target, regex), "不匹配 : " + regex);
        }


        public ValidatorFluent IsBetween(int min, int max)
        {
            if (!_checkCondition) return this;

            bool isNumeric = TypeHelper.IsNumeric(_target ?? string.Empty);
            if (isNumeric)
            {
                double val = Convert.ToDouble(_target);
                return IsValid(min <= val && val <= max, "必须介于 : " + min + ", " + max);
            }
            else
            {
                string strVal = _target as string;
                if (min > 0 && string.IsNullOrEmpty(strVal))
                    return IsValid(false, "长度必须介于 : " + min + ", " + max);

                return IsValid(min <= strVal.Length && strVal.Length <= max, "长度必须介于 : " + min + ", " + max);
            }
        }


        public ValidatorFluent Contains(string val)
        {
            if (!_checkCondition) return this;

            if (string.IsNullOrEmpty((string)_target))
                return IsValid(false, "不包含 : " + val);

            string valToCheck = (string)_target;
            return IsValid(valToCheck.Contains(val), "必须包含 : " + val);
        }


        public ValidatorFluent NotContain(string val)
        {
            if (!_checkCondition) return this;

            if (string.IsNullOrEmpty((string)_target))
                return this;

            string valToCheck = (string)_target;
            return IsValid(!valToCheck.Contains(val), "不应该包含 : " + val);
        }


        public ValidatorFluent Min(int min)
        {
            if (!_checkCondition) return this;

            bool isNumeric = TypeHelper.IsNumeric(_target);
            if (!isNumeric) return IsValid(false, "必须为数字值");

            double val = Convert.ToDouble(_target);
            return IsValid(val >= min, "必须大于最小值 : " + min);
        }


        public ValidatorFluent Max(int max)
        {
            if (!_checkCondition) return this;

            bool isNumeric = TypeHelper.IsNumeric(_target);
            if (!isNumeric) return IsValid(false, "必须为数字值");

            double val = Convert.ToDouble(_target);
            return IsValid(val <= max, "必须小于最大值 : " + max);
        }


        public ValidatorFluent IsTrue()
        {
            if (!_checkCondition) return this;

            bool isBool = _target is bool;
            if (!isBool) return IsValid(false, "必须是布尔值");

            return IsValid(((bool)_target) == true, "必须为 true");
        }


        public ValidatorFluent IsFalse()
        {
            if (!_checkCondition) return this;

            bool isBool = _target is bool;
            if (!isBool) return IsValid(false, "必须是布尔值");

            return IsValid(((bool)_target) == false, "必须为 false");
        }


        public ValidatorFluent IsAfterToday()
        {
            if (!_checkCondition) return this;

            IsAfter(DateTime.Today);
            _checkCondition = false;
            return this;
        }


        public ValidatorFluent IsBeforeToday()
        {
            if (!_checkCondition) return this;

            IsBefore(DateTime.Today);
            _checkCondition = false;
            return this;
        }


        public ValidatorFluent IsAfter(DateTime date)
        {
            if (!_checkCondition) return this;

            DateTime checkVal = (DateTime)_target;
            return IsValid(checkVal.Date.CompareTo(date.Date) > 0, "必须大于日期 : " + date.ToString());
        }


        public ValidatorFluent IsBefore(DateTime date)
        {
            if (!_checkCondition) return this;

            DateTime checkVal = (DateTime)_target;
            return IsValid(checkVal.Date.CompareTo(date.Date) < 0, "必须小于日期 : " + date.ToString());
        }


        public ValidatorFluent IsValidEmail()
        {
            if (!_checkCondition) return this;

            return IsValid(Validation.IsEmail((string)_target, false), "必须是一个有效的Email");
        }


        public ValidatorFluent IsValidMobilePhone()
        {
            if (!_checkCondition) return this;

            return IsValid(Validation.IsMobilePhone((string)_target, false), "必须是一个有效的手机号码");
        }

        public ValidatorFluent IsValidTelPhone()
        {
            if (!_checkCondition) return this;

            return IsValid(Validation.IsTelPhone((string)_target, false), "必须是一个有效的电话号码");
        }

        public ValidatorFluent IsValidIdentityCard()
        {
            if (!_checkCondition) return this;

            return IsValid(Validation.IsIdentityCard((string)_target, false), "必须是一个有效的身份证号码");
        }


        public ValidatorFluent IsValidUrl()
        {
            if (!_checkCondition) return this;

            return IsValid(Validation.IsUrl((string)_target, false), "必须是一个有效的URL");
        }


        public ValidatorFluent IsValidZip()
        {
            if (!_checkCondition) return this;

            return IsValid(Validation.IsZipCode((string)_target, false), "必须是一个有效的邮编");
        }


        public ValidatorFluent End()
        {
            return this;
        }


        #region Check
        private ValidatorFluent IsValid(bool isValid, string error)
        {
            if (!isValid)
            {
                string prefix = string.IsNullOrEmpty(_propertyName) ? "Property " : _propertyName + " ";
                _errors.Add(prefix + error);
            }
            return this;
        }
        #endregion
    }
}
