using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using OICAR.Models;
using OICAR.DAL.Repository.Interface;

namespace OICAR.DAL.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using var command = new SqlCommand("sp_GetAllUsers", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    users.Add(MapToUser(reader));
                }
            }
            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetUserById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@IDUser", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapToUser(reader);
            }

            return null;
        }

        public async Task AddAsync(User entity)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_CreateUser", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = entity.Username;
            command.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = entity.FirstName;
            command.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = entity.LastName;
            command.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = entity.Password;
            command.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary, -1).Value = entity.PasswordSalt;
            command.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = entity.Email;
            command.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 50).Value = entity.PhoneNumber;
            command.Parameters.Add("@IsAdmin", SqlDbType.Bit).Value = entity.IsAdmin;

            var outputParameter = command.Parameters.Add("@IDUser", SqlDbType.Int);
            outputParameter.Direction = ParameterDirection.Output;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            entity.Iduser = (int)outputParameter.Value;
        }

        public async Task UpdateAsync(User entity)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_UpdateUser", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@IDUser", SqlDbType.Int).Value = entity.Iduser;
            command.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = entity.Username;
            command.Parameters.Add("@Password", SqlDbType.NVarChar, 100).Value = entity.Password;
            command.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary, -1).Value = entity.PasswordSalt;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = entity.Email;
            command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = entity.FirstName;
            command.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = entity.LastName;
            command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 50).Value = entity.PhoneNumber;
            command.Parameters.Add("@IsAdmin", SqlDbType.Bit).Value = entity.IsAdmin;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_DeleteUser", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@IDUser", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetUserByUsername", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapToUser(reader);
            }

            return null;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetUserByEmail", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapToUser(reader);
            }

            return null;
        }

        public async Task<IEnumerable<User>> GetAdminsAsync()
        {
            var users = new List<User>();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetAdminUsers", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(MapToUser(reader));
            }

            return users;
        }

        private static User MapToUser(SqlDataReader reader)
        {
            return new User
            {
                Iduser = reader.GetInt32(reader.GetOrdinal("IDUser")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                Password = reader.GetString(reader.GetOrdinal("Password")),
                PasswordSalt = (byte[])reader["PasswordSalt"],
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                IsAdmin = reader.GetBoolean(reader.GetOrdinal("IsAdmin"))
            };
        }
    }
}