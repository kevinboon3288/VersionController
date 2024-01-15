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
        private bool _isVisible = new();
        private string _token = string.Empty;

        public ObservableCollection<Package> Packages
        {
            get => _packages;
            set { SetProperty(ref _packages, value); }
        }

        public string Token
        {
            get => _token;
            set { SetProperty(ref _token, value); }
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
                        package.IsChecked = _selectedPackage.IsChecked;                  
                    }
                }

                IsVisible = Packages.Any(x => x.IsChecked);
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set { SetProperty(ref _isVisible, value); }
        }

        public DelegateCommand<string> SearchCommand { get; set; }

        public PackageListViewModel(ILogger logger, IDirectoryUtils directoryUtils)
        {
            _logger = logger;
            _directoryUtils = directoryUtils;

            SearchCommand = new DelegateCommand<string>(OnSearch);
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
                Refresh();
                return;
            }

            Packages.Clear();

            List<string> filterPackages = _directoryUtils.GetFilterPackages(token);

            foreach (string filterPackage in filterPackages) 
            {
                Packages.Add(new Package(filterPackage));
            }
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
