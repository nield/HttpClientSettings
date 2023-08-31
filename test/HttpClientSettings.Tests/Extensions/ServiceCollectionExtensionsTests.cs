using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientSettings.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    private readonly HttpClientAppSettings _settings = new();

    [Fact]
    public void AddHttpClientSettings_ShouldAddSettingsToConfig()
    {
        var services = Substitute.For<IServiceCollection>();
        var config = Substitute.For<IConfiguration>();

        services.AddHttpClientSettings(config);

        services.Received().Configure<HttpClientAppSettings>(config);
    }

    [Fact]
    public void ValidateHttpClientAppSettings_GivenInvalidAppSettings_ShouldThrowException()
    {
        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, "")
             .Build();

        _settings.LoadClientsForUnitTesting(clients);

        Assert.Throws<InvalidAppSettingsException>(() => ServiceCollectionExtensions.ValidateHttpClientAppSettings(_settings));
    }
}
