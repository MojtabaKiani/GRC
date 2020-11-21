using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace FunctionalTests.GRCWeb.Extensions
{
    public class UserManagementHelperTests
    {
        [Theory]
        [InlineData("Administrator",true)]
        [InlineData("User",false)]
        [InlineData("",false)]
        public void IsAdministrator_Should_Return_Correct_Value_Based_On_User_Role(string role,bool value)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Mojtaba"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role , role),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(claimsPrincipal);

            Assert.Equal(value, mockHttpContext.Object.User.IsAdministrator());
        }
    }
}
