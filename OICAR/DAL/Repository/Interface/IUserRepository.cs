using OICAR.Models;

namespace OICAR.DAL.Repository.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAdminsAsync();
    }
}
