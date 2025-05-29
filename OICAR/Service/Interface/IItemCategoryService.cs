using System.Threading.Tasks;
using OICAR.DTOs;

namespace OICAR.Service.Interface
{
    public interface IItemCategoryService : IGenericService<ItemCategoryDTO>
    {
        Task<ItemCategoryDTO> GetByNameAsync(string categoryName);
    }
}
