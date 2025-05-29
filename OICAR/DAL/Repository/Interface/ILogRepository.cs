using OICAR.Models;

namespace OICAR.DAL.Repository.Interface
{
    public interface ILogRepository
    {
        IEnumerable<Log> GetLatestLogs(int count);
        int GetLogCount();
        void Add(Log log);
    }
}
