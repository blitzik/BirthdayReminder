using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Validation
{
    public interface IValidationObject : INotifyDataErrorInfo
    {
        void RaiseErrorsChanged(string propertyName);

        void AddMessage(string propertyName, string errorMessage, Severity severity = Severity.ERROR);

        void ClearMessages(string propertyName);
    }
}
