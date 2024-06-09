using CommonModule.Events;
using Prism.Events;

namespace PackageModule.ViewModels;

public class PackageViewModel : BindableBase, INavigationAware
{
    private readonly ILogger _logger;
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;
    private readonly IDirectoryUtils _directoryUtils;
    private bool _isVisible;
    private string? _inputFilePath;

    public bool IsVisible
    {
        get => _isVisible;
        set { SetProperty(ref _isVisible, value); }
    }

    public DelegateCommand PublishCommand { get; private set; }

    public PackageViewModel(ILogger logger, IRegionManager regionManager, IEventAggregator eventAggregator, IDirectoryUtils directoryUtils)
    {
        _logger = logger;
        _regionManager = regionManager;
        _eventAggregator = eventAggregator;
        _directoryUtils = directoryUtils;

        PublishCommand = new DelegateCommand(OnPublish);

        _eventAggregator.GetEvent<UIControlEvents>().Subscribe(OnReceived);
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

    private void OnPublish() 
    {
        try
        {
            if (!string.IsNullOrEmpty(_inputFilePath)) 
            {
                _directoryUtils.Publish(_inputFilePath, "publish_local_packages.cmd");
            }
        }
        catch (Exception ex) 
        {
            _logger.Error($"Unable to publish: {ex.Message}");
        }
    }

    private void OnReceived(Dictionary<string, dynamic> eventArgs) 
    {
        if (eventArgs == null) 
        {
            _logger.Error($"Null event arguments: [{nameof(UIControlEvents)}]");
            return;
        }

        if (eventArgs.TryGetValue("PackageCmdFilePath", out dynamic? filePath)) 
        {
            IsVisible = !string.IsNullOrEmpty(filePath);
            _inputFilePath = filePath;
        }
    }
}
