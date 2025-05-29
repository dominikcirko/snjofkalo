using OICAR.Models;

namespace OICAR.DAL.Repository.Interface
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<Tag> GetByNameAsync(string name);
    }
}
