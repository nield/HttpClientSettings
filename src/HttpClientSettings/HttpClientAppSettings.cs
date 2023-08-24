using System;
using System.Collections.Generic;
using System.Linq;

namespace HttpClientSettings
{
    public class HttpClientAppSettings
    {
        public IReadOnlyList<HttpClientSetting> Clients { get; set; } = new List<HttpClientSetting>();

        public HttpClientSetting GetClientSettings(string clientName) =>
            Clients.FirstOrDefault(x => x.Name == clientName) 
                ?? throw new ClientNotFoundException(clientName);

        public Endpoint GetEndpoint(string clientName, string endpointName, params object[] parameters)
        {
            var clientSettings = GetClientSettings(clientName);
            var endpointSettings = clientSettings.GetEndpoint(endpointName);

            var uri = GetUriWithReplacedParameters(endpointSettings.Uri, parameters);

            return new Endpoint
            {
                BaseUri = clientSettings.BaseUri,
                Uri = uri,
                Name = endpointSettings.Name
            };
        }

        private static string GetUriWithReplacedParameters(string uri, object[] parameters) =>        
            parameters.Length > 0 ? string.Format(uri, parameters) : uri;

        internal void LoadClientsForUnitTesting(IList<HttpClientSetting> clients) =>
            Clients = new List<HttpClientSetting>(clients);
    }

    public class HttpClientSetting
    {
        public string Name { get; set; } = "";
        public string BaseUri { get; set; } = "";
        public IReadOnlyList<EndpointSettings> Endpoints { get; set; } = new List<EndpointSettings>();

        public EndpointSettings GetEndpoint(string endpointName) =>
            Endpoints.FirstOrDefault(x => x.Name == endpointName)
                ?? throw new EndpointNotFoundException(endpointName);
    }

    public class EndpointSettings
    {
        public string Uri { get; set; } = "";
        public string Name { get; set; } = "";
    }

    public class Endpoint 
    {
        public string BaseUri { get; set; } = "";
        public string Uri { get; set; } = "";
        public string Name { get; set; } = "";

        public Uri FullUri => new Uri(new Uri(BaseUri), Uri);
    }
}
