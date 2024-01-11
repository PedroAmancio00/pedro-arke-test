using Microsoft.IdentityModel.Tokens;

namespace ArkeTest.Services.Factory.IFactory
{
    public interface ITokenValidationParametersFactory
    {
        TokenValidationParameters Create(string secret);
    }
}
