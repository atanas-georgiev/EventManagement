namespace EventManagement.UserManagement.Shared.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using EventManagement.UserManagement.Shared.Models;

    using Microsoft.AspNetCore.Identity;

    public static class UserExtensions
    {
        public static IEnumerable<Claim> GetClaims(this User user, UserManager<User> userManager)
        {
            var claims = new List<Claim>
                               {
                                   new Claim(ClaimTypes.Email, user.Email),
                                   new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
                               };

            var roles = userManager.GetRolesAsync(user).GetAwaiter().GetResult();
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }
    }
}
