using AutoMapper;
using OICAR.DAL.Repository.Interface;
using OICAR.DTOs;
using OICAR.Models;
using OICAR.Service.Interface;

namespace OICAR.Service.Implementation
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, ILogService logService, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartDTO>> GetAllAsync()
        {
            var carts = await _cartRepository.GetAllAsync();
            _logService.LogAction("Info", "Retrieved all carts.");
            return _mapper.Map<IEnumerable<CartDTO>>(carts);
        }

        public async Task<CartDTO> GetByIdAsync(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);

            if (cart == null)
            {
                _logService.LogAction("Warning", $"Cart with id={id} not found.");
                return null;
            }

            _logService.LogAction("Info", $"Retrieved cart with id={id}.");
            return _mapper.Map<CartDTO>(cart);
        }

        public async Task AddAsync(CartDTO cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);
            await _cartRepository.AddAsync(cart);
            _logService.LogAction("Info", $"Cart with id={cart.Idcart} has been added.");
        }

        public async Task UpdateAsync(CartDTO cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);
            await _cartRepository.UpdateAsync(cart);
            _logService.LogAction("Info", $"Cart with id={cart.Idcart} has been updated.");
        }

        public async Task DeleteAsync(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);

            if (cart == null)
            {
                _logService.LogAction("Warning", $"Attempted to delete non-existent cart with id={id}.");
                return;
            }

            await _cartRepository.DeleteAsync(id);
            _logService.LogAction("Info", $"Cart with id={id} has been deleted.");
        }

        public async Task<CartDTO> GetByUserIdAsync(int userId)
        {
            CartDTO cartDto = null;
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart != null)
            {
                cartDto = new CartDTO
                {
                    IDCart = cart.Idcart,
                    UserID = cart.UserId,
                    CartItems = cart.CartItems.Select(ci => new CartItemDTO
                    {
                        IDCartItem = ci.IdcartItem,
                        CartID = ci.CartId,
                        ItemID = ci.ItemId,
                        Quantity = ci.Quantity
                    }).ToList()
                };
            }
            return cartDto;
        }
    }
}