using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ArkeTest.Services.Factory.IFactory
{
    public interface ITokenHandlerWrapper
    {
        ClaimsPrincipal ValidateToken(string? token, TokenValidationParameters validationParameters, out SecurityToken validatedToken);
    }
}
