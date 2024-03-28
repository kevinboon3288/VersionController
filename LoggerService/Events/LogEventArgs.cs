using System;

namespace VersionController.Services.Events
{
    public class LogEventArgs: EventArgs
    {
        public required string LogMessage { get; set; }
    }
}
