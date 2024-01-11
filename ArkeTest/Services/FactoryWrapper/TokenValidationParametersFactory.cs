using ArkeTest.Services.Factory.IFactory;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ArkeTest.Services.Factory
{
    public class TokenValidationParametersFactory : ITokenValidationParametersFactory
    {
        public TokenValidationParametersFactory()
        {
        }

        public TokenValidationParameters Create(string secret)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

    }
}
