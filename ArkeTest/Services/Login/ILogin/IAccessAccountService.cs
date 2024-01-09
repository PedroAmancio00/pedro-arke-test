using ArkeTest.DTO;

namespace ArkeTest.Services.Login.ILogin
{
    public interface IAccessAccountService
    {
        Task<ReturnDTO> AccessAccount(AccessAccountDTO dto);
    }
}
