using OICAR.Models;

namespace OICAR.DAL.Repository.Interface
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetByCartIdAsync(int cartId);
    }
}
