using System.Data;
using System.Data.SQLite;

public class DriverService
{
    private readonly ILogger<DriverService> _logger;
    private readonly AppDbContext _dbContext;

    public DriverService(ILogger<DriverService> logger, AppDbContext dbContext)
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
        return null;
    }

    public string GetDriverNameSorted(int id)
    {
        _logger.LogInformation($"Getting driver with id {id} from database");
        var driver = GetDriver(id);
        if (driver != null)
        {
            return $"{sortStrinoAlphapetically(driver.FirstName)} {sortStrinoAlphapetically(driver.LastName)}";
        }
        return null;
    }
    public void AddDriver(DriverDTO driver)
    {
        _logger.LogInformation($"Adding driver {driver.FirstName} {driver.LastName} with phone {driver.Phone} to database");
        using var command = _dbContext.CreateCommand();
        command.CommandText = "INSERT INTO Driver (firstName, lastName, phone) VALUES (@firstName, @lastName, @phone)";
        command.Parameters.Add(new SQLiteParameter("@firstName", driver.FirstName));
        command.Parameters.Add(new SQLiteParameter("@lastName", driver.LastName));
        command.Parameters.Add(new SQLiteParameter("@phone", driver.Phone));
        command.ExecuteNonQuery();
    }

    public void updateDriver(DriverDTO driver, int id)
    {
        _logger.LogInformation($"Updating driver {driver.FirstName} {driver.LastName} with phone {driver.Phone} to database");
        using var command = _dbContext.CreateCommand();
        command.CommandText = "UPDATE Driver SET firstName = @firstName, lastName = @lastName, phone = @phone WHERE id = @id";
        command.Parameters.Add(new SQLiteParameter("@firstName", driver.FirstName));
        command.Parameters.Add(new SQLiteParameter("@lastName", driver.LastName));
        command.Parameters.Add(new SQLiteParameter("@phone", driver.Phone));
        command.Parameters.Add(new SQLiteParameter("@id", id));
        command.ExecuteNonQuery();
    }

    public void deleteDriver(int id)
    {
        _logger.LogInformation($"Deleting driver with id {id} from database");
        using var command = _dbContext.CreateCommand();
        command.CommandText = "DELETE FROM Driver WHERE id = @id";
        command.Parameters.Add(new SQLiteParameter("@id", id));
        command.ExecuteNonQuery();
    }

    public void CreateRandomDrivers(int count)
    {
        _logger.LogInformation($"Creating {count} random drivers");

        for (int i = 0; i < count; i++)
        {
            var driver = new DriverDTO
            {
                FirstName = getRandomName(),
                LastName = getRandomName(),
            };
            AddDriver(driver);
        }
    }

    private string getRandomName()
    {
        var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();
        var name = "";
        for (int i = 0; i < 10; i++)
        {
            name += alphabet[random.Next(alphabet.Length)];
        }
        return name;
    }
 
 private string sortStrinoAlphapetically(string str)
    {
        char[] arr = str.ToCharArray();
        Array.Sort(arr);
        return new string(arr);
    }
}