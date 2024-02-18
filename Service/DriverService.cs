using System.Data;
using System.Data.SQLite;

public class DriverService
{
    private readonly ILogger<DriverService> _logger;
    private readonly IDriverRepository _repository;
    public DriverService(ILogger<DriverService> logger, IDriverRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public IEnumerable<Driver> GetDrivers()
    {
        return _repository.GetDrivers();
    }

    public Driver GetDriver(int id)
    {
        return _repository.GetDriver(id);
    }

    public void AddDriver(DriverDTO driver)
    {
        var updatedDriver = new Driver
        {
            FirstName = driver.FirstName,
            LastName = driver.LastName,
            Phone = driver.Phone
        };
        _repository.AddDriver(updatedDriver);
    }

    public void UpdateDriver(DriverDTO driver, int id)
    {
        var updatedDriver = new Driver
        {
            Id = id,
            FirstName = driver.FirstName,
            LastName = driver.LastName,
            Phone = driver.Phone
        };
        _repository.UpdateDriver(updatedDriver);
    }

    public void DeleteDriver(int id)
    {
        _repository.DeleteDriver(id);
    }
    
    public string GetDriverNameSorted(int id)
    {
        _logger.LogInformation($"Getting driver with id {id} from database");
        var driver = GetDriver(id);
        return $"{sortStrinoAlphapetically(driver.FirstName)} {sortStrinoAlphapetically(driver.LastName)}";
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