using System.Data.SQLite;

public class DriverRepository : IDriverRepository
{
    private readonly ILogger<DriverRepository> _logger;
    private readonly AppDbContext _dbContext;

    public DriverRepository(ILogger<DriverRepository> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public IEnumerable<Driver> GetDrivers()
    {
        _logger.LogInformation("Getting drivers from database");
        using var command = _dbContext.CreateCommand();
        command.CommandText = "SELECT * FROM Driver order by firstName asc";
        using var reader = command.ExecuteReader();
        var drivers = new List<Driver>();
        while (reader.Read())
        {
            _logger.LogInformation($"Reading driver {reader.GetString(1)} {reader.GetString(2)}");
            drivers.Add(new Driver
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Phone = reader.IsDBNull(3) ? "" : reader.GetString(3)
            });
        }
        _logger.LogInformation($"Returning {drivers.Count} drivers");
        return drivers;
    }

    public Driver GetDriver(int id)
    {
        _logger.LogInformation($"Getting driver with id {id} from database");
        using var command = _dbContext.CreateCommand();
        command.CommandText = "SELECT * FROM Driver WHERE id = @id";
        command.Parameters.Add(new SQLiteParameter("@id", id));
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            _logger.LogInformation($"Reading driver {reader.GetString(1)} {reader.GetString(2)}");
            return new Driver
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Phone = reader.IsDBNull(3) ? "" : reader.GetString(3)
            };
        }
        else
        {
            throw new DriverNotFound();
        }
    }

    public void AddDriver(Driver driver)
    {
        _logger.LogInformation($"Adding driver {driver.FirstName} {driver.LastName} to database");
        using var command = _dbContext.CreateCommand();
        command.CommandText = "INSERT INTO Driver (firstName, lastName, phone) VALUES (@firstName, @lastName, @phone)";
        command.Parameters.Add(new SQLiteParameter("@firstName", driver.FirstName));
        command.Parameters.Add(new SQLiteParameter("@lastName", driver.LastName));
        command.Parameters.Add(new SQLiteParameter("@phone", driver.Phone));
        command.ExecuteNonQuery();
    }

    public void UpdateDriver(Driver driver)
    {
        _logger.LogInformation($"Updating driver {driver.FirstName} {driver.LastName} in database");
        using var command = _dbContext.CreateCommand();
        command.CommandText = "UPDATE Driver SET firstName = @firstName, lastName = @lastName, phone = @phone WHERE id = @id";
        command.Parameters.Add(new SQLiteParameter("@firstName", driver.FirstName));
        command.Parameters.Add(new SQLiteParameter("@lastName", driver.LastName));
        command.Parameters.Add(new SQLiteParameter("@phone", driver.Phone));
        command.Parameters.Add(new SQLiteParameter("@id", driver.Id));
        command.ExecuteNonQuery();
    }

    public void DeleteDriver(int id)
    {
        _logger.LogInformation($"Deleting driver with id {id} from database");
        using var command = _dbContext.CreateCommand();
        command.CommandText = "DELETE FROM Driver WHERE id = @id";
        command.Parameters.Add(new SQLiteParameter("@id", id));
        command.ExecuteNonQuery();
    }
}