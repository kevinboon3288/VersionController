using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Serilog;
using VersionController.PackageModule.ViewModels;
using VersionController.PackageModule.Views;

namespace VersionController.PackageModule
{
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
            _regionManager.RegisterViewWithRegion("PackageListContentRegion", typeof(PackageListView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PackageListView>();
           
            ViewModelLocationProvider.Register<PackageListView, PackageListViewModel>();
        }
    }
}
