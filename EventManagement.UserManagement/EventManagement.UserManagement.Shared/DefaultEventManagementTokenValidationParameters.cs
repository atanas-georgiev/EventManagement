namespace EventManagement.UserManagement.Shared
{
    using System;
    using System.Text;

    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public class DefaultEventManagementTokenValidationParameters : TokenValidationParameters
    {
        public DefaultEventManagementTokenValidationParameters(IConfigurationRoot configuration)
        {
            this.ClockSkew = TimeSpan.Zero;
            this.ValidateAudience = true;
            this.ValidAudience = configuration["Security:TokenAudience"];
            this.ValidateIssuer = true;
            this.ValidIssuer = configuration["Security:TokenIssuer"];
            this.IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Security:TokenSigningKey"]));
            this.ValidateIssuerSigningKey = true;
            this.RequireExpirationTime = true;
            this.ValidateLifetime = true;
        }
    }
}