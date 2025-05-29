using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OICAR.DAL.Repository.Interface;
using OICAR.Models;
using System.Data;

namespace OICAR.DAL.Repository.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly string _connectionString;

        public CartRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            var carts = new List<Cart>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetAllCarts", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        carts.Add(MapReaderToCart(reader));
                    }
                }
            }
            return carts;
        }

        public async Task<Cart> GetByIdAsync(int id)
        {
            Cart cart = null;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetCartById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IDCart", id);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        cart = MapReaderToCart(reader);
                    }
                }
            }
            return cart;
        }

        public async Task AddAsync(Cart entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_CreateCart", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", entity.UserId);

                var outputParameter = command.Parameters.Add("@IDCart", SqlDbType.Int);
                outputParameter.Direction = ParameterDirection.Output;

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                entity.Idcart = (int)outputParameter.Value;
            }
        }

        public async Task UpdateAsync(Cart entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_UpdateCart", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IDCart", entity.Idcart);
                command.Parameters.AddWithValue("@UserID", entity.UserId);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_DeleteCart", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IDCart", id);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<Cart> GetByUserIdAsync(int userId)
        {
            Cart cart = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_GetCartByUserId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            cart = new Cart
                            {
                                Idcart = reader.GetInt32(reader.GetOrdinal("IDCart")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserID")),
                                CartItems = new List<CartItem>()
                            };
                        }
                    }
                }

                if (cart != null)
                {
                    using (var command = new SqlCommand("sp_GetCartItemsByCartId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CartID", cart.Idcart);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var cartItem = new CartItem
                                {
                                    IdcartItem = reader.GetInt32(reader.GetOrdinal("IDCartItem")),
                                    ItemId = reader.GetInt32(reader.GetOrdinal("ItemID")),
                                    CartId = reader.GetInt32(reader.GetOrdinal("CartID")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                                };

                                cart.CartItems.Add(cartItem);
                            }
                        }
                    }
                }
            }

            return cart;
        }

        public async Task<bool> IsCartEmptyAsync(int cartId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_CheckIfCartIsEmpty", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IDCart", cartId);

                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();

                return (int)result == 0;
            }
        }

        private Cart MapReaderToCart(SqlDataReader reader)
        {
            return new Cart
            {
                Idcart = reader.GetInt32(reader.GetOrdinal("IDCart")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserID"))
            };
        }
    }
}
