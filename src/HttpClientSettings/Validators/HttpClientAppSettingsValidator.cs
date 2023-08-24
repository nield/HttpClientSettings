using System;
using System.Collections.Generic;

namespace HttpClientSettings
{
    internal class HttpClientAppSettingsValidator
    {
        private readonly HttpClientAppSettings _clientSettings;

        public HttpClientAppSettingsValidator(HttpClientAppSettings settings)
        {
            _clientSettings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public HttpClientAppSettingsValidationResponse Validate()
        {
            var response = new HttpClientAppSettingsValidationResponse();

            foreach (var client in _clientSettings.Clients)
            {
                ValidateClientSettings(client, response);
                ValidateEndPointSettings(client.Name, client.Endpoints, response);
            }
           
            return response;
        }

        private void ValidateClientSettings(HttpClientSetting httpClientSetting,
            HttpClientAppSettingsValidationResponse response)
        {
            if (string.IsNullOrWhiteSpace(httpClientSetting.Name))
            {
                response.Errors.Add($"{nameof(HttpClientSetting.Name)} is required");
            }

            if (string.IsNullOrWhiteSpace(httpClientSetting.BaseUri))
            {
                response.Errors.Add($"ClientName: '{httpClientSetting.Name}', {nameof(HttpClientSetting.BaseUri)} is required");
            }
            else if (Uri.IsWellFormedUriString(httpClientSetting.BaseUri, UriKind.Relative)) 
            {
                response.Errors.Add($"ClientName: '{httpClientSetting.Name}', {nameof(HttpClientSetting.BaseUri)} is not a valid uri");
            }

            if (httpClientSetting.Endpoints.Count == 0)
            {
                response.Errors.Add($"ClientName: '{httpClientSetting.Name}', {nameof(HttpClientSetting.Endpoints)} is required");
            }
        }

        private void ValidateEndPointSettings(string httpClientName, 
            IReadOnlyList<EndpointSettings> endpointSettings,
            HttpClientAppSettingsValidationResponse response)
        {
            if (endpointSettings.Count <= 0) return;

            foreach (var endpoint in endpointSettings)
            {
                ValidateEndpoint(httpClientName, endpoint, response);
            }
        }

        private void ValidateEndpoint(string httpClientName,
            EndpointSettings endpoint,
            HttpClientAppSettingsValidationResponse response)
        {
            if (string.IsNullOrWhiteSpace(endpoint.Name))
            {
                response.Errors.Add($"ClientName: '{httpClientName}', Endpoint {nameof(EndpointSettings.Name)} is required");
            }

            if (string.IsNullOrWhiteSpace(endpoint.Uri))
            {
                response.Errors.Add($"ClientName: '{httpClientName}', Endpoint {nameof(EndpointSettings.Uri)} is required");
            }
        }
    }

    internal class HttpClientAppSettingsValidationResponse
    {
        public bool IsSuccess => Errors.Count <= 0;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
