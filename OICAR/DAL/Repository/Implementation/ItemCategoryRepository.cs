using Microsoft.Data.SqlClient;
using System.Data;
using OICAR.Models;
using OICAR.DAL.Repository.Interface;

namespace OICAR.DAL.Repository.Implementations
{
    public class ItemCategoryRepository : IItemCategoryRepository
    {
        private readonly string _connectionString;

        public ItemCategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<IEnumerable<ItemCategory>> GetAllAsync()
        {
            var categories = new List<ItemCategory>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetAllItemCategories", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(MapReaderToItemCategory(reader));
                    }
                }
            }
            return categories;
        }

        public async Task<ItemCategory> GetByIdAsync(int id)
        {
            ItemCategory category = null;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetItemCategoryById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IDItemCategory", id);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        category = MapReaderToItemCategory(reader);
                    }
                }
            }
            return category;
        }

        public async Task AddAsync(ItemCategory entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_CreateItemCategory", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryName", entity.CategoryName);

                var outputId = new SqlParameter("@IDItemCategory", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputId);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                entity.IditemCategory = (int)outputId.Value;
            }
        }

        public async Task UpdateAsync(ItemCategory entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_UpdateItemCategory", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IDItemCategory", entity.IditemCategory);
                command.Parameters.AddWithValue("@CategoryName", entity.CategoryName);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_DeleteItemCategory", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IDItemCategory", id); 

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<ItemCategory> GetByNameAsync(string categoryName)
        {
            ItemCategory category = null;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetItemCategoryByName", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryName", categoryName);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        category = MapReaderToItemCategory(reader);
                    }
                }
            }
            return category;
        }

        public async Task<bool> CategoryExistsAsync(string categoryName)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_CheckItemCategoryExists", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryName", categoryName);

                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();

                return (int)result > 0;
            }
        }

        private ItemCategory MapReaderToItemCategory(SqlDataReader reader)
        {
            return new ItemCategory
            {
                IditemCategory = reader.GetInt32(reader.GetOrdinal("IDItemCategory")),
                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
            };
        }
    }
}
