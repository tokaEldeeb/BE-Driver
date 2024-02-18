public interface IDriverRepository
{
    IEnumerable<Driver> GetDrivers();
    Driver GetDriver(int id);

    void AddDriver(Driver driver);

    void UpdateDriver(Driver driver);

    void DeleteDriver(int id);
}
