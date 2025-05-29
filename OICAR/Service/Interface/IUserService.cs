

using OICAR.DTOs;

namespace OICAR.Service.Interface
{
    public interface IUserService : IGenericService<UserDTO>
    {
        Task<UserDTO> GetByUsernameAsync(string username);
        Task<UserDTO> GetByEmailAsync(string email);
        Task<IEnumerable<UserDTO>> GetAdminsAsync();
    }
}
