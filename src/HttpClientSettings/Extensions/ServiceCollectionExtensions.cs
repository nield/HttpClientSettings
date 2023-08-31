using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientSettings
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClientSettings(this IServiceCollection services, 
            IConfiguration configuration, bool validateSettings = true)
        {
            var section = configuration.GetRequiredSection(Constants.AppSettings.SectionName);

            services.AddOptions<HttpClientAppSettings>()
                .Bind(section)
                .Validate(httpClientAppSettings =>
                {
                    if (validateSettings) ValidateHttpClientAppSettings(httpClientAppSettings);

                    return true;
                });

            return services;
        }

        internal static void ValidateHttpClientAppSettings(HttpClientAppSettings httpClientAppSettings)
        {
            var validator = new HttpClientAppSettingsValidator(httpClientAppSettings);

            var validationResponse = validator.Validate();

            if (!validationResponse.IsSuccess)
            {
                throw new InvalidAppSettingsException(validationResponse.Errors);
            }
        }
    }
}
