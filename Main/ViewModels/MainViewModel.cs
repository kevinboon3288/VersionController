using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Serilog;
using System;
using VersionController.PackageModule.ViewModels;
using VersionController.Services.Events;

namespace VersionController.Main.ViewModels
{
    public class MainViewModel : BindableBase, INavigationAware
    {
        private readonly ILogger _logger;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public event EventHandler? LogReceived;

        public MainViewModel(ILogger logger, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _logger = logger;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<LogEvent>().Subscribe(OnLogEventReceived);
        }

        private void OnLogEventReceived(string message)
        {
            LogEventArgs eventArgs = new()
            {
                LogMessage = message
            };

            LogReceived?.Invoke(this, eventArgs);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _logger.Information($"It is at {nameof(MainViewModel)}.{nameof(OnNavigatedTo)} now.");

            IRegion region = _regionManager.Regions["PackageListContentRegion"];
            region.RequestNavigate("PackageListView");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _logger.Information($"It is at {nameof(MainViewModel)}.{nameof(OnNavigatedFrom)} now.");
        }
    }
}
