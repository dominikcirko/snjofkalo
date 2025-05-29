
using OICAR.DTOs;
using OICAR.Service.Interface;

namespace OICAR.Service.Interface
{
    public interface IOrderService : IGenericService<OrderDTO>
    {
        Task<IEnumerable<OrderDTO>> GetByUserIdAsync(int userId);
        Task UpdateOrderStatusAsync(int orderId, int statusId);
    }
}
