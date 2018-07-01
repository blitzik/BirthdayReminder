using prjt.Domain;
using Perst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Services.Persistence
{
    public class Root : Persistent
    {
        public FieldIndex<int, Person> PersonIndex;


        public Root(Storage db)
        {
            PersonIndex = db.CreateFieldIndex<int, Person>("_birthdayIndex", false);
        }
    }
}
