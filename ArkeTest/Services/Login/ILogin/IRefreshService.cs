using ArkeTest.DTO;

namespace ArkeTest.Services.Login.ILogin
{
    public interface IRefreshService
    {
        Task<ReturnDTO> Refresh();
    }
}
