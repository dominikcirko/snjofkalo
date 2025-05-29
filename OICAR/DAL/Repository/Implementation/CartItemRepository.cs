using Microsoft.Data.SqlClient;
using OICAR.DAL.Repository.Interface;
using OICAR.Models;
using System.Data;

namespace OICAR.DAL.Repository.Implementations
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly string _connectionString;

        public CartItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync()
        {
            var cartItems = new List<CartItem>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetAllCartItems", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        cartItems.Add(MapReaderToCartItem(reader));
                    }
                }
            }

            return cartItems;
        }

        public async Task<CartItem> GetByIdAsync(int id)
        {
            CartItem cartItem = null;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetCartItemById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IDCartItem", id);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        cartItem = MapReaderToCartItem(reader);
                    }
                }
            }

            return cartItem;
        }

        public async Task AddAsync(CartItem entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_CreateCartItem", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CartID", entity.CartId);
                command.Parameters.AddWithValue("@ItemID", entity.ItemId);
                command.Parameters.AddWithValue("@Quantity", entity.Quantity);

                var idCartItemParameter = new SqlParameter("@IDCartItem", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(idCartItemParameter);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                entity.IdcartItem = (int)idCartItemParameter.Value;
            }
        }

        public async Task UpdateAsync(CartItem entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_UpdateCartItem", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IDCartItem", entity.IdcartItem);
                command.Parameters.AddWithValue("@CartID", entity.CartId);
                command.Parameters.AddWithValue("@ItemID", entity.ItemId);
                command.Parameters.AddWithValue("@Quantity", entity.Quantity);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_DeleteCartItem", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IDCartItem", id);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<CartItem>> GetByCartIdAsync(int cartId)
        {
            var cartItems = new List<CartItem>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetCartItemsByCartId", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CartID", cartId);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        cartItems.Add(MapReaderToCartItem(reader));
                    }
                }
            }

            return cartItems;
        }

        private CartItem MapReaderToCartItem(SqlDataReader reader)
        {
            return new CartItem
            {
                IdcartItem = reader.GetInt32(reader.GetOrdinal("IDCartItem")),
                CartId = reader.GetInt32(reader.GetOrdinal("CartID")),
                ItemId = reader.GetInt32(reader.GetOrdinal("ItemID")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
            };
        }
    }
}
