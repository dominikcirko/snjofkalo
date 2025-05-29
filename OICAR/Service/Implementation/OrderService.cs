using AutoMapper;
using OICAR.DAL.Repository.Interface;
using OICAR.DTOs;
using OICAR.Models;
using OICAR.Service.Interface;

namespace OICAR.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ILogService logService, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            _logService.LogAction("Info", "Retrieved all orders.");
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                _logService.LogAction("Warning", $"Order with id={id} not found.");
                return null;
            }

            _logService.LogAction("Info", $"Retrieved order with id={id}.");
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task AddAsync(OrderDTO orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            await _orderRepository.AddAsync(order);
            _logService.LogAction("Info", $"Order with id={order.Idorder} has been added.");
        }

        public async Task UpdateAsync(OrderDTO orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            await _orderRepository.UpdateAsync(order);
            _logService.LogAction("Info", $"Order with id={order.Idorder} has been updated.");
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                _logService.LogAction("Warning", $"Attempted to delete non-existent order with id={id}.");
                return;
            }

            await _orderRepository.DeleteAsync(id);
            _logService.LogAction("Info", $"Order with id={id} has been deleted.");
        }

        public async Task<IEnumerable<OrderDTO>> GetByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            _logService.LogAction("Info", $"Retrieved orders for user with id={userId}.");
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<IEnumerable<OrderDTO>> GetByStatusAsync(int statusId)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(statusId);
            _logService.LogAction("Info", $"Retrieved orders with status id={statusId}.");
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task UpdateOrderStatusAsync(int orderId, int statusId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
            {
                _logService.LogAction("Warning", $"Attempted to update status for non-existent order with id={orderId}.");
                return;
            }

            order.StatusId = statusId;
            await _orderRepository.UpdateAsync(order);
            _logService.LogAction("Info", $"Order with id={orderId} status updated to {statusId}.");
        }
    }
}