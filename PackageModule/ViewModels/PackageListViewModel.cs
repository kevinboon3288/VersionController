using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Serilog;

namespace VersionController.PackageModule.ViewModels
{
    public class PackageListViewModel : BindableBase, INavigationAware
    {
        private readonly ILogger _logger;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public PackageListViewModel(ILogger logger, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _logger = logger;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}
