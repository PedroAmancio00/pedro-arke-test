using ArkeTest.DTO;
using ArkeTest.DTO.Login;

namespace ArkeTest.Services.Login.ILogin
{
    public interface ICreateLoginService
    {
        Task<ReturnDTO> CreateLogin(CreateLoginDTO dto);
    }
}
