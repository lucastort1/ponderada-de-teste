using System.Threading.Tasks;
using Xunit;
using Moq;
using backend.Services;
using backend.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;

public class AuthServiceTests
{
    [Fact]
    public async Task LoginUserAsync_ReturnsSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<AuthService>>();
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Firebase:ApiKey"]).Returns("FAKE_API_KEY");

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(handlerMock.Object);

        var loginResponse = new
        {
            idToken = "fake-token",
            localId = "fake-uid"
        };

        var content = new StringContent(JsonSerializer.Serialize(loginResponse), Encoding.UTF8, "application/json");

        handlerMock
            .Setup(h => h.Send(It.IsAny<HttpRequestMessage>()))
            .Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = content
            });

        var authService = new AuthService(configMock.Object, loggerMock.Object);

        var loginRequest = new LoginRequest
        {
            Email = "lucas@email.com",
            Password = "123456"
        };

        // Act
        var result = await authService.LoginUserAsync(loginRequest);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Login bem-sucedido", result.Message);
    }
}
