using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjt.Domain;

namespace prjt.EventAggregator.Messages
{
    public class PersonCreatedMessage : BasePersonMessage
    {
        public PersonCreatedMessage(Person person) : base(person)
        {
        }
    }
}
