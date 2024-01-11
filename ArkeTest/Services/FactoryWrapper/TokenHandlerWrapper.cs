using ArkeTest.Services.Factory.IFactory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ArkeTest.Services.FactoryWrapper
{
    public class TokenHandlerWrapper : ITokenHandlerWrapper
    {
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        public ClaimsPrincipal ValidateToken(string? token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            return _tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        }
    }
}
