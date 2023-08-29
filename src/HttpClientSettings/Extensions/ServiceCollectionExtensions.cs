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

            services.Configure<HttpClientAppSettings>(section)
                .PostConfigure<HttpClientAppSettings>(config =>
                {
                    if (validateSettings)
                    {
                        var validator = new HttpClientAppSettingsValidator(config);
                        validator.Validate();
                    }
                });

            return services;
        }
    }
}
