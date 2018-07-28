using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjt.ViewModels;
using prjt.ViewModels.Base;

namespace prjt.EventAggregator.Messages
{
    public class BirthdayChangeViewMessage<T> : IBirthdayChangeViewMessage<IViewModel> where T : IViewModel
    {
        private ViewSide _side;
        public ViewSide Side
        {
            get { return _side; }
        }


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


        public BirthdayChangeViewMessage(ViewSide side)
        {
            _type = typeof(T);
            _side = side;
        }


        public BirthdayChangeViewMessage(T viewModel, ViewSide side)
        {
            _viewModel = viewModel;
            _side = side;
        }
    }
}
