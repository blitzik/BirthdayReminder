using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.EventAggregator.Messages
{
    public abstract class BasePersonMessage
    {
        private Person _person;
        public Person Person
        {
            get { return _person; }
        }


        public BasePersonMessage(Person person)
        {
            _person = person;
        }
    }
}
