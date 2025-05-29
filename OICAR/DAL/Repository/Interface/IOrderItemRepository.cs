using OICAR.Models;

namespace OICAR.DAL.Repository.Interface
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderItem>> GetByItemIdAsync(int itemId);
    }
}
