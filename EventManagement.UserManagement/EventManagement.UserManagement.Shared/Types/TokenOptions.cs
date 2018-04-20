namespace EventManagement.UserManagement.Shared.Types
{
    using System;

    using Microsoft.IdentityModel.Tokens;

    public sealed class TokenOptions
    {
        public TokenOptions(string issuer, string audience, SecurityKey signingKey, int tokenExpiryInMinutes = 5)
        {
            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new ArgumentNullException($"{nameof(this.Audience)} is mandatory in order to generate a JWT!");
            }

            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new ArgumentNullException($"{nameof(this.Issuer)} is mandatory in order to generate a JWT!");
            }

            this.Audience = audience;
            this.Issuer = issuer;
            this.SigningKey = signingKey ?? throw new ArgumentNullException(
                                  $"{nameof(this.SigningKey)} is mandatory in order to generate a JWT!");
            this.TokenExpiryInMinutes = tokenExpiryInMinutes;
        }

        public string Audience { get; }

        public string Issuer { get; }

        public SecurityKey SigningKey { get; }

        public int TokenExpiryInMinutes { get; }
    }

    public struct TokenConstants
    {
        public const string TokenName = "jwt";
    }
}