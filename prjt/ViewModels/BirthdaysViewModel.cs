using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjt.EventAggregator.Messages;
using prjt.ViewModels.Base;

namespace prjt.ViewModels
{
    public enum ViewSide
    {
        LEFT,
        RIGHT
    }


    public class BirthdaysViewModel : BaseConductorAllActive, IHandle<IBirthdayChangeViewMessage<IViewModel>>
    {
        private IViewModel _leftSide;
        public IViewModel LeftSide
        {
            get { return _leftSide; }
            set
            {
                _leftSide = value;
                NotifyOfPropertyChange(() => LeftSide);
            }
        }


        private IViewModel _rightSide;
        public IViewModel RightSide
        {
            get { return _rightSide; }
            set
            {
                _rightSide = value;
                NotifyOfPropertyChange(() => RightSide);
            }
        }


        public BirthdaysViewModel()
        {
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);

            ActivateItem(ViewModelResolver.Resolve<PersonDetailViewModel>());

            LeftSide = ActivateItem<PersonsOverviewViewModel>();
            RightSide = ActivateItem<EmptySelectionViewModel>();
        }


        public void Handle(IBirthdayChangeViewMessage<IViewModel> message)
        {
            IViewModel vm;
            if (message.ViewModel != null) {
                vm = message.ViewModel;
                ActivateItem(vm);
            } else {
                vm = GetViewModel(message.Type);
                ActivateItem(vm);
            }

            switch (message.Side) {
                case ViewSide.LEFT:
                    LeftSide = vm;
                    break;

                case ViewSide.RIGHT:
                    RightSide = vm;
                    break;
            }
        }

    }
}
