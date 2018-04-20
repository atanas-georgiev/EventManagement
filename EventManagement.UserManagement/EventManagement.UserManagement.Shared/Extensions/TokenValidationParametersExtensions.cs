namespace EventManagement.UserManagement.Shared.Extensions
{
    using EventManagement.UserManagement.Shared.Types;

    using Microsoft.IdentityModel.Tokens;

    public static class TokenValidationParametersExtensions
    {
        public static TokenOptions ToTokenOptions(
            this TokenValidationParameters tokenValidationParameters,
            int tokenExpiryInMinutes = 5)
        {
            return new TokenOptions(
                tokenValidationParameters.ValidIssuer,
                tokenValidationParameters.ValidAudience,
                tokenValidationParameters.IssuerSigningKey,
                tokenExpiryInMinutes);
        }
    }
}