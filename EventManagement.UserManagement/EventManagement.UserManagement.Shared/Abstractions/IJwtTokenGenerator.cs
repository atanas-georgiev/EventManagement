namespace JwtAuthenticationHelper.Abstractions
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using EventManagement.UserManagement.Shared.Types;

    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(string userName, IEnumerable<Claim> userClaims);

        TokenWithClaimsPrincipal GenerateAccessTokenWithClaimsPrincipal(string userName, IEnumerable<Claim> userClaims);
    }
}