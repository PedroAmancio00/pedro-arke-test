using ArkeTest.DTO;
using ArkeTest.DTO.User;

namespace ArkeTest.Services.User.IUser
{
    public interface ICreateUser
    {
        Task<ReturnDTO> CreateOrUpdateUser(CreateUserDTO dto);
    }
}
