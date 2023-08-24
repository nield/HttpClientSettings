namespace HttpClientSettings.Tests.Exceptions;

public class EndpointNotFoundExceptionTests
{
    [Fact]
    public void Given_ExceptionIsThrown_ShouldReturnMessage()
    {
        var endpointName = "test endpoint";

        EndpointNotFoundException sut = new(endpointName);

        sut.Message.Should().Be($"Endpoint: '{endpointName}' not found");
    }
}
