using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Validation
{
    public interface IDelegateRuleSet<T> : IRuleSet
    {
        IDelegateRuleSet<T> AddRule(string message, Severity severity, Func<T, bool> action);
        bool Check(T obj);
    }
}
