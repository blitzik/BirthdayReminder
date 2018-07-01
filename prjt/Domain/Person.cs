using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Domain
{
    public class Person : PropertyChangedBase
    {
        private int _birthdayIndex;
        public int BirthdayIndex
        {
            get { return _birthdayIndex; }
        }


        public void UpdateIndexes()
        {
            _birthdayIndex = CalcDateIndex(Birthday);
        }


        public static int CalcDateIndex(DateTime date)
        {
            return 31 * (date.Month - 1) + date.Day;
        }


        // -----


        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; NotifyOfPropertyChange(() => FirstName); }
        }


        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; NotifyOfPropertyChange(() => LastName); }
        }


        private string _note;
        public string Note
        {
            get { return _note; }
            set { _note = value; NotifyOfPropertyChange(() => Note); }
        }


        private DateTime _birthday;
        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                NotifyOfPropertyChange(() => Birthday);
                NotifyOfPropertyChange(() => DaysToBirthday);
            }
        }


        public int DaysToBirthday
        {
            get
            {
                DateTime today = DateTime.Today;
                int year = today.Year;
                if (Birthday.Month < today.Month || (Birthday.Month <= today.Month && Birthday.Day < today.Day)) {
                    year++;
                }
                DateTime nextBirthday = new DateTime(year, Birthday.Month, Birthday.Day);

                TimeSpan result = nextBirthday.Subtract(today);
                return result.Days;
            }
        }


        public Person(string firstName, string lastName, DateTime birthday)
        {
            _firstName = firstName;
            _lastName = lastName;
            _birthday = birthday;

            UpdateIndexes();
        }
    }
}
