using Prism.Events;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using System;
using System.IO;

namespace VersionController.Services
{
    public class LogSink : ILogEventSink
    {
        private readonly IEventAggregator _eventAggregator;

        public LogSink(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Emit(LogEvent logEvent)
        {
            ArgumentNullException.ThrowIfNull(logEvent);

            StringWriter strWriter = new();

            MessageTemplateTextFormatter textFormatter = new MessageTemplateTextFormatter("{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}]: {Message}{Exception}");
            textFormatter.Format(logEvent, strWriter);

            _eventAggregator.GetEvent<Events.LogEvent>().Publish(strWriter.ToString());
        }
    }
}
