using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HttpClientSettings
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClientSettings(this IServiceCollection services, 
            IConfiguration configuration)
        {
            var section = configuration.GetRequiredSection(Constants.AppSettings.SectionName);

            services.Configure<HttpClientAppSettings>(section);

            return services;
        }

        public static IApplicationBuilder ValidateHttpClientSettings(this IApplicationBuilder applicationBuilder)
        {
            var httpClientSettings = applicationBuilder                    
                                        .ApplicationServices
                                        .GetRequiredService<IOptions<HttpClientAppSettings>>();

            var validator = new HttpClientAppSettingsValidator(httpClientSettings.Value);

            var validationResponse = validator.Validate();

            if (!validationResponse.IsSuccess) throw new InvalidAppSettingsException(validationResponse.Errors);
            
            return applicationBuilder;
        }
    }
}
