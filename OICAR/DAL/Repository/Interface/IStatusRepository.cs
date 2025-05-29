using OICAR.Models;

namespace OICAR.DAL.Repository.Interface
{
    public interface IStatusRepository : IGenericRepository<Status>
    {
        Task<Status> GetByNameAsync(string name);
    }
}
