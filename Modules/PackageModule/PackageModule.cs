namespace PackageModule;

public class PackageModule : IModule
{
    private readonly ILogger _logger;
    private readonly IRegionManager _regionManager;

    public PackageModule(ILogger logger, IRegionManager regionManager)
    {
        _logger = logger;
        _regionManager = regionManager;
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
        _regionManager.RegisterViewWithRegion("PackageContentRegion", typeof(PackageView));
        _regionManager.RegisterViewWithRegion("DotNugetPackageContentRegion", typeof(DotNugetPackageListView));
        _regionManager.RegisterViewWithRegion("PackageListContentRegion", typeof(PackageListView));
        
        //TODO: Update this implementation if there is a better way for first navigation
        IRegion region = _regionManager.Regions["PackageContentRegion"];
        region.RequestNavigate(nameof(PackageView));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<PackageView>();
        containerRegistry.RegisterForNavigation<DotNugetPackageListView>();
        containerRegistry.RegisterForNavigation<PackageListView>();
       
        ViewModelLocationProvider.Register<PackageView, PackageViewModel>();
        ViewModelLocationProvider.Register<DotNugetPackageListView, DotNugetPackageListViewModel>();
        ViewModelLocationProvider.Register<PackageListView, PackageListViewModel>();
    }
}
