using Caliburn.Micro;
using Common.Commands;
using Common.EventAggregator.Messages;
using prjt.EventAggregator.Messages;
using prjt.ViewModels.Base;

namespace prjt.ViewModels
{
    public class MainViewModel :
        BaseConductorOneActive,
        IHandle<IChangeViewMessage<IViewModel>>
    {
        private DelegateCommand<object> _hideOverlayCommand;
        public DelegateCommand<object> HideOverlayCommand
        {
            get
            {
                if (_hideOverlayCommand == null) {
                    _hideOverlayCommand = new DelegateCommand<object>(p => {
                        if (!Overlay.Token.IsMandatory) {
                            Overlay.HideOverlay();
                        }
                    });
                }
                return _hideOverlayCommand;
            }
        }


        public MainViewModel()
        {
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);

            Handle(new ChangeViewMessage<BirthdaysViewModel>());
        }


        public void Handle(IChangeViewMessage<IViewModel> message)
        {
            IViewModel vm;
            if (message.ViewModel != null) {
                vm = message.ViewModel;
            } else {
                vm = GetViewModel(message.Type);
            }

            if (vm == ActiveItem) {
                return;
            }
            message.Apply(vm);
            ActivateItem(vm);
        }
    }
}
