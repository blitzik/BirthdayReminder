﻿using Caliburn.Micro;
using Common.Commands;
using Common.EventAggregator.Messages;
using Common.Validation;
using prjt.Domain;
using prjt.EventAggregator.Messages;
using prjt.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace prjt.ViewModels
{
    public class PersonFormViewModel : BaseScreen
    {
        private Person _person;
        public Person Person
        {
            get { return _person; }
            set
            {
                _person = value;
                FirstName = value.FirstName;
                LastName = value.LastName;
                Note = value.Note;
                SelectedDate = value.Birthday;
            }
        }


        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                Set(ref _firstName, value);
                SaveRecordCommand.RaiseCanExecuteChanged();
            }
        }


        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                Set(ref _lastName, value);
                SaveRecordCommand.RaiseCanExecuteChanged();
            }
        }


        private string _note;
        public string Note
        {
            get { return _note; }
            set
            {
                Set(ref _note, value);
            }
        }


        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                Set(ref _selectedDate, value);
                SaveRecordCommand.RaiseCanExecuteChanged();
            }
        }


        private DelegateCommand<object> _returnBackCommand;
        public DelegateCommand<object> ReturnBackCommand
        {
            get
            {
                if (_returnBackCommand == null) {
                    _returnBackCommand = new DelegateCommand<object>(p => ReturnBack());
                }
                return _returnBackCommand;
            }
        }


        private DelegateCommand<object> _saveRecordCommand;
        public DelegateCommand<object> SaveRecordCommand
        {
            get
            {
                if (_saveRecordCommand == null) {
                    _saveRecordCommand = new DelegateCommand<object>(
                        p => SaveRecord(),
                        p => Validation.Check<string>(nameof(FirstName), FirstName) && Validation.Check<string>(nameof(LastName), LastName) && SelectedDate != null
                    );
                }
                return _saveRecordCommand;
            }
        }


        private PersonFacade _personFacade;

        public PersonFormViewModel(PersonFacade personFacade)
        {
            _personFacade = personFacade;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);

            if (Person == null) {
                FirstName = null;
                LastName = null;
                Note = null;
                SelectedDate = DateTime.Today;
            }
        }


        protected override void InitializeValidation()
        {
            //Validation.CreateRuleSet<string>(new string[] { nameof(FirstName), nameof(LastName) })
            //          .AddRule("Vyplňte prosím toto pole", Severity.INFO, x => { return string.IsNullOrEmpty(x); });

            Validation.CreateRuleSet<string>(nameof(FirstName))
                      .AddRule("Vyplňte prosím pole Jméno", Severity.INFO, x => { return string.IsNullOrEmpty(x); })
                      .AddRule("Jméno musí obsahovat alespoň 3 znaky", Severity.INFO, x => { return !string.IsNullOrEmpty(x) && x.Length < 3; });

            Validation.CreateRuleSet<string>(nameof(LastName))
                      .AddRule("Vyplňte prosím pole Příjmení", Severity.INFO, x => { return string.IsNullOrEmpty(x); })
                      .AddRule("Příjmení musí obsahovat alespoň 3 znaky", Severity.INFO, x => { return !string.IsNullOrEmpty(x) && x.Length < 3; });
        }


        private void SaveRecord()
        {
            if (Person == null) {
                Person p = new Person(FirstName, LastName, SelectedDate) { Note = _note };
                _personFacade.StorePerson(p);
                EventAggregator.PublishOnUIThread(new PersonCreatedMessage(p));
                FlashMessagesManager.DisplayFlashMessage("Záznam byl úspěšně přidán!", Common.FlashMessages.Type.SUCCESS);

            } else {
                Person.FirstName = FirstName;
                Person.LastName = LastName;
                Person.Note = Note;
                Person.Birthday = SelectedDate;
                _personFacade.UpdatePerson(Person);
                FlashMessagesManager.DisplayFlashMessage("Záznam byl úspěšně upraven!", Common.FlashMessages.Type.SUCCESS);
            }
            EventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(BirthdaysViewModel)));
        }


        private void ReturnBack()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(BirthdaysViewModel)));
        }
    }
}
