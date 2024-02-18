using System.Data;
using System.Data.SQLite;

public class AppDbContext : IDisposable
{
    private readonly IDbConnection _connection;

    public AppDbContext(string connectionString)
    {
        _connection = new SQLiteConnection(connectionString);
        _connection.Open();
    }

    public IDbCommand CreateCommand()
    {
        return _connection.CreateCommand();
    }
    public string GetConnectionString()
    {
        return _connection.ConnectionString;
    }

    // Implement other methods for database operations as needed

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}
