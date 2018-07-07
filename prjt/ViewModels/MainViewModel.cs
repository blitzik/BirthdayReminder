using Caliburn.Micro;
using Common.EventAggregator.Messages;
using prjt.ViewModels.Base;

namespace prjt.ViewModels
{
    public class MainViewModel : BaseConductorOneActive, IHandle<ChangeViewMessage<IViewModel>>
    {

        public MainViewModel()
        {
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);

            ActivateItem(nameof(BirthdaysViewModel));
        }


        public void Handle(ChangeViewMessage<IViewModel> message)
        {
            if (message.ViewModel != null) {
                ActivateItem(message.ViewModel);
            } else {
                ActivateItem(message.ViewModelName);
            }
        }
    }
}
