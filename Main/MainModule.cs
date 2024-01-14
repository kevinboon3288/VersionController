using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Serilog;
using VersionController.Main.ViewModels;
using VersionController.Main.Views;
using VersionController.PackageModule.ViewModels;
using VersionController.PackageModule.Views;

namespace VersionController.Main
{
    public class MainModule : IModule
    {
        private readonly ILogger _logger;
        private readonly IRegionManager _regionManager;

        public MainModule(ILogger logger, IRegionManager regionManager)
        {
            _logger = logger;
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("MainContentRegion", typeof(MainView));
            //_regionManager.RegisterViewWithRegion("PackageListContentRegion", typeof(PackageView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainView>();
            //containerRegistry.RegisterForNavigation<PackageView>();
           
            ViewModelLocationProvider.Register<MainView, MainViewModel>();
            //ViewModelLocationProvider.Register<PackageView, PackageListViewModel>();
        }
    }
}
