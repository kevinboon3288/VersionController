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
        _regionManager.RegisterViewWithRegion("SettingsContentRegion", typeof(SettingsView));
        _regionManager.RegisterViewWithRegion("SettingsProjectContentRegion", typeof(SettingsProjectView));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<SettingsView>();
        containerRegistry.RegisterForNavigation<SettingsProjectView>();

        ViewModelLocationProvider.Register<SettingsView, SettingsViewModel>();
        ViewModelLocationProvider.Register<SettingsProjectView, SettingsProjectViewModel>();
    }
}
