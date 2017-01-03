using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using StructuredLogging.Client.Models;

namespace StructuredLogging.Client
{
    sealed class WebApiClient
        : HttpClient
    {
        public WebApiClient(string baseUrl)
        {
            BaseAddress = new Uri(baseUrl);
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.HeaderMediaTypeJson));
        }
    }

    public sealed class StructuredLoggingClient
    {
        private readonly string _baseUrl;

        public StructuredLoggingClient(string baseUrl)
        {
            _baseUrl = baseUrl;

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public async void CreateAsync(RawEvent rawEvent)
        {
            // url: /api/event/
            string url = $"{_baseUrl}/{Constants.RoutePrefix}/{Constants.RouteEvent}";

            string jsonString = JsonConvert.SerializeObject(rawEvent);
            StringContent content = new StringContent(jsonString, Encoding.UTF8, Constants.HeaderMediaTypeJson);

            using (WebApiClient client = new WebApiClient(_baseUrl))
            {
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string messageString = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException(messageString);
                }
            }
        }

        public async void CreateAsync(RawEvents rawEvents)
        {
            // url: /api/events/
            string url = $"{_baseUrl}/{Constants.RoutePrefix}/{Constants.RouteEvents}";

            string jsonString = JsonConvert.SerializeObject(rawEvents);
            StringContent content = new StringContent(jsonString, Encoding.UTF8, Constants.HeaderMediaTypeJson);

            using (WebApiClient client = new WebApiClient(_baseUrl))
            { 
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string messageString = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException(messageString);
                }
            }
        }
    }
}