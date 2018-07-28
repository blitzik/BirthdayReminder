using Caliburn.Micro;
using Common.FlashMessages;
using Common.Validation;
using Common.ViewModelResolver;
using Common.ViewModels;
using intf.Views;
using Perst;
using prjt.Domain;
using prjt.Facades;
using prjt.Services.Persistence;
using prjt.ViewModels;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace BirthdayReminder
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;


        public Bootstrapper()
        {
            Initialize();
        }


        protected override void Configure()
        {
            var config = new TypeMappingConfiguration()
            {
                DefaultSubNamespaceForViews = "intf.Views",
                DefaultSubNamespaceForViewModels = "prjt.ViewModels"
            };
            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);
                        

            // -----


            _container = new SimpleContainer();
            _container.Instance(_container);

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, Caliburn.Micro.EventAggregator>();

            _container.Singleton<IViewModelResolver, ViewModelResolver>();
            

            // default window definition
            _container.Singleton<MainViewModel>();

            // window definitions
            _container.PerRequest<DeletePersonViewModel>(typeof(DeletePersonViewModel).FullName);

            // View Model definitions
            _container.Singleton<BirthdaysViewModel>(typeof(BirthdaysViewModel).FullName);
            _container.PerRequest<PersonFormViewModel>(typeof(PersonFormViewModel).FullName);
            _container.Singleton<PersonDetailViewModel>(typeof(PersonDetailViewModel).FullName);
            _container.Singleton<PersonsOverviewViewModel>(typeof(PersonsOverviewViewModel).FullName);
            _container.Singleton<PersonsListViewModel>(typeof(PersonsListViewModel).FullName);
            _container.Singleton<PersonsLoadingScreenViewModel>(typeof(PersonsLoadingScreenViewModel).FullName);
            _container.Singleton<EmptySelectionViewModel>(typeof(EmptySelectionViewModel).FullName);
            

            // Facades
            _container.Singleton<PersonFacade>();


            // services
            _container.PerRequest<IValidationObject, ValidationObject>();
            _container.Singleton<prjt.Services.Persistence.StorageFactory>();
            _container.Singleton<StoragePool>();
            _container.Singleton<IFlashMessagesManager, FlashMessagesManager>();
        }


        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            StoragePool sp = _container.GetInstance<StoragePool>();
            Storage db = _container.GetInstance<prjt.Services.Persistence.StorageFactory>().OpenConnection(prjt.Services.Persistence.StorageFactory.MAIN_DATABASE_NAME);
            sp.Add(prjt.Services.Persistence.StorageFactory.MAIN_DATABASE_NAME, db);

            //GeneratePersons(db);

            MainViewModel mvm = _container.GetInstance<MainViewModel>();
            _container.BuildUp(mvm);

            _container.GetInstance<IWindowManager>().ShowWindow(mvm);
        }


        protected override void OnExit(object sender, EventArgs e)
        {
        }


        protected override object GetInstance(System.Type service, string key)
        {
            return _container.GetInstance(service, key);
        }


        protected override IEnumerable<object> GetAllInstances(System.Type service)
        {
            return _container.GetAllInstances(service);
        }


        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }


        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            IList<Assembly> assemblies = new List<Assembly>()
            {
                GetType().Assembly,
                typeof(MainView).GetTypeInfo().Assembly
            };

            return assemblies;
        }


        private void GeneratePersons(Storage db)
        {
            Root root = db.Root as Root;
            if (root == null) {
                root = new Root(db);
                db.Root = root;
            }
            Random r = new Random();
            for (int i = 0; i < 100000; i++) {
                DateTime birthday = new DateTime(r.Next(1940, 2011), r.Next(1, 13), r.Next(1, 29));
                Person p = new Person("Lorem", "Consecteteur", birthday);
                root.PersonIndex.Put(p);
            }

            db.Commit();
        }

    }
}
