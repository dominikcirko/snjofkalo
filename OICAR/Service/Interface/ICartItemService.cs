

using OICAR.DTOs;

namespace OICAR.Service.Interface
{
    public interface ICartItemService : IGenericService<CartItemDTO>
    {
        Task<IEnumerable<CartItemDTO>> GetByCartIdAsync(int cartId);
    }
}
