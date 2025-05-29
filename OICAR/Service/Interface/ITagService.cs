
using OICAR.DTOs;

namespace OICAR.Service.Interface
{
    public interface ITagService : IGenericService<TagDTO>
    {
        Task<TagDTO> GetByNameAsync(string name);
    }
}
