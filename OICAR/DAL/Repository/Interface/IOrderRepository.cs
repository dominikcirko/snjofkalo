using OICAR.Models;

namespace OICAR.DAL.Repository.Interface
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(int statusId);
    }
}
