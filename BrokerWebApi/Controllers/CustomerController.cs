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
    public async Task<IActionResult> SendFile(IFormFile sendFile)
    {
        if (sendFile.Length >= _fileSizeLimit) return BadRequest("The file is too large" );

        using var memoryStream = new MemoryStream();
        await sendFile.CopyToAsync(memoryStream);
        
        var file = new File { Content = memoryStream.ToArray(), ContentType = sendFile.ContentType};
        _context.Files.Add(file);
        await _context.SaveChangesAsync();

        return Ok(new { fileId = file.Id});
    }
    
    [HttpPost]
    [Route("SendPackage")]
    public async Task<IActionResult> SendPackage(string jsonPackage, int fileId)
    {
        var package = JsonNode.Parse(jsonPackage);
        if (package is null) return BadRequest("Package failed deserialization");

        foreach (var prop in new[] {"Type", "Version"})
            if (package[prop] is null) return BadRequest($@"missing field '{prop}'");

        if (await _context.Files.FirstOrDefaultAsync(file => file.Id == fileId) is null)
            return NotFound($@"Not found file with id - {fileId}");

        var newPackage = new Package { Content = jsonPackage, FileId = fileId };
        _context.Packages.Add(newPackage);
        await _context.SaveChangesAsync();
        
        return Ok(new {packageId = newPackage.Id});
    }
    
    [HttpGet]
    [Route("GetPackagesStatus/{packageId:int}")]
    public async Task<IActionResult> GetPackageStatus(int packageId)
    {
        var result = await _context.Packages.FirstOrDefaultAsync(package => package.Id ==packageId);
        if (result is null) return NotFound($@"Package with id - {packageId} not found");

        return Ok(new {PackageStatus= result.Sent});
    }
}