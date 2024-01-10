using ArkeTest.DTO;
using ArkeTest.DTO.Login;

namespace ArkeTest.Services.Login.ILogin
{
    public interface IChangePasswordService
    {
        Task<ReturnDTO> ChangePassword(ChangePasswordDto dto);
    }
}
