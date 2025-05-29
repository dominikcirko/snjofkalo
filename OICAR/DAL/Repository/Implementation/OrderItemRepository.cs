using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OICAR.DAL.Repository.Interface;
using OICAR.Models;

namespace OICAR.DAL.Repository.Implementations
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly string _connectionString;

        public OrderItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            var orderItems = new List<OrderItem>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_GetAllOrderItems", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orderItems.Add(MapToOrderItem(reader));
                        }
                    }
                }
            }

            return orderItems;
        }

        public async Task<OrderItem> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_GetOrderItemById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IDOrderItem", SqlDbType.Int).Value = id;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapToOrderItem(reader);
                        }
                    }
                }
            }

            return null;
        }

        public async Task AddAsync(OrderItem entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_CreateOrderItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = entity.OrderId;
                    command.Parameters.Add("@ItemID", SqlDbType.Int).Value = entity.ItemId;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = entity.Quantity;

                    var outputParameter = command.Parameters.Add("@IDOrderItem", SqlDbType.Int);
                    outputParameter.Direction = ParameterDirection.Output;

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    entity.IdorderItem = (int)outputParameter.Value;
                }
            }
        }

        public async Task UpdateAsync(OrderItem entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_UpdateOrderItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IDOrderItem", SqlDbType.Int).Value = entity.IdorderItem;
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = entity.OrderId;
                    command.Parameters.Add("@ItemID", SqlDbType.Int).Value = entity.ItemId;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = entity.Quantity;

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_DeleteOrderItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IDOrderItem", SqlDbType.Int).Value = id;

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            var orderItems = new List<OrderItem>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_GetOrderItemsByOrderId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderId;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orderItems.Add(MapToOrderItem(reader));
                        }
                    }
                }
            }

            return orderItems;
        }

        public async Task<IEnumerable<OrderItem>> GetByItemIdAsync(int itemId)
        {
            var orderItems = new List<OrderItem>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_GetOrderItemsByItemId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ItemID", SqlDbType.Int).Value = itemId;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orderItems.Add(MapToOrderItem(reader));
                        }
                    }
                }
            }

            return orderItems;
        }

        private OrderItem MapToOrderItem(SqlDataReader reader)
        {
            return new OrderItem
            {
                IdorderItem = reader.GetInt32(reader.GetOrdinal("IDOrderItem")),
                OrderId = reader.GetInt32(reader.GetOrdinal("OrderID")),
                ItemId = reader.GetInt32(reader.GetOrdinal("ItemID")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
            };
        }
    }
}