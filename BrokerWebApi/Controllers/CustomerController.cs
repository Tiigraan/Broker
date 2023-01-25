using Microsoft.AspNetCore.Mvc;
using WebAPIPracticeProject.Data;
using WebAPIPracticeProject;
using WebAPIPracticeProject.Data.Model;
using File = WebAPIPracticeProject.Data.Model.File;

namespace WebAPIPracticeProject.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly BrokerDataContext _context;

    private readonly long _fileSizeLimit;
    
    public CustomerController(BrokerDataContext context, IConfiguration config)
    {
        _context = context;
        _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
    }
    
    [HttpPost]
    [Route("CreatePackages")]
    public IActionResult CreateRequest(string json)
    {
        // To do

        return BadRequest();
    }
    
    [HttpPost]
    [Route("SendFile")]
    public async Task<IActionResult> SendFile(IFormFile sendFile)
    {
        if (sendFile.Length >= _fileSizeLimit)
            return BadRequest(new { message = "The file is too large" });

        using var memoryStream = new MemoryStream();
        await sendFile.CopyToAsync(memoryStream);
        
        var file = new File { Content = memoryStream.ToArray() };
        _context.Files.Add(file);
        await _context.SaveChangesAsync();

        return Ok(new { fileId = file.Id });
    }
    
    [HttpGet]
    [Route("GetPackagesStatus/{requestId:int}")]
    public IActionResult GetRequestStatus(int requestId)
    {
        // To do
        
        return BadRequest();
    }
}