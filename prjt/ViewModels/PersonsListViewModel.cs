using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using prjt.EventAggregator.Messages;
using prjt.Domain;
using Common.Commands;
using Common.EventAggregator.Messages;
using prjt.ViewModels.Base;

namespace prjt.ViewModels
{
    public class PersonsListViewModel : BaseScreen, IHandle<PersonRemovedMessage>
    {
        private ObservableCollection<Person> _persons;
        public ObservableCollection<Person> Persons
        {
            get { return _persons; }
            set
            {
                _persons = value;
                NotifyOfPropertyChange(() => Persons);
            }
        }
        

        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                NotifyOfPropertyChange(() => SelectedPerson);
                if (value == null) {
                    EventAggregator.PublishOnUIThread(new BirthdayChangeViewMessage<IViewModel>(nameof(EmptySelectionViewModel), prjt.ViewModels.ViewSide.RIGHT));
                } else {
                    EventAggregator.PublishOnUIThread(new BirthdayChangeViewMessage<IViewModel>(nameof(PersonDetailViewModel), prjt.ViewModels.ViewSide.RIGHT));
                    EventAggregator.PublishOnUIThread(new PersonDetailMessage(value));
                }
            }
        }


        private DelegateCommand<object> _addPersonCommand;
        public DelegateCommand<object> AddPersonCommand
        {
            get
            {
                if (_addPersonCommand == null) {
                    _addPersonCommand = new DelegateCommand<object>(p => AddPerson());
                }
                return _addPersonCommand;
            }
        }


        public PersonsListViewModel()
        {
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);
        }


        public void Handle(PersonRemovedMessage message)
        {
            Persons.Remove(message.Person);
            SelectedPerson = Persons.FirstOrDefault();
        }


        // -----


        private void AddPerson()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<IViewModel>(nameof(PersonFormViewModel)));
        }
        

    }
}
