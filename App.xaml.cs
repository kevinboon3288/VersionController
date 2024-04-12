using CommonModule.Core;
using PackageModule.Core;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using Serilog;
using Serilog.Core;
using System.Windows;
using Unity;

namespace VersionController
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private IRegionManager _regionManager;

        protected override Window CreateShell()
        {
            return Container.Resolve<VersionControllerWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //TODO: Register interface and its class
            containerRegistry.Register<IDirectoryUtils, DirectoryUtils>();

            IEventAggregator eventAggregator = containerRegistry.GetContainer().Resolve<IEventAggregator>();
            ILogEventSink logControlSink = new LogSink(eventAggregator);

            containerRegistry.RegisterInstance<ILogger>(new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Sink(logControlSink, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger());

            _regionManager = containerRegistry.GetContainer().Resolve<IRegionManager>();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModule.MainModule>();
            moduleCatalog.AddModule<PackageModule.PackageModule>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Flush all Serilog sinks before the app closes
            Log.CloseAndFlush();

            base.OnExit(e);
        }
    }
}
