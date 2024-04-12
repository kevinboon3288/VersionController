namespace PackageModule.ViewModels;

public class DotNugetPackageListViewModel : BindableBase, INavigationAware
{
    private readonly ILogger _logger;
    private readonly IDirectoryUtils _directoryUtils;

    private ObservableCollection<Package> _dotNuGetPackages = new();
    private Package _selectedDotNugetPackage = new();
    private bool _isVisible = false;
    private bool _isAllChecked = false;
    private string _dotNugetPackageToken = string.Empty;

    public ObservableCollection<Package> DotNuGetPackages
    {
        get => _dotNuGetPackages;
        set { SetProperty(ref _dotNuGetPackages, value); }
    }

    public string DotNugetPackageToken
    {
        get => _dotNugetPackageToken;
        set
        {
            SetProperty(ref _dotNugetPackageToken, value);
            if (string.IsNullOrEmpty(_dotNugetPackageToken))
            {
                Refresh();
            }
        }
    }

    public Package SelectedDotNuGetPackage
    {
        get => _selectedDotNugetPackage;
        set
        {
            SetProperty(ref _selectedDotNugetPackage, value);
            if (_dotNuGetPackages != null)
            {
                Package? package = DotNuGetPackages.ToList().FirstOrDefault(x => x.Name == _selectedDotNugetPackage.Name);
                if (package != null)
                {
                    package.IsChecked = !_selectedDotNugetPackage.IsChecked;
                }
            }

            IsVisible = DotNuGetPackages.Any(x => x.IsChecked);
        }
    }

    public bool IsAllChecked
    {
        get => _isAllChecked;
        set
        {
            SetProperty(ref _isAllChecked, value);
            DotNuGetPackages.ToList().ForEach(x => x.IsChecked = _isAllChecked);
            IsVisible = DotNuGetPackages.Any(x => x.IsChecked);
        }
    }

    public bool IsVisible
    {
        get => _isVisible;
        set { SetProperty(ref _isVisible, value); }
    }
    public DelegateCommand<string> DotNugetSearchCommand { get; set; }
    public DelegateCommand DeleteCommand { get; set; }
    public DelegateCommand PublishCommand { get; set; }

    public DotNugetPackageListViewModel(ILogger logger, IDirectoryUtils directoryUtils)
    {
        _logger = logger;
        _directoryUtils = directoryUtils;

        DotNugetSearchCommand = new DelegateCommand<string>(OnDotNugetSearch);
        DeleteCommand = new DelegateCommand(OnDelete);
        PublishCommand = new DelegateCommand(OnPublish);
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        Refresh();
    }

    private void Refresh() 
    {
        IsVisible = false;
        DotNuGetPackages.Clear();

        List<(string, string?)> dotNuGetPackages = _directoryUtils.GetDotNugetPackages();

        foreach ((string package, string? version) in dotNuGetPackages)
        {
            DotNuGetPackages.Add(new Package(package, version));
        }
    }

    private void OnDotNugetSearch(string token) 
    {
        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        DotNuGetPackages.Clear();

        List<(string, string?)> filterPackages = _directoryUtils.GetDotNugetFilterPackages(token);

        foreach ((string filterPackage, string? version) in filterPackages)
        {
            DotNuGetPackages.Add(new Package(filterPackage, version));
        }
    }

    private void OnDelete()
    {
        if (!DotNuGetPackages.Any(x => x.IsChecked))
        {
            _logger.Warning("Unable to delete the packages due to no package is selected.");
            return;
        }

        List<string> deletePackages = new();

        foreach (Package package in DotNuGetPackages)
        {
            if (package.IsChecked)
            {
                deletePackages.Add(package.Name);
            }
        }

        _directoryUtils.Delete(deletePackages, ConstantFilePaths.DotNugetFilePath);

        Refresh();
    }

    private void OnPublish()
    {
        //TODO: Add logic to OnPublish method
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }
}
