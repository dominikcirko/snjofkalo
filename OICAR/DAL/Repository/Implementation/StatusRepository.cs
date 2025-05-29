using Microsoft.Data.SqlClient;
using OICAR.DAL.Repository.Interface;
using OICAR.Models;
using System.Data;


namespace OICAR.DAL.Repository.Implementations
{
    public class StatusRepository : IStatusRepository
    {
        private readonly string _connectionString;

        public StatusRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            var statuses = new List<Status>();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetAllStatuses", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                statuses.Add(MapToStatus(reader));
            }

            return statuses;
        }

        public async Task<Status> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetStatusById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@IDStatus", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapToStatus(reader);
            }

            return null;
        }

        public async Task AddAsync(Status entity)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_CreateStatus", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = entity.Name;
            command.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = entity.Description;

            var outputParameter = command.Parameters.Add("@IDStatus", SqlDbType.Int);
            outputParameter.Direction = ParameterDirection.Output;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            entity.Idstatus = (int)outputParameter.Value;
        }

        public async Task UpdateAsync(Status entity)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_UpdateStatus", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@IDStatus", SqlDbType.Int).Value = entity.Idstatus;
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = entity.Name;
            command.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = entity.Description;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_DeleteStatus", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@IDStatus", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<Status> GetByNameAsync(string name)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetStatusByName", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = name;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapToStatus(reader);
            }

            return null;
        }

        private static Status MapToStatus(SqlDataReader reader)
        {
            return new Status
            {
                Idstatus = reader.GetInt32(reader.GetOrdinal("IDStatus")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.GetString(reader.GetOrdinal("Description"))
            };
        }
    }
}