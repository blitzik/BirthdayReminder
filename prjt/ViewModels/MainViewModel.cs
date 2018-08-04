using Caliburn.Micro;
using Common.EventAggregator.Messages;
using prjt.EventAggregator.Messages;
using prjt.ViewModels.Base;

namespace prjt.ViewModels
{
    public class MainViewModel :
        BaseConductorOneActive,
        IHandle<IChangeViewMessage<IViewModel>>
    {

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
