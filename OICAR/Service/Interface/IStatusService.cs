
using OICAR.DTOs;

namespace OICAR.Service.Interface
{
    public interface IStatusService : IGenericService<StatusDTO>
    {
        Task<StatusDTO> GetByNameAsync(string name);

    }
}
