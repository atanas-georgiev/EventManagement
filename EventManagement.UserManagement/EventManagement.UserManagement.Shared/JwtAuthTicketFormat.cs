namespace EventManagement.UserManagement.Shared
{
    using System;
    using System.IdentityModel.Tokens.Jwt;

    using EventManagement.UserManagement.Shared.Types;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.IdentityModel.Tokens;

    public sealed class JwtAuthTicketFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string Algorithm = SecurityAlgorithms.HmacSha256;

        private readonly IDataProtector dataProtector;

        private readonly IDataSerializer<AuthenticationTicket> ticketSerializer;

        private readonly TokenValidationParameters validationParameters;

        public JwtAuthTicketFormat(
            TokenValidationParameters validationParameters,
            IDataSerializer<AuthenticationTicket> ticketSerializer,
            IDataProtector dataProtector)
        {
            this.validationParameters = validationParameters
                                        ?? throw new ArgumentNullException(
                                            $"{nameof(validationParameters)} cannot be null");
            this.ticketSerializer = ticketSerializer
                                    ?? throw new ArgumentNullException($"{nameof(ticketSerializer)} cannot be null");
            
            this.dataProtector =
                dataProtector ?? throw new ArgumentNullException($"{nameof(dataProtector)} cannot be null");
        }

        public string Protect(AuthenticationTicket data) => this.Protect(data, null);

        public string Protect(AuthenticationTicket data, string purpose)
        {
            var array = this.ticketSerializer.Serialize(data);

            return Base64UrlTextEncoder.Encode(this.dataProtector.Protect(array));
        }

        public AuthenticationTicket Unprotect(string protectedText) => this.Unprotect(protectedText, null);

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var authTicket = this.ticketSerializer.Deserialize(
                this.dataProtector.Unprotect(Base64UrlTextEncoder.Decode(protectedText)));

            var embeddedJwt = authTicket.Properties?.GetTokenValue(TokenConstants.TokenName);

            try
            {
                new JwtSecurityTokenHandler().ValidateToken(embeddedJwt, this.validationParameters, out var token);

                if (!(token is JwtSecurityToken jwt))
                {
                    throw new SecurityTokenValidationException("JWT token was found to be invalid");
                }

                if (!jwt.Header.Alg.Equals(Algorithm, StringComparison.Ordinal))
                {
                    throw new ArgumentException($"Algorithm must be '{Algorithm}'");
                }
            }
            catch (Exception)
            {
                return null;
            }

            return authTicket;
        }
    }
}