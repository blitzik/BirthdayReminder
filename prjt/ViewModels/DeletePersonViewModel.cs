using Caliburn.Micro;
using Common.Commands;
using prjt.Domain;
using prjt.EventAggregator.Messages;
using prjt.Facades;
using prjt.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.ViewModels
{
    public class DeletePersonViewModel : BaseScreen
    {
        private Person _person;
        public Person Person
        {
            get { return _person; }
            set { _person = value; }
        }


        private DelegateCommand<object> _deleteRecordCommand;
        public DelegateCommand<object> DeleteRecordCommand
        {
            get
            {
                if (_deleteRecordCommand == null) {
                    _deleteRecordCommand = new DelegateCommand<object>(p => DeleteRecord());
                }
                return _deleteRecordCommand;
            }
        }


        private DelegateCommand<object> _cancelCommand;
        public DelegateCommand<object> CancelCommand
        {
            get
            {
                if (_cancelCommand == null) {
                    _cancelCommand = new DelegateCommand<object>(p => Cancel());
                }
                return _cancelCommand;
            }
        }


        private PersonFacade _personFacade;

        public DeletePersonViewModel(PersonFacade personFacade)
        {
            _personFacade = personFacade;
        }


        private void DeleteRecord()
        {
            Person.IsMarkedForDelete = true;
            _personFacade.DeletePerson(Person);
            FlashMessagesManager.DisplayFlashMessage("Záznam byl úspěšně odstraněn!", Common.FlashMessages.Type.SUCCESS);

            TryClose();
        }


        private void Cancel()
        {
            TryClose();
        }

    }
}
