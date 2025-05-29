using OICAR.Models;

namespace OICAR.Service.Interface
{
    public interface ILogService
    {
        IEnumerable<Log> GetLatestLogs(int count);
        int GetLogCount();
        void LogAction(string level, string message);
    }
}
