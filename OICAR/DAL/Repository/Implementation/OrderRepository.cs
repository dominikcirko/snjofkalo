using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using OICAR.DAL.Repository.Interface;
using OICAR.Models;

namespace OICAR.DAL.Repository.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = new List<Order>();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetAllOrders", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                orders.Add(MapToOrder(reader));
            }

            return orders;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetOrderById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@IDOrder", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapToOrder(reader);
            }

            return null;
        }

        public async Task AddAsync(Order entity)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_CreateOrder", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@UserID", SqlDbType.Int).Value = entity.UserId;
            command.Parameters.Add("@StatusID", SqlDbType.Int).Value = entity.StatusId;
            command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = entity.OrderDate;
            command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = entity.TotalAmount;

            var outputParameter = command.Parameters.Add("@IDOrder", SqlDbType.Int);
            outputParameter.Direction = ParameterDirection.Output;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            entity.Idorder = (int)outputParameter.Value;
        }

        public async Task UpdateAsync(Order entity)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_UpdateOrder", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@IDOrder", SqlDbType.Int).Value = entity.Idorder;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = entity.UserId;
            command.Parameters.Add("@StatusID", SqlDbType.Int).Value = entity.StatusId;
            command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = entity.OrderDate;
            command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = entity.TotalAmount;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_DeleteOrder", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@IDOrder", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = new List<Order>();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetOrdersByUserId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                orders.Add(MapToOrder(reader));
            }

            return orders;
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(int statusId)
        {
            var orders = new List<Order>();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetOrdersByStatus", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@StatusID", SqlDbType.Int).Value = statusId;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                orders.Add(MapToOrder(reader));
            }

            return orders;
        }

        private static Order MapToOrder(SqlDataReader reader)
        {
            return new Order
            {
                Idorder = reader.GetInt32(reader.GetOrdinal("IDOrder")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserID")),
                StatusId = reader.GetInt32(reader.GetOrdinal("StatusID")),
                OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount"))
            };
        }
    }
}