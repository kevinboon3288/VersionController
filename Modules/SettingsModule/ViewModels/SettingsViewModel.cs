namespace SettingsModule.ViewModels;

public class SettingsViewModel : BindableBase, INavigationAware
{
    private readonly ILogger _logger;
    private readonly IRegionManager _regionManager;

    public DelegateCommand ReturnToMainCommand { get; set; }

    public SettingsViewModel(ILogger logger, IRegionManager regionManager)
    {
        _logger = logger;
        _regionManager = regionManager;

        ReturnToMainCommand = new DelegateCommand(OnReturnToMain);
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        IRegion region = _regionManager.Regions["SettingsProjectContentRegion"];
        region.RequestNavigate("SettingsProjectView");
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }

    private void OnReturnToMain()
    {
        IRegion region = _regionManager.Regions["MainContentRegion"];
        region.RequestNavigate("MainView");
    }
}
