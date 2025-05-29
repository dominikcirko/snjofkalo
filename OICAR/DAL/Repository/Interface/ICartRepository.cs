using OICAR.Models;

namespace OICAR.DAL.Repository.Interface
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetByUserIdAsync(int userId); 
        Task<bool> IsCartEmptyAsync(int cartId);
    }
}
