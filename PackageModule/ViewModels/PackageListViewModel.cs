using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Serilog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VersionController.Core;
using VersionController.PackageModule.Models;

namespace VersionController.PackageModule.ViewModels
{
    public class PackageListViewModel : BindableBase, INavigationAware
    {
        private readonly ILogger _logger;
        private readonly IDirectoryUtils _directoryUtils;

        private ObservableCollection<Package> _packages = new();
        private Package _selectedPackage = new();
        private bool _isVisible = false;
        private bool _isAllChecked = false;
        private string _token = string.Empty;

        public ObservableCollection<Package> Packages
        {
            get => _packages;
            set { SetProperty(ref _packages, value); }
        }

        public string Token
        {
            get => _token;
            set 
            { 
                SetProperty(ref _token, value);
                if (string.IsNullOrEmpty(_token)) 
                { 
                    Refresh();
                }
            }
        }

        // TODO: Handle the selected package in the list properly
        public Package SelectedPackage
        {
            get => _selectedPackage;
            set 
            { 
                SetProperty(ref _selectedPackage, value); 
                if(_selectedPackage != null) 
                {
                    Package? package = Packages.ToList().FirstOrDefault(x => x.Name == _selectedPackage.Name);
                    if (package != null) 
                    { 
                        package.IsChecked = !_selectedPackage.IsChecked;                  
                    }
                }

                IsVisible = Packages.Any(x => x.IsChecked);
            }
        }

        public bool IsAllChecked
        {
            get => _isAllChecked;
            set 
            { 
                SetProperty(ref _isAllChecked, value);
                Packages.ToList().ForEach(x => x.IsChecked = _isAllChecked);
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set { SetProperty(ref _isVisible, value); }
        }

        public DelegateCommand<string> SearchCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand PublishCommand { get; set; }

        public PackageListViewModel(ILogger logger, IDirectoryUtils directoryUtils)
        {
            _logger = logger;
            _directoryUtils = directoryUtils;

            SearchCommand = new DelegateCommand<string>(OnSearch);
            DeleteCommand = new DelegateCommand(OnDelete);
            PublishCommand = new DelegateCommand(OnPublish);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Refresh();
        }

        private void Refresh() 
        {
            IsVisible = false;
            Packages.Clear();

            List<string> packages = _directoryUtils.GetPackages();

            foreach (string package in packages) 
            {
                Packages.Add(new Package(package));
            }       
        }

        private void OnSearch(string token) 
        {
            if (string.IsNullOrEmpty(token)) 
            {
                return;
            }

            Packages.Clear();

            List<string> filterPackages = _directoryUtils.GetFilterPackages(token);

            foreach (string filterPackage in filterPackages) 
            {
                Packages.Add(new Package(filterPackage));
            }
        }

        private void OnDelete() 
        {
            if (!Packages.Any(x => x.IsChecked)) 
            {
                return;
            }

            List<string> deletePackages = new();

            foreach (Package package in Packages) 
            {
                if (package.IsChecked) 
                {
                    deletePackages.Add(package.Name);
                }
            }

            _directoryUtils.Delete(deletePackages);                     
        }

        private void OnPublish() 
        { 
            //TODO: Implement the publish feature
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
