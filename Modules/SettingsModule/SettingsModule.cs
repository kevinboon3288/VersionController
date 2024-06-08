namespace SettingsModule;

public class SettingsModule : IModule
{
    private readonly ILogger _logger;
    private readonly IRegionManager _regionManager;

    public SettingsModule(ILogger logger, IRegionManager regionManager)
    {
        _logger = logger;
        _regionManager = regionManager;
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
        _regionManager.RegisterViewWithRegion("SettingContentRegion", typeof(SettingsView));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<SettingsView>();

        ViewModelLocationProvider.Register<SettingsView, SettingsViewModel>();
    }
}
