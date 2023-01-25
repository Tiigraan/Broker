using Microsoft.AspNetCore.Mvc;
using WebAPIPracticeProject.Data;

namespace WebAPIPracticeProject.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly BrokerDataContext _context;

    public CustomerController(BrokerDataContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Route("CreatePackages")]
    public IActionResult CreateRequest(string json)
    {
        // To do

        return BadRequest();
    }
    
    [HttpPost]
    [Route("UploadFile/{requestId:int}")]
    public IActionResult UploadFile(int requestId)
    {
        // To do

        return BadRequest();
    }
    
    [HttpGet]
    [Route("GetPackagesStatus/{requestId:int}")]
    public IActionResult GetRequestStatus(int requestId)
    {
        // To do
        
        return BadRequest();
    }
}