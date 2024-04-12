namespace MainModule
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
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainView>();
           
            ViewModelLocationProvider.Register<MainView, MainViewModel>();
        }
    }
}
