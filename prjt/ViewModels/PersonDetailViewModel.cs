using Caliburn.Micro;
using Common.Commands;
using Common.EventAggregator.Messages;
using prjt.Domain;
using prjt.EventAggregator.Messages;
using prjt.ViewModels;
using prjt.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.ViewModels
{
    public class PersonDetailViewModel : BaseConductorOneActive, IHandle<PersonDetailMessage>
    {
        private Person _person;
        public Person Person
        {
            get { return _person; }
        }


        private DelegateCommand<object> _deleteRecordCommand;
        public DelegateCommand<object> DeleteRecordCommand
        {
            get
            {
                if (_deleteRecordCommand == null) {
                    _deleteRecordCommand = new DelegateCommand<object>(
                        p => DisplayRecordDeletion(),
                        p => _person != null
                    );
                }
                return _deleteRecordCommand;
            }
        }


        private DelegateCommand<object> _updateRecordCommand;
        public DelegateCommand<object> UpdateRecordCommand
        {
            get
            {
                if (_updateRecordCommand == null) {
                    _updateRecordCommand = new DelegateCommand<object>(
                        p => DisplayRecordUpdate(),
                        p => _person != null
                    );
                }
                return _updateRecordCommand;
            }
        }


        private IWindowManager _windowManager;

        public PersonDetailViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);
        }


        public void Handle(PersonDetailMessage message)
        {
            _person = message.Person;
            NotifyOfPropertyChange(() => Person);
            DeleteRecordCommand.RaiseCanExecuteChanged();
            UpdateRecordCommand.RaiseCanExecuteChanged();
        }


        private void DisplayRecordDeletion()
        {
            ConfirmationViewModel vm = PrepareViewModel<ConfirmationViewModel>();
            vm.Text = "Skutečně si přejete odstranit vybraný záznam?";
            vm.OnYesClicked += () => {
                Person.IsMarkedForDelete = true;
                Overlay.HideOverlay();
            };
            vm.OnCancelClicked += () => { Overlay.HideOverlay(); };

            Overlay.DisplayOverlay(vm);
        }


        private void DisplayRecordUpdate()
        {
            PersonFormViewModel vm = ViewModelResolver.Resolve<PersonFormViewModel>();
            vm.Person = Person;

            EventAggregator.BeginPublishOnUIThread(new ChangeViewMessage<IViewModel>(vm));
        }
    }
}
