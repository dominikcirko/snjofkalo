

using OICAR.DTOs;

namespace OICAR.Service.Interface
{
    public interface ICartService : IGenericService<CartDTO>
    {
        Task<CartDTO> GetByUserIdAsync(int userId);
    }
}
