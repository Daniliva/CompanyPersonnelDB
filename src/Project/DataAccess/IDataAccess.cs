using Microsoft.Data.SqlClient;

namespace Project.DataAccess;

/// <summary>The only door to the database. Services depend on this interface so that they can be tested without SQL Server.</summary>
public interface IDataAccess
{
    /// <summary>Runs a SELECT and maps every row to <typeparamref name="T"/>.</summary>
    Task<List<T>> QueryAsync<T>(string query, params SqlParameter[] parameters) where T : class, new();

    /// <summary>Runs a query and returns the first column of the first row (null when there are no rows, <see cref="DBNull"/> for SQL NULL).</summary>
    Task<object?> ScalarAsync(string query, params SqlParameter[] parameters);

    /// <summary>Runs INSERT/UPDATE/DELETE in a transaction and returns the number of affected rows.</summary>
    Task<int> ExecuteNonQueryAsync(string query, params SqlParameter[] parameters);
}
