using Perst;
using prjt.Domain;
using prjt.Services.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Facades
{
    public class PersonFacade : BaseFacade
    {
        public PersonFacade(StoragePool storagePool) : base(storagePool)
        {
        }


        public void StorePerson(Person person)
        {
            Root().PersonIndex.Put(person);
            Storage().Commit();
        }


        public void UpdatePerson(Person person)
        {
            Root().PersonIndex.Remove(person);
            person.UpdateIndexes();
            Storage().Modify(person);
            Root().PersonIndex.Put(person);
            Storage().Commit();
        }


        public List<Person> FindAllBirthdays()
        {
            DateTime now = DateTime.Now;
            var persons = from Person p in Root().PersonIndex select p;

            return new List<Person>(persons);
        }


        public List<Person> FindUpcommingBirthdays()
        {
            int now = Person.CalcDateIndex(DateTime.Today);
            var persons = from Person p in Root().PersonIndex where p.BirthdayIndex >= now select p;

            return new List<Person>(persons);
        }


        public List<Person> FindLastBirthdays()
        {
            int now = Person.CalcDateIndex(DateTime.Today);
            var persons = from Person p in Root().PersonIndex where p.BirthdayIndex < now select p;

            return new List<Person>(persons);
        }


        public void DeletePerson(Person person)
        {
            Root().PersonIndex.Remove(person);
            Storage().Deallocate(person);
            Storage().Commit();
        }
    }
}
