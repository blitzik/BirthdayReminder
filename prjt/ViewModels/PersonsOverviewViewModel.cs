using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjt.EventAggregator.Messages;
using prjt.Domain;
using prjt.Facades;
using prjt.ViewModels.Base;

namespace prjt.ViewModels
{
    public enum FilterOptions
    {
        ALL,
        UPCOMMING,
        LAST
    }


    public class PersonsOverviewViewModel : BaseConductorOneActive, IHandle<PersonCreatedMessage>
    {
        private ObservableCollection<Person> _persons;
        public ObservableCollection<Person> Persons
        {
            get { return _persons; }
            set
            {
                _persons = value;
                NotifyOfPropertyChange(() => Persons);
            }
        }


        private Dictionary<FilterOptions, string> _filterOptions;
        public Dictionary<FilterOptions, string> FilterOptions
        {
            get { return _filterOptions; }
        }


        private bool _isFilterEnabled;
        public bool IsFilterEnabled
        {
            get { return _isFilterEnabled; }
            set
            {
                _isFilterEnabled = value;
                NotifyOfPropertyChange(() => IsFilterEnabled);
            }
        }


        private FilterOptions _selectedFilter;
        public FilterOptions SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                if (_selectedFilter == value) {
                    return;
                }
                _selectedFilter = value;
                LoadRecords();

                NotifyOfPropertyChange(() => SelectedFilter);
            }
        }


        private PersonsListViewModel _personsListViewModel;
        private PersonFacade _personFacade;

        public PersonsOverviewViewModel(PersonFacade personFacade)
        {
            _personFacade = personFacade;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);

            _personsListViewModel = (PersonsListViewModel)ViewModelResolver.Resolve(nameof(PersonsListViewModel));

            _filterOptions = new Dictionary<FilterOptions, string>();
            _filterOptions.Add(ViewModels.FilterOptions.ALL, "Všechny záznamy");
            _filterOptions.Add(ViewModels.FilterOptions.UPCOMMING, "Pouze nadcházející narozeniny");
            _filterOptions.Add(ViewModels.FilterOptions.LAST, "Pouze uplynulé narozeniny");

            SelectedFilter = ViewModels.FilterOptions.UPCOMMING;

            LoadRecords();
        }


        public void Handle(PersonCreatedMessage message)
        {
            LoadRecords();
        }


        private async void LoadRecords()
        {
            IsFilterEnabled = false;
            _personsListViewModel.SelectedPerson = null;
            ActivateItem(nameof(PersonsLoadingScreenViewModel));
            //await Task.Delay(5000);
            ObservableCollection<Person> persons = new ObservableCollection<Person>();
            Task t = Task.Factory.StartNew(() => {
                switch (SelectedFilter) {
                    case ViewModels.FilterOptions.UPCOMMING:
                        persons = new ObservableCollection<Person>(_personFacade.FindUpcommingBirthdays());
                        break;
                    case ViewModels.FilterOptions.LAST:
                        persons = new ObservableCollection<Person>(_personFacade.FindLastBirthdays());
                        break;
                    default:
                        persons = new ObservableCollection<Person>(_personFacade.FindAllBirthdays());
                        break;
                }
                _personsListViewModel.Persons = persons;
                if (persons.Count() > 0) {
                    Person firstPerson = persons.First();
                    _personsListViewModel.SelectedPerson = firstPerson;
                    EventAggregator.PublishOnUIThread(new PersonDetailMessage(firstPerson));
                }
            });
            await t;
            ActivateItem(nameof(PersonsListViewModel));
            IsFilterEnabled = true;
        }
    }
}
