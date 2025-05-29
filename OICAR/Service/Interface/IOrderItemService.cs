
using OICAR.DTOs;

namespace OICAR.Service.Interface
{
    public interface IOrderItemService : IGenericService<OrderItemDTO>
    {
        Task<IEnumerable<OrderItemDTO>> GetByItemIdAsync(int itemId);
        Task<IEnumerable<OrderItemDTO>> GetByOrderIdAsync(int orderId);
    }
}
