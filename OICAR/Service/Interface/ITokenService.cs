
using OICAR.DTOs;

namespace OICAR.Service.Interface
{
    public interface ITokenService
    {
        string GenerateJwtToken(UserDTO user);
    }
}
