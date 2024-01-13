using System;

namespace VersionController.Services.Events
{
    public class LogEventArgs: EventArgs
    {
        public string LogMessage { get; set; }
    }
}
