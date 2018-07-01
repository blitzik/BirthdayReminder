using Caliburn.Micro;
using Common.ViewModels;
using System;

namespace Common.ViewModelResolver
{
    public class ViewModelResolver : IViewModelResolver<IViewModel>
    {
        private readonly SimpleContainer _container;


        public ViewModelResolver(SimpleContainer container)
        {
            _container = container;
        }


        public IViewModel Resolve(string viewModel)
        {
            IViewModel vm = _container.GetInstance(Type.GetType(viewModel), viewModel) as IViewModel;
            if (vm != null) {
                _container.BuildUp(vm);
            }

            return vm;
        }
    }
}
