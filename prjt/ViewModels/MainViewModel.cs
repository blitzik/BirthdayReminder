using Caliburn.Micro;
using Common.EventAggregator.Messages;
using Common.ViewModels;

namespace prjt.ViewModels
{
    public class MainViewModel : BaseConductorOneActive, IHandle<ChangeViewMessage>
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


        public void Handle(ChangeViewMessage message)
        {
            if (message.ViewModel != null) {
                ActivateItem(message.ViewModel);
            } else {
                ActivateItem(message.ViewModelName);
            }
        }
    }
}
