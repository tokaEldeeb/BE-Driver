using Moq;
using NUnit.Framework;

namespace BE_Driver_Test;

public class Tests
{
    private Mock<IDriverRepository> mockRepo;
    private DriverService driverService;
    [SetUp]
    public void Setup()
    {
        mockRepo = new Mock<IDriverRepository>();
        var logger = new Mock<ILogger<DriverService>>();
        driverService = new DriverService(logger.Object, mockRepo.Object);
    }

    [Test]
    public void TestGetDrivers()
    {
        // Arrange
        mockRepo.Setup(repo => repo.GetDrivers()).Returns(new List<Driver>
        {
            new Driver { Id = 1, FirstName = "John", LastName = "Doe", Phone = "+02 1234567890" },
            new Driver { Id = 2, FirstName = "Jane", LastName = "Doe", Phone = "+02 1234567890" }
        });

        // Act
        var drivers = driverService.GetDrivers();

        // Assert
        Assert.That(drivers.Count(), Is.EqualTo(2));
    }

    [Test]
    public void TestGetDriverById()
    {
        // Arrange
        mockRepo.Setup(repo => repo.GetDriver(1)).Returns(new Driver { Id = 1, FirstName = "John", LastName = "Doe", Phone = "+02 1234567890" });

        // Act
        var driver = driverService.GetDriver(1);

        // Assert
        Assert.That(driver.Id, Is.EqualTo(1));
    }

    [Test]
    public void TestAddDriver()
    {
        // Arrange
        var driverDTO = new DriverDTO { FirstName = "John", LastName = "Doe", Phone = "+02 1234567890" };

        // Act
        driverService.AddDriver(driverDTO);

        // Assert
        mockRepo.Verify(repo => repo.AddDriver(It.Is<Driver>(d => d.FirstName == "John" && d.LastName == "Doe" && d.Phone == "+02 1234567890")), Times.Once);
    }

    [Test]
    public void TestUpdateDriver()
    {
        // Arrange
        var driverDTO = new DriverDTO { FirstName = "John", LastName = "Doe", Phone = "+02 1234567890" };

        // Act
        driverService.UpdateDriver(driverDTO, 1);

        // Assert
        mockRepo.Verify(repo => repo.UpdateDriver(It.Is<Driver>(d => d.Id == 1 && d.FirstName == "John" && d.LastName == "Doe" && d.Phone == "+02 1234567890")), Times.Once);
    }

    [Test]
    public void TestDeleteDriver()
    {
        // Arrange
        var driver = new Driver { Id = 1, FirstName = "John", LastName = "Doe", Phone = "+02 1234567890" };

        // Act
        driverService.DeleteDriver(1);

        // Assert
        mockRepo.Verify(repo => repo.DeleteDriver(1), Times.Once);
    }

    [Test]
    public void TestGetDriverNameSorted()
    {
        // Arrange
        mockRepo.Setup(repo => repo.GetDriver(1)).Returns(new Driver { Id = 1, FirstName = "John", LastName = "Doe", Phone = "+02 1234567890" });

        // Act
        var driverName = driverService.GetDriverNameSorted(1);

        // Assert
        Assert.That(driverName, Is.EqualTo("Jhno Deo"));
    }
}