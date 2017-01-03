using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using StructuredLogging.Client.Models;

namespace StructuredLogging.Client.Sample
{
    class Program
    {
        static void Main()
        {
            var rawEvent = new RawEvent
            {
                Timestamp = new DateTime(2017, 1, 1, 15, 12, 38).ToString("O"),
                Level = "Verbose",
                MessageTemplate = "CPU usage is {CpuPercentage:P2} on {MachineName}",
                Properties = new Dictionary<string, object>
                {
                    { "CpuPercentage", 0.18 },
                    { "MachineName", "nbherb-rmbp" }
                }
            };

            var baseUrl = GetSetting("baseUrl", "http://localhost:80");
            var client = new StructuredLoggingClient(baseUrl);

            try
            {
                Console.WriteLine("Posting dummy event.");
                client.CreateAsync(rawEvent).GetAwaiter();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Press ANY key to quit.");
            Console.ReadKey();
        }

        private static T GetSetting<T>(string key, T defaultValue)
        {
            var settings = ConfigurationManager.AppSettings;

            if (!settings.AllKeys.Contains(key))
            {
                return defaultValue;
            }

            var setting = settings[key];
            T value;

            try
            {
                value = (T) Convert.ChangeType(setting, typeof(T));
            }
            catch (Exception)
            {
                return defaultValue;
            }

            return value;
        }
    }
}