using System.Net;

namespace HttpClientSettings.Tests.Validators;

public class HttpClientAppSettingsValidatorTests
{
    private readonly HttpClientAppSettings _settings = new();

    private const string _defaultClientName = "testClient";
    private const string _defaultEndpointName = "testEndpoint";

    private HttpClientAppSettingsValidator _validator;

    [Fact]
    public void Constructor_GivenNullSettings_ShouldThrowException()
    {
        var sut = Assert.Throws<ArgumentNullException>(() => new HttpClientAppSettingsValidator(null));
        
        sut.ParamName.Should().Be("settings");
    }

    [Fact]
    public void Validate_GivenEmptyClientName_ShouldReturnErrors()
    {
        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, "")
             .Build();

        _settings.LoadClientsForUnitTesting(clients);

        _validator = new(_settings);

        var sut = _validator.Validate();

        sut.IsSuccess.Should().BeFalse();
        sut.Errors.Should().Contain($"{nameof(HttpClientSetting.Name)} is required");
    }

    [Fact]
    public void Validate_GivenEmptyClientBaseUri_ShouldReturnErrors()
    {
        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, _defaultClientName)
             .With(x => x.BaseUri, "")
             .Build();

        _settings.LoadClientsForUnitTesting(clients);

        _validator = new(_settings);

        var sut = _validator.Validate();

        sut.IsSuccess.Should().BeFalse();
        sut.Errors.Should().Contain($"ClientName: '{_defaultClientName}', {nameof(HttpClientSetting.BaseUri)} is required");
    }

    [Fact]
    public void Validate_GivenInvalidClientBaseUri_ShouldReturnErrors()
    {
        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, _defaultClientName)
             .With(x => x.BaseUri, "localhost")
             .Build();

        _settings.LoadClientsForUnitTesting(clients);

        _validator = new(_settings);

        var sut = _validator.Validate();

        sut.IsSuccess.Should().BeFalse();
        sut.Errors.Should().Contain($"ClientName: '{_defaultClientName}', {nameof(HttpClientSetting.BaseUri)} is not a valid uri");
    }

    [Fact]
    public void Validate_GivenClientWithEmptyEndpoints_ShouldReturnErrors()
    {
        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, _defaultClientName)
             .With(x => x.BaseUri, "http://localhost")
             .Build();

        _settings.LoadClientsForUnitTesting(clients);

        _validator = new(_settings);

        var sut = _validator.Validate();

        sut.IsSuccess.Should().BeFalse();
        sut.Errors.Should().Contain($"ClientName: '{_defaultClientName}', {nameof(HttpClientSetting.Endpoints)} is required");
    }

    [Fact]
    public void Validate_GivenEmptyEndpointName_ShouldReturnErrors()
    {
        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, _defaultClientName)
             .With(x => x.BaseUri, "http://localhost")
             .With(x => x.Endpoints, Builder<EndpointSettings>.CreateListOfSize(1)
                                        .All()
                                        .With(x => x.Name, "")
                                        .Build()
                                        .ToList())
             .Build();

        _settings.LoadClientsForUnitTesting(clients);

        _validator = new(_settings);

        var sut = _validator.Validate();

        sut.IsSuccess.Should().BeFalse();
        sut.Errors.Should().Contain($"ClientName: '{_defaultClientName}', Endpoint {nameof(EndpointSettings.Name)} is required");
    }

    [Fact]
    public void Validate_GivenEmptyEndpointUri_ShouldReturnErrors()
    {
        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, _defaultClientName)
             .With(x => x.BaseUri, "http://localhost")
             .With(x => x.Endpoints, Builder<EndpointSettings>.CreateListOfSize(1)
                                        .All()
                                        .With(x => x.Name, _defaultEndpointName)
                                        .With(x => x.Uri, "")
                                        .Build()
                                        .ToList())
             .Build();

        _settings.LoadClientsForUnitTesting(clients);

        _validator = new(_settings);

        var sut = _validator.Validate();

        sut.IsSuccess.Should().BeFalse();
        sut.Errors.Should().Contain($"ClientName: '{_defaultClientName}', Endpoint {nameof(EndpointSettings.Uri)} is required");
    }
}
