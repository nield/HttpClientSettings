namespace HttpClientSettings.Tests.Exceptions;

public class InvalidAppSettingsExceptionTests
{
    [Fact]
    public void Given_ExceptionIsThrown_ShouldReturnMessage()
    {
        var errors = new List<string>
        { 
            "failure1", 
            "failure2" 
        };

        InvalidAppSettingsException sut = new(errors);

        sut.Message.Should().Be($"Invalid http client settings found: {string.Join(",", errors)}");
    }
}
