namespace CommonModule.Events.Payloads;

public class LogEventArgs : EventArgs
{
    public required string LogMessage { get; set; }
}
