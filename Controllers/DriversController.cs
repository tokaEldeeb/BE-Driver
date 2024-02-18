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

    [HttpPost]
    public IActionResult Post(Driver driver)
    {
        _driverService.AddDriver(driver);
        return CreatedAtRoute("GetDrivers", null);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Driver driver, int id)
    {
        _driverService.updateDriver(driver, id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _driverService.deleteDriver(id);
        return NoContent();
    }
}