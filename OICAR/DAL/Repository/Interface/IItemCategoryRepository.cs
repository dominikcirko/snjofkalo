using OICAR.Models;

namespace OICAR.DAL.Repository.Interface
{
    public interface IItemCategoryRepository : IGenericRepository<ItemCategory>
    {
        Task<ItemCategory> GetByNameAsync(string categoryName);
        Task<bool> CategoryExistsAsync(string categoryName);
    }
}
