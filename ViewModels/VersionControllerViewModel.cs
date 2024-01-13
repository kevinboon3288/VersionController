using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Serilog;
using System;
using VersionController.Services.Events;

namespace VersionController.ViewModels
{
    public class VersionControllerViewModel : BindableBase
    {
        private readonly ILogger _logger;
        private readonly IEventAggregator _eventAggregator;

        public event EventHandler LogReceived;

        public VersionControllerViewModel(ILogger logger, IEventAggregator eventAggregator)
        {
            _logger = logger;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<LogEvent>().Subscribe(OnLogEventReceived);
        }

        private void OnLogEventReceived(string message) 
        {
            LogEventArgs eventArgs = new()
            { 
                LogMessage = message
            };

            LogReceived?.Invoke(this, eventArgs);
        }
    }
}
