using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.EventAggregator.Messages
{
    public class PersonDetailMessage : BasePersonMessage
    {
        public PersonDetailMessage(Person person) : base(person)
        {
        }
    }
}
