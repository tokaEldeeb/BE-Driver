using Microsoft.AspNetCore.Mvc;

namespace BE_Driver.Drivers;
[ApiController]
[Route("[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly DriverService _driverService;

    public DriversController(ILogger<DriversController> logger, DriverService service)
    {
        _logger = logger;
        _driverService = service;
    }

    [HttpGet(Name = "GetDrivers")]
    public IEnumerable<Driver> Get()
    {
        var drivers = _driverService.GetDrivers();
        return drivers;
    }

    [HttpGet("{id}")]
    public Driver GetOne(int id)
    {
        var driver = _driverService.GetDriver(id);
        return driver;
    }
    [HttpGet("{id}/sorted")]
    public string GetOneAlphapetized(int id)
    { 
        var driver = _driverService.GetDriverNameSorted(id);
        return driver;
    }

    [HttpPost(Name = "PostDrivers")]
    public IActionResult Post(DriverDTO driver)
    {
        _driverService.AddDriver(driver);
        return CreatedAtRoute("PostDrivers", null);
    }

    [HttpPost("Random", Name = "CreateRandomly")]
    public IActionResult CreateRandomly()
    {
        _driverService.CreateRandomDrivers(10);
        return CreatedAtRoute("CreateRandomly", null);
    }

    [HttpPut("{id}")]
    public IActionResult Put(DriverDTO driver, int id)
    {
        _driverService.UpdateDriver(driver, id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _driverService.DeleteDriver(id);
        return NoContent();
    }
}