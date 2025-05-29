using AutoMapper;
using OICAR.DAL.Repository.Interface;
using OICAR.DTOs;
using OICAR.Models;
using OICAR.Service.Interface;

namespace OICAR.Service.Implementation
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public CartItemService(ICartItemRepository cartItemRepository, ILogService logService, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemDTO>> GetAllAsync()
        {
            var cartItems = await _cartItemRepository.GetAllAsync();
            _logService.LogAction("Info", "Retrieved all cart items.");
            return _mapper.Map<IEnumerable<CartItemDTO>>(cartItems);
        }

        public async Task<CartItemDTO> GetByIdAsync(int id)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(id);

            if (cartItem == null)
            {
                _logService.LogAction("Warning", $"Cart item with id={id} not found.");
                return null;
            }

            _logService.LogAction("Info", $"Retrieved cart item with id={id}.");
            return _mapper.Map<CartItemDTO>(cartItem);
        }

        public async Task AddAsync(CartItemDTO cartItemDto)
        {
            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            await _cartItemRepository.AddAsync(cartItem);

            _logService.LogAction("Info", $"Cart item with id={cartItem.IdcartItem} has been added.");
        }

        public async Task UpdateAsync(CartItemDTO cartItemDto)
        {
            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            await _cartItemRepository.UpdateAsync(cartItem);

            _logService.LogAction("Info", $"Cart item with id={cartItem.IdcartItem} has been updated.");
        }

        public async Task DeleteAsync(int id)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(id);

            if (cartItem == null)
            {
                _logService.LogAction("Warning", $"Attempted to delete non-existent cart item with id={id}.");
                return;
            }

            await _cartItemRepository.DeleteAsync(id);
            _logService.LogAction("Info", $"Cart item with id={id} has been deleted.");
        }

        public async Task<IEnumerable<CartItemDTO>> GetByCartIdAsync(int cartId)
        {
            var cartItems = await _cartItemRepository.GetByCartIdAsync(cartId);
            _logService.LogAction("Info", $"Retrieved cart items for cart id={cartId}.");
            return _mapper.Map<IEnumerable<CartItemDTO>>(cartItems);
        }
    }
}