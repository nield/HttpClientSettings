using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientSettings.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddHttpClientSettings_ShouldAddSettingsToConfig()
    {
        var services = Substitute.For<IServiceCollection>();
        var config = Substitute.For<IConfiguration>();

        services.AddHttpClientSettings(config);

        services.Received().Configure<HttpClientAppSettings>(config);
    }
}
