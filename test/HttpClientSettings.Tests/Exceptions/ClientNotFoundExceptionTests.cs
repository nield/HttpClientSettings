namespace HttpClientSettings.Tests.Exceptions;

public class ClientNotFoundExceptionTests
{
    [Fact]
    public void Given_ExceptionIsThrown_ShouldReturnMessage()
    {
        var clientName = "test client";

        ClientNotFoundException sut = new(clientName);

        sut.Message.Should().Be($"Client: '{clientName}' not found");
    }
}
