using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HttpClientSettings.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    private const string _defaultClientName = "testClient";
    private const string _defaultEndpointName = "testEndpoint";

    [Fact]
    public void AddHttpClientSettings_ShouldAddSettingsToConfig()
    {
        var services = Substitute.For<IServiceCollection>();
        var config = Substitute.For<IConfiguration>();

        services.AddHttpClientSettings(config);

        services.Received().Configure<HttpClientAppSettings>(config);
    }

    [Fact]
    public void ValidateHttpClientSettings_WithInvalidValidationResponse_ShouldThrowException()
    {
        var appBuilder = Substitute.For<IApplicationBuilder>();

        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, _defaultClientName)
             .Build();

        var settings = new HttpClientAppSettings();
        settings.LoadClientsForUnitTesting(clients);

        appBuilder.ApplicationServices.GetService<IOptions<HttpClientAppSettings>>()
            .Returns(Options.Create(settings));

        Assert.Throws<InvalidAppSettingsException>(() => appBuilder.ValidateHttpClientSettings());
    }

    [Fact]
    public void ValidateHttpClientSettings_WithValidValidationResponse_ShouldNotThrowException()
    {
        var appBuilder = Substitute.For<IApplicationBuilder>();

        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, _defaultClientName)
             .With(x => x.BaseUri, "http://localhost")
             .With(x => x.Endpoints, Builder<EndpointSettings>.CreateListOfSize(1)
                                        .All()
                                        .With(x => x.Name, _defaultEndpointName)
                                        .With(x => x.Uri, "api/test")
                                        .Build()
                                        .ToList())
             .Build();

        var settings = new HttpClientAppSettings();
        settings.LoadClientsForUnitTesting(clients);

        appBuilder.ApplicationServices.GetService<IOptions<HttpClientAppSettings>>()
            .Returns(Options.Create(settings));

        var sut = appBuilder.ValidateHttpClientSettings();

        sut.Should().NotBeNull();
    }
}
