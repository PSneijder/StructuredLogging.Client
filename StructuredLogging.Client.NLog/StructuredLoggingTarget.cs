using System;
using NLog;
using NLog.Targets;
using System.Configuration;
using System.Net;
using StructuredLogging.Client.Models;
using StructuredLogging.Client.NLog.Extensions;

namespace StructuredLogging.Client.NLog
{
    [Target("StructuredLogging")]
    public sealed class StructuredLoggingTarget
        : Target
    {
        private string _serverUrl;
        private StructuredLoggingClient _client;

        public string ServerUrl
        {
            get
            {
                return _serverUrl;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value) || !Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
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

        protected override void Write(LogEventInfo logEvent)
        {
            RawEvent rawEvent = logEvent.ToRawEvents();

            try
            {
                _client.CreateAsync(rawEvent).GetAwaiter();
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message, ex);
            }
        }
    }
}