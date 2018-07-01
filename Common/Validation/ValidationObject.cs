using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Validation
{
    public class ValidationObject : IValidationObject
    {
        private Dictionary<string, List<IValidationMessage>> _errors;


        public ValidationObject()
        {
            _errors = new Dictionary<string, List<IValidationMessage>>();
        }


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public void RaiseErrorsChanged(string propertyname)
        {
            var handler = ErrorsChanged;
            if (handler != null) {
                handler(this, new DataErrorsChangedEventArgs(propertyname));
            }
        }


        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }


        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName)) {
                return null;
            }

            return _errors[propertyName];
        }


        public void AddMessage(string propertyName, string errorMessage, Severity severity = Severity.ERROR)
        {
            if (!_errors.ContainsKey(propertyName)) {
                _errors[propertyName] = new List<IValidationMessage>();
            }

            _errors[propertyName].Add(new ValidationMessage(errorMessage, severity));
            RaiseErrorsChanged(propertyName);
        }


        public void ClearMessages(string propertyName)
        {
            if (_errors.ContainsKey(propertyName)) {
                _errors.Remove(propertyName);
            }
        }
    }
}
