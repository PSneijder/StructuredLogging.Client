using System;
using System.Configuration;
using log4net.Appender;
using log4net.Core;
using StructuredLogging.Client.Log4Net.Extensions;
using StructuredLogging.Client.Models;

namespace StructuredLogging.Client.Log4Net
{
    public sealed class StructuredLoggingAppender
        : BufferingAppenderSkeleton
    {
        private StructuredLoggingClient _client;
        private string _serverUrl;

        public string ServerUrl
        {
            get
            {
                return _serverUrl;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value) && Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
                {
                    throw new ConfigurationErrorsException($"Configuration ServerUrl = '{value}' is not valid.");
                }

                _serverUrl = value;

                if (_client == null)
                {
                    _client = new StructuredLoggingClient(value);
                }
            }
        }
        
        protected override void SendBuffer(LoggingEvent[] events)
        {
            RawEvents rawEvents = events.ToRawEvents();

            try
            {
                _client.CreateAsync(rawEvents).GetAwaiter();
            }
            catch (Exception ex)
            {
                ErrorHandler.Error(ex.Message);
            }
        }
    }
}