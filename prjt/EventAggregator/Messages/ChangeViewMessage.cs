using Common.EventAggregator.Messages;
using prjt.Domain;
using prjt.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.EventAggregator.Messages
{
    public class ChangeViewMessage<T> : IChangeViewMessage<IViewModel> where T : IViewModel
    {
        protected Type _type;
        public Type Type
        {
            get { return _type; }
        }


        protected IViewModel _viewModel;
        public IViewModel ViewModel
        {
            get { return _viewModel; }
        }


        private Action<T> _action;


        public ChangeViewMessage(Action<T> action = null)
        {
            _type = typeof(T);
            _action = action;
        }


        public ChangeViewMessage(T viewModel, Action<T> action = null)
        {
            _viewModel = viewModel;
            _action = action;
        }


        public void Apply(IEnumerable<IViewModel> viewModels)
        {
            if (_action != null) {
                foreach (IViewModel vm in viewModels) {
                    _action?.Invoke((T)vm);
                }
            }
        }


        public void Apply(IViewModel viewModel)
        {
            _action?.Invoke((T)viewModel);
        }
    }
}
