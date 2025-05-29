using System.Data;
using Microsoft.Data.SqlClient;
using OICAR.DAL.Repository.Interface;
using OICAR.Models;

namespace OICAR.DAL.Repository.Implementation
{
    public class LogRepository : ILogRepository
    {
        private readonly string _connectionString;

        public LogRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public IEnumerable<Log> GetLatestLogs(int count)
        {
            var logs = new List<Log>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_GetLatestLogs", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Count", SqlDbType.Int).Value = count;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            logs.Add(MapToLog(reader));
                        }
                    }
                }
            }

            return logs;
        }

        public int GetLogCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_GetLogCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    return (int)command.ExecuteScalar();
                }
            }
        }

        public void Add(Log log)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_CreateLog", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Timestamp", SqlDbType.DateTime).Value = log.Timestamp;
                    command.Parameters.Add("@Level", SqlDbType.NVarChar, 50).Value = log.Level;
                    command.Parameters.Add("@Message", SqlDbType.NVarChar).Value = log.Message;

                    var outputParameter = command.Parameters.Add("@Id", SqlDbType.Int);
                    outputParameter.Direction = ParameterDirection.Output;

                    connection.Open();
                    command.ExecuteNonQuery();

                    log.Id = (int)outputParameter.Value;
                }
            }
        }

        private Log MapToLog(SqlDataReader reader)
        {
            return new Log
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Timestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp")),
                Level = reader.GetString(reader.GetOrdinal("Level")),
                Message = reader.GetString(reader.GetOrdinal("Message"))
            };
        }
    }
}