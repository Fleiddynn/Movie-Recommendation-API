using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using WebApplication1;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using WebApplication1.Entitites;

public class UserAuthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserAuthTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        var registerDto = new
        {
            first_name = "fahafhah",
            last_name = "afhgafhfahaf",
            email = "afhafhafh@gmail.com",
            password = "1324151"
        };
        await _client.PostAsJsonAsync("/api/User/register", registerDto);

        var loginDto = new
        {
            email = "afhafhafh@gmail.com",
            password = "1324151"
        };
        var response = await _client.PostAsJsonAsync("/api/User/login", loginDto);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        Assert.False(string.IsNullOrEmpty(result.token));
        Assert.Equal("afhafhafh@gmail.com", result.user.email);
    }

    private class LoginResponse
    {
        public string token { get; set; }
        public UserDTO user { get; set; }
    }
}