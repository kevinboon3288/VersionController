using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using System.Windows;
using VersionController.ViewModels;

namespace VersionController
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //TODO: Register interface and its class
            //containerRegistry.Register<IDirectoryUtils, DirectoryUtils>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<VersionControllerWindow>();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            //TODO: Register New Added View and ViewModel at ViewModelLocationProvider
            ViewModelLocationProvider.Register<VersionControllerWindow, VersionControllerViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //moduleCatalog.AddModule<VersionControllerMainView>();
        }
    }
}
