using Microsoft.Data.SqlClient;

namespace Project.DataAccess
{
    public class DataAccessLayer
    {
        private readonly string _connectionString;

        public DataAccessLayer(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<List<T>> QueryAsync<T>(string query,  params SqlParameter[] parameters) where T : class, new()
        {
            var mapper = new DataMapper<T>();
            var results = new List<T>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(mapper.Map(reader));
                        }
                    }
                }
            }

            return results;
        }

        public async Task<object> ScalarAsync(string query, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return (await command.ExecuteScalarAsync())!;
                }
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string query, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddRange(parameters);
                            int result = await command.ExecuteNonQueryAsync();
                            transaction.Commit();
                            return result;
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
