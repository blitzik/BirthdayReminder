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
            DeletePersonViewModel dp = (DeletePersonViewModel)ViewModelResolver.Resolve(nameof(DeletePersonViewModel));
            dp.Person = Person;

            _windowManager.ShowDialog(dp);
        }


        private void DisplayRecordUpdate()
        {
            PersonFormViewModel vm = (PersonFormViewModel)ViewModelResolver.Resolve(nameof(PersonFormViewModel));
            vm.Person = Person;

            EventAggregator.BeginPublishOnUIThread(new ChangeViewMessage<IViewModel>(vm));
        }


        // -----


        public void DisplayTestNotifications()
        {
            /*NotificationsManager.DisplayNotifications(
                new Services.Notifications.NotificationsCollection()
                .Add("Lorem ipsum Dolor sit Amet!", Services.Notifications.Type.INFO)
                .Add("Proin in tellus sit amet nibh dignissim sagittis!", Services.Notifications.Type.ERROR)
                .Add("Consectetuer adipiscing.", Services.Notifications.Type.SUCCESS)
                .Add("Doloremque laudantium.", Services.Notifications.Type.WARNING)
            );*/
        }
    }
}
