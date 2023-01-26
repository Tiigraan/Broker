using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebAPIPracticeProject.Data;
using WebAPIPracticeProject;
using WebAPIPracticeProject.Data.Model;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
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
    [Route("SendFile")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(int))]
    public async Task<IActionResult> SendFile(IFormFile sendFile)
    {
        if (sendFile.Length >= _fileSizeLimit) return BadRequest("The file is too large" );

        using var memoryStream = new MemoryStream();
        await sendFile.CopyToAsync(memoryStream);
        
        var file = new File { Content = memoryStream.ToArray(), ContentType = sendFile.ContentType};
        _context.Files.Add(file);
        await _context.SaveChangesAsync();

        return Accepted(file.Id);
    }
    
    [HttpPost]
    [Route("SendPackage")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(int))]
    public async Task<IActionResult> SendPackage(string jsonPackage, int fileId)
    {
        var package = JsonNode.Parse(jsonPackage);
        if (package is null) return BadRequest("Package failed deserialization");

        foreach (var prop in new[] {"Type", "Version"})
            if (package[prop] is null) return UnprocessableEntity($@"missing field '{prop}'");

        if (await _context.Files.FirstOrDefaultAsync(file => file.Id == fileId) is null)
            return NotFound($@"Not found file with id - {fileId}");

        var newPackage = new Package { Content = jsonPackage, FileId = fileId };
        _context.Packages.Add(newPackage);
        await _context.SaveChangesAsync();
        
        return Accepted(newPackage.Id);
    }
    
    [HttpGet]
    [Route("GetPackagesStatus/{packageId:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Status))]
    public async Task<IActionResult> GetPackageStatus(int packageId)
    {
        var package = await _context.Packages.FirstOrDefaultAsync(package => package.Id ==packageId);
        if (package is null) return NotFound($@"Package with id - {packageId} not found");

        return Ok(package.Sent);
    }
}