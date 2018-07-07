using Common.FlashMessages;
using Common.ViewModelResolver;
using Common.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common.ViewModels;
using Caliburn.Micro;

namespace Common.ViewModels
{
    public abstract class BaseConductorOneActive : Conductor<IViewModel>.Collection.OneActive, IViewModel, INotifyDataErrorInfo
    {
        // property injection
        private IEventAggregator _eventAggregator;
        public IEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
            set { _eventAggregator = value; }
        }


        // property injection
        private IViewModelResolver<IViewModel> _viewModelResolver;
        public IViewModelResolver<IViewModel> ViewModelResolver
        {
            get { return _viewModelResolver; }
            set { _viewModelResolver = value; }
        }


        // property injection
        private IFlashMessagesManager _flashMessagesManager;
        public IFlashMessagesManager FlashMessagesManager
        {
            get { return _flashMessagesManager; }
            set { _flashMessagesManager = value; }
        }


        protected void ActivateItem(string viewModelName)
        {
            ActivateItem(GetViewModel(viewModelName));
        }


        protected IViewModel GetViewModel(string viewModelName)
        {
            IViewModel viewModel = _viewModelResolver.Resolve(viewModelName);
            if (viewModel == null) {
                throw new Exception("Requested ViewModel does not Exist!");
            }

            return viewModel;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            InitializeValidation();
        }


        // ----- INotifyPropertyChanged


        public override bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            Validation.Check(propertyName, newValue, true);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

            return base.Set(ref oldValue, newValue, propertyName);
        }


        // ----- INotifyDataErrorInfo


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        // property injection
        private IValidationObject _validation;
        public IValidationObject Validation
        {
            get { return _validation; }
            set { _validation = value; }
        }


        protected virtual void InitializeValidation()
        {
        }


        public bool HasErrors
        {
            get { return Validation.HasErrors; }
        }


        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) {
                // todo
                return new List<string>();
            }

            if (!Validation.Errors.ContainsKey(propertyName)) {
                return new List<string>();
            }

            return Validation.Errors[propertyName];
        }

    }
}
