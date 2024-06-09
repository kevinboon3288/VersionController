using CommonModule.Events;

namespace SettingsModule.ViewModels;

public class SettingsProjectViewModel : BindableBase, INavigationAware
{
    private readonly ILogger _logger;
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;
    private string? _packageCmdFilePath;
    private string? _inputCmdFilePath;

    public string? PackageCmdFilePath
    {
        get => _packageCmdFilePath;
        set
        {
            SetProperty(ref _packageCmdFilePath, value);
        }
    }

    public DelegateCommand SaveCommand { get; private set; }
    public DelegateCommand CancelCommand { get; private set; }

    public SettingsProjectViewModel(ILogger logger, IRegionManager regionManager, IEventAggregator eventAggregator)
    {
        _logger = logger;
        _regionManager = regionManager;
        _eventAggregator = eventAggregator;

        SaveCommand = new DelegateCommand(OnSave);
        CancelCommand = new DelegateCommand(OnCancel);
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

    private void OnSave() 
    {
        if (string.IsNullOrEmpty(PackageCmdFilePath)) 
        {
            _logger.Error("Package CMD file path is null or empty.");
            return;
        }

        _inputCmdFilePath = PackageCmdFilePath;

        _eventAggregator.GetEvent<UIControlEvents>().Publish(new Dictionary<string, dynamic>() 
        {
            { nameof(PackageCmdFilePath), PackageCmdFilePath }
        });

        _logger.Information($"Package CMD file path save successfully: [{PackageCmdFilePath}]");
    }

    private void OnCancel()
    {
        IRegion region = _regionManager.Regions["MainContentRegion"];
        region.RequestNavigate("MainView");
    }
}
