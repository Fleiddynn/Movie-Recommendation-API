using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Entitites;
using WebApplication1.DbContexts;
using WebApplication1.Dto;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using WebApplication1.DbContexts.UserData;
using WebApplication1.DbContexts.MovieRecData;

namespace GoogleFacebookLoginTests
{
    public class GoogleLoginTests
    {
        private AllDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AllDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new AllDbContext(options);
        }

        private IConfiguration GetTestConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "SuperSecretKeyForTesting1234567890"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };
            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        private UserController GetController(AllDbContext context, IConfiguration configuration, AuthenticateResult authResult)
        {
            var userRepository = new UserRepository(context);
            var movieRepository = new MovieRepository(context);
            var controller = new UserController(context);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(s => s.GetService(typeof(IConfiguration))).Returns(configuration);

            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService
                .Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
                .ReturnsAsync(authResult);

            mockServiceProvider.Setup(s => s.GetService(typeof(IAuthenticationService))).Returns(mockAuthenticationService.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = mockServiceProvider.Object;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            return controller;
        }

        [Fact]
        public async Task GoogleLogin()
        {
            var dbName = "GoogleLoginTestDb_NewUser";
            var context = GetDbContext(dbName);
            var config = GetTestConfiguration();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "testuser@example.com"),
                new Claim(ClaimTypes.GivenName, "Test"),
                new Claim(ClaimTypes.Surname, "User")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);
            var authResult = AuthenticateResult.Success(new AuthenticationTicket(principal, "Cookies"));

            var controller = GetController(context, config, authResult);

            var result = await controller.GoogleResponse();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as dynamic;
            Assert.NotNull(response.token);
            Assert.NotNull(response.user);
            Assert.Equal("testuser@example.com", response.user.email);
            Assert.Equal("Test", response.user.first_name);

            var newUser = await context.Users.FirstOrDefaultAsync(u => u.email == "testuser@example.com");
            Assert.NotNull(newUser);
            Assert.Equal("Google", newUser.social_login_provider);
        }
    }
}
