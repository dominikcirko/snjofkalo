using OICAR.Models;

namespace OICAR.DAL.Repository.Interface
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task<IEnumerable<Item>> GetByCategoryIdAsync(int categoryId);
        //Task<IEnumerable<Item>> GetByTagIdAsync(int? tagId);
        Task<IEnumerable<Item>> SearchByTitleAsync(string title);
        Task<int> IsInStockAsync(int itemId);
    }
}
