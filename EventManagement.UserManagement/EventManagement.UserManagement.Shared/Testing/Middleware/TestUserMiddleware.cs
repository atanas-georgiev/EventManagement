namespace EventManagement.UserManagement.Shared.Testing.Middleware
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    using EventManagement.UserManagement.Shared.Models;

    using JwtAuthenticationHelper.Abstractions;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;

    public class TestUserMiddleware
    {
        private readonly RequestDelegate next;

        private readonly IJwtTokenGenerator tokenGenerator;

        public TestUserMiddleware(RequestDelegate next, IJwtTokenGenerator tokenGenerator)
        {
            this.next = next;
            this.tokenGenerator = tokenGenerator;
        }

        public Task Invoke(HttpContext context)
        {
            var testEmail = context.Request.Headers["TestEmail"];
            var testAdmin = context.Request.Headers["TestAdmin"];
            var testName = context.Request.Headers["TestName"];

            if (!string.IsNullOrWhiteSpace(testEmail) && !string.IsNullOrWhiteSpace(testName) && !string.IsNullOrWhiteSpace(testAdmin))
            {
                var isAdmin = bool.Parse(testAdmin);
                var claims = new List<Claim>
                                 {
                                     new Claim(ClaimTypes.Email, testEmail),
                                     new Claim(ClaimTypes.Name, testName),
                                     isAdmin
                                         ? new Claim(ClaimTypes.Role, "Admin")
                                         : new Claim(ClaimTypes.Role, "User")
                                 };

                var accessTokenResult = this.tokenGenerator.GenerateAccessTokenWithClaimsPrincipal(testEmail, claims);
                context.User = accessTokenResult.ClaimsPrincipal;              
            }
            
            return this.next(context);
        }
    }
}
