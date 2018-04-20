namespace EventManagement.UserManagement.Shared.Types
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authentication;

    public sealed class TokenWithClaimsPrincipal
    {
        public string AccessToken { get; internal set; }

        public AuthenticationProperties AuthProperties { get; internal set; }

        public ClaimsPrincipal ClaimsPrincipal { get; internal set; }
    }
}