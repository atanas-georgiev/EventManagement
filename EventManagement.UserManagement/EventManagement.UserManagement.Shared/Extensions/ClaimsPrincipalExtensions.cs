namespace EventManagement.UserManagement.Shared.Extensions
{
    using System;
    using System.Linq;
    using System.Security.Claims;

    using EventManagement.UserManagement.Shared.Models;

    public static class ClaimsPrincipalExtensions
    {
        public static User GetUserDetails(this ClaimsPrincipal user)
        {
            return new User()
            {
                UserName = GetClaimValueByType(user, ClaimTypes.Email),
                Email = GetClaimValueByType(user, ClaimTypes.Email),
                FirstName = GetClaimValueByType(user, ClaimTypes.Name).Split(' ')[0],
                LastName = GetClaimValueByType(user, ClaimTypes.Name).Split(' ')[1]
            };
        }

        private static string GetClaimValueByType(
            this ClaimsPrincipal user,
            string claimType)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claimType == null)
            {
                throw new ArgumentNullException(nameof(claimType));
            }

            var value = user.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

            return value;
        }

        private static string[] GetClaimValuesByType(
            this ClaimsPrincipal user,
            string claimType)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claimType == null)
            {
                throw new ArgumentNullException(nameof(claimType));
            }

            var values = user.Claims.Where(c => c.Type == claimType).Select(c => c.Value).ToArray();

            return values;
        }
    }
}
