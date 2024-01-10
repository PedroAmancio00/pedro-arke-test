using ArkeTest.DTO.User;
using ArkeTest.DTO;

namespace ArkeTest.Services.User.IUser
{
    public interface ICreateUser
    {
        Task<ReturnDTO> CreateOrUpdateUser(CreateUserDTO dto);
    }
}
