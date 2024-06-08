using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainModule.ViewModels;

public class HeaderViewModel : BindableBase, INavigationAware
{
    private readonly ILogger _logger;
    private readonly IRegionManager _regionManager;

    public DelegateCommand NavigateToSettingsCommand {  get; set; }


    public HeaderViewModel(ILogger logger, IRegionManager regionManager)
    {
        _logger = logger;
        _regionManager = regionManager;

        NavigateToSettingsCommand = new DelegateCommand(OnNavigateToSettings);
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

    private void OnNavigateToSettings() 
    {
        IRegion region = _regionManager.Regions["MainContentRegion"];
        region.RequestNavigate("SettingsView");
    }
}
