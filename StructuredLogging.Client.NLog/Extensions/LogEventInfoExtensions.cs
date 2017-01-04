using System.Collections.Generic;
using NLog;
using StructuredLogging.Client.Models;

namespace StructuredLogging.Client.NLog.Extensions
{
    static class LogEventInfoExtensions
    {
        private static readonly IDictionary<string, string> LevelMap = new Dictionary<string, string>
        {
            { "Trace", "Verbose" },
            { "Debug", "Debug" },
            { "Info", "Information" },
            { "Warn", "Warning" },
            { "Error", "Error" },
            { "Fatal", "Fatal" }
        };

        public static RawEvent ToRawEvents(this LogEventInfo eventInfo)
        {
            var rawEvent = new RawEvent();

            string level;
            if (!LevelMap.TryGetValue(eventInfo.Level.Name, out level))
            {
                level = "Information";
            }

            rawEvent.Level = level;
            rawEvent.Timestamp = eventInfo.TimeStamp.ToString("O");
            rawEvent.MessageTemplate = eventInfo.FormattedMessage;

            if (eventInfo.Exception != null)
            {
                rawEvent.Exception = eventInfo.Exception.ToString();
            }
            
            return rawEvent;
        }
    }
}