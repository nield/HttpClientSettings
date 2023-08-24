namespace HttpClientSettings.Tests;

public class HttpClientAppSettingsTests
{
    private readonly HttpClientAppSettings _settings = new();

    private const string _defaultClientName = "testClient";
    private const string _defaultEndpointName = "testEndpoint";

    [Fact]
    public void GetClientSettings_WithNonExistingClientName_ShouldThrowException()
    {
        Assert.Throws<ClientNotFoundException>(() => _settings.GetClientSettings("invalid"));
    }

    [Fact]
    public void GetClientSettings_WithExistingClientName_ShouldReturnTheClientSettings()
    {
        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, _defaultClientName)
             .Build();

        _settings.LoadClientsForUnitTesting(clients);

        var sut = _settings.GetClientSettings(_defaultClientName);

        sut.Should().NotBeNull();
        sut.Name.Should().Be(_defaultClientName);
    }

    [Fact]
    public void GetEndpoint_WithNonExistingEndpointName_ShouldReturnThrowException()
    {
        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, _defaultClientName)
             .Build();

        _settings.LoadClientsForUnitTesting(clients);

        Assert.Throws<EndpointNotFoundException>(() => _settings.GetEndpoint(_defaultClientName, "invalid"));
    }

    [Fact]
    public void GetEndpoint_WithExistingEndpointName_ShouldReturnEndpoint()
    {
        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, _defaultClientName)
             .With(x => x.Endpoints, Builder<EndpointSettings>.CreateListOfSize(1)
                                        .All()
                                        .With(x => x.Name, _defaultEndpointName)
                                        .Build()
                                        .ToList())            
             .Build();

        _settings.LoadClientsForUnitTesting(clients);

        var sut = _settings.GetEndpoint(_defaultClientName, _defaultEndpointName);

        sut.Should().NotBeNull();

        sut.Name.Should().Be(_defaultEndpointName);        
    }

    [Fact]
    public void GetEndpoint_WithExistingEndpointNameAndParameters_ShouldReturnEndpointWithReplacedUriParameters()
    {
        var param1 = "data";
        var param2 = 10;

        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, _defaultClientName)
             .With(x => x.Endpoints, Builder<EndpointSettings>.CreateListOfSize(1)
                                        .All()
                                        .With(x => x.Name, _defaultEndpointName)
                                        .With(x => x.Uri, "/api/test/{0}?page={1}")
                                        .Build()
                                        .ToList())
             .Build();

        _settings.LoadClientsForUnitTesting(clients);

        var sut = _settings.GetEndpoint(_defaultClientName, _defaultEndpointName, param1, param2);

        sut.Should().NotBeNull();

        sut.Name.Should().Be(_defaultEndpointName);
        sut.Uri.Should().Be($"/api/test/{param1}?page={param2}");
    }

    [Theory]
    [InlineData(@"http://localhost", @"api/v1/test", @"http://localhost/api/v1/test")]
    [InlineData(@"http://localhost/", @"/api/v1/test", @"http://localhost/api/v1/test")]
    [InlineData(@"http://localhost/", @"api/v1/test", @"http://localhost/api/v1/test")]
    [InlineData(@"http://localhost", @"/api/v1/test", @"http://localhost/api/v1/test")]
    public void GetEndpoint_WithExistingEndpointNameAndUriWithTrailingAndLeadingSlash_ShouldReturnEndpointWithUri(
        string clientUri, string endpointUri, string expectedFullUri)
    {
        var param1 = "data";
        var param2 = 10;

        var clients = Builder<HttpClientSetting>.CreateListOfSize(1)
             .All()
             .With(x => x.Name, _defaultClientName)
             .With(x => x.BaseUri, clientUri)
             .With(x => x.Endpoints, Builder<EndpointSettings>.CreateListOfSize(1)
                                        .All()
                                        .With(x => x.Name, _defaultEndpointName)
                                        .With(x => x.Uri, endpointUri)
                                        .Build()
                                        .ToList())
             .Build();

        _settings.LoadClientsForUnitTesting(clients);

        var sut = _settings.GetEndpoint(_defaultClientName, _defaultEndpointName, param1, param2);

        sut.Should().NotBeNull();

        sut.Name.Should().Be(_defaultEndpointName);
        sut.FullUri.ToString().Should().Be(expectedFullUri);   
    }
}
