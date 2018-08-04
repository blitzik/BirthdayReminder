﻿using Caliburn.Micro;
using Common.ViewModelResolver;
using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Common.Validation;
using Common.FlashMessages;

namespace Common.ViewModels
{
    public abstract class BaseConductorAllActive<P> : Conductor<P>.Collection.AllActive, IViewModel, INotifyDataErrorInfo where P : class, IViewModel
    {
        // property injection
        private IEventAggregator _eventAggregator;
        public IEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
            set { _eventAggregator = value; }
        }


        // property injection
        private IViewModelResolver _viewModelResolver;
        public IViewModelResolver ViewModelResolver
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


        protected VM ActivateItem<VM>() where VM : P
        {
            VM vm = GetViewModel<VM>();
            ActivateItem(vm);

            return vm;
        }


        protected VM GetViewModel<VM>() where VM : P
        {
            VM vm = ViewModelResolver.Resolve<VM>();
            if (vm == null) {
                throw new Exception("Requested ViewModel does not Exist!");
            }

            return vm;
        }


        protected P GetViewModel(System.Type viewModel)
        {
            P vm = ViewModelResolver.Resolve<P>(viewModel);
            if (vm == null) {
                throw new Exception("Requested ViewModel does not Exist!");
            }

            return vm;
        }


        protected VM PrepareViewModel<VM>() where VM : IViewModel, new()
        {
            VM vm = Activator.CreateInstance<VM>();
            ViewModelResolver.BuildUp(vm);

            return vm;
        }


        protected VM PrepareViewModel<VM>(Func<VM> instantiation) where VM : IViewModel
        {
            VM vm = instantiation.Invoke();
            ViewModelResolver.BuildUp(vm);

            return vm;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            InitializeValidation();
        }


        // ----- INotifyPropertyChanged


        public override bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Validation != null) {
                Validation.Check(propertyName, newValue, true);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }

            return base.Set(ref oldValue, newValue, propertyName);
        }


        // ----- INotifyDataErrorInfo


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        // property injection
        private IValidationObject _validation;
        public IValidationObject Validation
        {
            get { return _validation; }
            set
            {
                _validation = value;
                _validation.ErrorsChanged += (object sender, DataErrorsChangedEventArgs e) => {
                    ErrorsChanged?.Invoke(this, e);
                };
            }
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
