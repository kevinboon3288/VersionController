namespace MainModule.ViewModels;

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
        IRegion region = _regionManager.Regions["PackageListContentRegion"];
        region.RequestNavigate("PackageListView");
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }
}
