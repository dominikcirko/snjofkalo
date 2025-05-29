
using AutoMapper;
using OICAR.DAL.Repository.Interface;
using OICAR.DTOs;
using OICAR.Models;
using OICAR.Service.Interface;

namespace OICAR.Service.Implementation
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, ILogService logService, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemDTO>> GetAllAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            _logService.LogAction("Info", "Retrieved all items.");
            return _mapper.Map<IEnumerable<ItemDTO>>(items);
        }

        public async Task<ItemDTO> GetByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);

            if (item == null)
            {
                _logService.LogAction("Warning", $"Item with id={id} not found.");
                return null;
            }

            _logService.LogAction("Info", $"Retrieved item with id={id}.");
            return _mapper.Map<ItemDTO>(item);
        }

        public async Task AddAsync(ItemDTO itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);
            await _itemRepository.AddAsync(item);
            _logService.LogAction("Info", $"Item with id={item.Iditem} has been added.");
        }

        public async Task UpdateAsync(ItemDTO itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);
            await _itemRepository.UpdateAsync(item);
            _logService.LogAction("Info", $"Item with id={item.Iditem} has been updated.");
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);

            if (item == null)
            {
                _logService.LogAction("Warning", $"Attempted to delete non-existent item with id={id}.");
                return;
            }

            await _itemRepository.DeleteAsync(id);
            _logService.LogAction("Info", $"Item with id={id} has been deleted.");
        }

        public async Task<IEnumerable<ItemDTO>> GetByCategoryIdAsync(int categoryId)
        {
            var items = await _itemRepository.GetByCategoryIdAsync(categoryId);
            _logService.LogAction("Info", $"Retrieved items for category id={categoryId}.");
            return _mapper.Map<IEnumerable<ItemDTO>>(items);
        }

        //public async Task<IEnumerable<ItemDTO>> GetByTagIdAsync(int? tagId)
        //{
        //    var items = await _itemRepository.GetByTagIdAsync(tagId);
        //    _logService.LogAction("Info", $"Retrieved items for tag id={tagId}.");
        //    return _mapper.Map<IEnumerable<ItemDTO>>(items);
        //}

        public async Task<IEnumerable<ItemDTO>> SearchByTitleAsync(string title)
        {
            var items = await _itemRepository.SearchByTitleAsync(title);
            _logService.LogAction("Info", $"Searched items with title containing '{title}'.");
            return _mapper.Map<IEnumerable<ItemDTO>>(items);
        }

        public async Task<int> IsInStockAsync(int itemId)
        {
            var inStock = await _itemRepository.IsInStockAsync(itemId);

            if (inStock == 1)
            {
                _logService.LogAction("Info", $"Item with id={itemId} is in stock.");
            }
            else
            {
                _logService.LogAction("Warning", $"Item with id={itemId} is out of stock.");
            }

            return inStock;
        }
    }

}