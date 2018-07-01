using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjt.ViewModels;
using Common.ViewModels;

namespace prjt.EventAggregator.Messages
{
    public class BirthdayChangeViewMessage
    {
        private ViewSide _side;
        public ViewSide Side
        {
            get { return _side; }
        }


        private string _viewModelName;
        public string ViewModelName
        {
            get { return _viewModelName; }
        }


        private IViewModel _viewModel;
        public IViewModel ViewModel
        {
            get { return _viewModel; }
        }


        public BirthdayChangeViewMessage(string viewName, ViewSide side)
        {
            _viewModelName = viewName;
            _side = side;
        }


        public BirthdayChangeViewMessage(IViewModel viewModel, ViewSide side)
        {
            _viewModel = viewModel;
            _side = side;
        }
    }
}
