using OICAR.DTOs;

namespace OICAR.Service.Interface
{
    public interface IItemService : IGenericService<ItemDTO>
    {
        Task<IEnumerable<ItemDTO>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<ItemDTO>> SearchByTitleAsync(string title);
        Task<int> IsInStockAsync(int itemId);
    }
}
