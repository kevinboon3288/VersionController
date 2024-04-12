namespace PackageModule.ViewModels;

public class PackageViewModel : BindableBase, INavigationAware
{
    private readonly ILogger _logger;
    private readonly IRegionManager _regionManager;

    public PackageViewModel(ILogger logger, IRegionManager regionManager)
    {
        _logger = logger;
        _regionManager = regionManager;
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext != null) 
        {
            OnNavigateToDotNugetPackage();
            OnNavigateToPackageList();
        }
    }

    private void OnNavigateToDotNugetPackage() 
    {
        IRegion region = _regionManager.Regions["DotNugetPackageContentRegion"];
        region.RequestNavigate(nameof(DotNugetPackageListView));
    }        
    
    private void OnNavigateToPackageList() 
    {
        IRegion region = _regionManager.Regions["PackageListContentRegion"];
        region.RequestNavigate(nameof(PackageListView));
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }
}
