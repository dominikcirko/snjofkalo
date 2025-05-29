using OICAR.DAL.Repository.Interface;
using OICAR.Models;
using OICAR.Service.Interface;


namespace OICAR.Service.Implementation
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public IEnumerable<Log> GetLatestLogs(int count)
        {
            return _logRepository.GetLatestLogs(count);
        }

        public int GetLogCount()
        {
            return _logRepository.GetLogCount();
        }

        public void LogAction(string level, string message)
        {
            var log = new Log
            {
                Level = level,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            _logRepository.Add(log);
        }
    }
}
