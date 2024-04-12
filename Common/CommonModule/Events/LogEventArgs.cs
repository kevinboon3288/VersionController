namespace CommonModule.Events;

public class LogEventArgs: EventArgs
{
    public required string LogMessage { get; set; }
}
