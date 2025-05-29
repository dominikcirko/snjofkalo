using AutoMapper;
using OICAR.DAL.Repository.Interface;
using OICAR.DTOs;
using OICAR.Models;
using OICAR.Service.Interface;

namespace OICAR.Service.Implementation
{
    public class ItemCategoryService : IItemCategoryService
    {
        private readonly IItemCategoryRepository _itemCategoryRepository;
        private readonly IMapper _mapper;

        public ItemCategoryService(IItemCategoryRepository itemCategoryRepository, IMapper mapper)
        {
            _itemCategoryRepository = itemCategoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemCategoryDTO>> GetAllAsync()
        {
            var itemCategories = await _itemCategoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ItemCategoryDTO>>(itemCategories);
        }

        public async Task<ItemCategoryDTO> GetByIdAsync(int id)
        {
            var itemCategory = await _itemCategoryRepository.GetByIdAsync(id);
            return _mapper.Map<ItemCategoryDTO>(itemCategory);
        }

        public async Task AddAsync(ItemCategoryDTO itemCategoryDto)
        {
            var itemCategory = _mapper.Map<ItemCategory>(itemCategoryDto);
            await _itemCategoryRepository.AddAsync(itemCategory);
        }

        public async Task UpdateAsync(ItemCategoryDTO itemCategoryDto)
        {
            var itemCategory = _mapper.Map<ItemCategory>(itemCategoryDto);
            await _itemCategoryRepository.UpdateAsync(itemCategory);
        }

        public async Task DeleteAsync(int id)
        {
            var itemCategory = await _itemCategoryRepository.GetByIdAsync(id);

            await _itemCategoryRepository.DeleteAsync(id);
        }

        public async Task<ItemCategoryDTO> GetByNameAsync(string categoryName)
        {
            var itemCategory = await _itemCategoryRepository.GetByNameAsync(categoryName);
            return _mapper.Map<ItemCategoryDTO>(itemCategory);
        }

        public async Task<bool> CategoryExistsAsync(string categoryName)
        {
            var exists = await _itemCategoryRepository.CategoryExistsAsync(categoryName);
            return exists;
        }
    }
}