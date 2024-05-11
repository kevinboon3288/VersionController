namespace SettingsModule.ViewModels;

public class SettingsViewModel : BindableBase, INavigationAware
{
    private readonly ILogger _logger;
    private readonly IRegionManager _regionManager;

    public SettingsViewModel(ILogger logger, IRegionManager regionManager)
    {
        _logger = logger;
        _regionManager = regionManager;
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }
}
