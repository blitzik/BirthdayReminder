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
using prjt.Facades;

namespace prjt.ViewModels
{
    public class PersonsListViewModel : BaseScreen
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
                    EventAggregator.PublishOnUIThread(new BirthdayChangeViewMessage<EmptySelectionViewModel>(ViewSide.RIGHT));
                } else {
                    EventAggregator.PublishOnUIThread(new BirthdayChangeViewMessage<PersonDetailViewModel>(ViewSide.RIGHT));
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


        private DelegateCommand<Person> _removePersonCommand;
        public DelegateCommand<Person> RemovePersonCommand
        {
            get
            {
                if (_removePersonCommand == null) {
                    _removePersonCommand = new DelegateCommand<Person>(p => RemovePerson(p));
                }
                return _removePersonCommand;
            }
        }


        private PersonFacade _personFacade;

        public PersonsListViewModel(PersonFacade personFacade)
        {
            _personFacade = personFacade;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);
        }


        // -----


        private void AddPerson()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<PersonFormViewModel>());
        }


        private void RemovePerson(Person p)
        {
            Persons.Remove(p);
            SelectedPerson = Persons.Count > 0 ? Persons.First() : null;

            _personFacade.DeletePerson(p);
            FlashMessagesManager.DisplayFlashMessage("Záznam byl úspěšně odstraněn!", Common.FlashMessages.Type.SUCCESS);
        }



    }
}
