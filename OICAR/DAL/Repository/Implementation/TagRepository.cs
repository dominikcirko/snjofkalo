using Microsoft.Data.SqlClient;
using OICAR.DAL.Repository.Interface;
using OICAR.Models;
using System.Data;

namespace OICAR.DAL.Repository.Implementations
{
    public class TagRepository : ITagRepository
    {
        private readonly string _connectionString;

        public TagRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            var tags = new List<Tag>();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetAllTags", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                tags.Add(MapToTag(reader));
            }

            return tags;
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetTagById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@IDTag", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapToTag(reader);
            }

            return null;
        }

        public async Task AddAsync(Tag entity)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_CreateTag", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = entity.Name;

            var outputParameter = command.Parameters.Add("@IDTag", SqlDbType.Int);
            outputParameter.Direction = ParameterDirection.Output;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            entity.Idtag = (int)outputParameter.Value;
        }

        public async Task UpdateAsync(Tag entity)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_UpdateTag", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@IDTag", SqlDbType.Int).Value = entity.Idtag;
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = entity.Name;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_DeleteTag", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@IDTag", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<Tag> GetByNameAsync(string name)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetTagByName", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = name;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapToTag(reader);
            }

            return null;
        }

        private static Tag MapToTag(SqlDataReader reader)
        {
            return new Tag
            {
                Idtag = reader.GetInt32(reader.GetOrdinal("IDTag")),
                Name = reader.GetString(reader.GetOrdinal("Name"))
            };
        }
    }
}