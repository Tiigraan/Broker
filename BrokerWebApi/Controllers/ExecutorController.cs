using Microsoft.AspNetCore.Mvc;

namespace WebAPIPracticeProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ExecutorController : ControllerBase
{
    [HttpGet]
    [Route("GetNewPackages")]
    public IActionResult GetNewPackages()
    {
        // To do
        
        return NotFound();
    }

    [HttpGet]
    [Route("GetPackage/{packageId:int}")]
    public IActionResult GetPackage(int packageId)
    {
        // To do
        
        return NotFound();
    }
    
    [HttpGet]
    [Route("GetFile/{fileId:int}")]
    public IActionResult GetFile(int fileId)
    {
        // To do
        
        return NotFound();
    }
}