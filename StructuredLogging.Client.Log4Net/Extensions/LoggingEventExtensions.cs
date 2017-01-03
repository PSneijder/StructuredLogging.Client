using System.Collections.Generic;
using log4net.Core;
using StructuredLogging.Client.Models;

namespace StructuredLogging.Client.Log4Net.Extensions
{
    static class LoggingEventExtensions
    {
        private static readonly IDictionary<string, string> LevelMap = new Dictionary<string, string>
        {
            { "DEBUG", "Debug" },
            { "INFO", "Information" },
            { "WARN", "Warning" },
            { "ERROR", "Error" },
            { "FATAL", "Fatal" }
        };

        public static RawEvents ToRawEvents(this LoggingEvent[] events)
        {
            var rawEvents = new List<RawEvent>();

            foreach (LoggingEvent loggingEvent in events)
            {
                var rawEvent = new RawEvent();

                string level;
                if (!LevelMap.TryGetValue(loggingEvent.Level.Name, out level))
                {
                    level = "Information";
                }

                rawEvent.Level = level;
                rawEvent.Timestamp = loggingEvent.TimeStamp.ToString("O");
                rawEvent.MessageTemplate = loggingEvent.RenderedMessage;

                if (loggingEvent.ExceptionObject != null)
                { 
                    rawEvent.Exception = loggingEvent.ExceptionObject.ToString();
                }

                rawEvents.Add(rawEvent);
            }

            return new RawEvents { Events = rawEvents.ToArray() };
        }
    }
}