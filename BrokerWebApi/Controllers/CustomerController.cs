using Microsoft.AspNetCore.Mvc;

namespace WebAPIPracticeProject.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
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