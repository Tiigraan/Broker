using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIPracticeProject.Data;
using WebAPIPracticeProject.Data.Model;
using File = System.IO.File;

namespace WebAPIPracticeProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ExecutorController : ControllerBase
{
    private readonly BrokerDataContext _context;
    
    public ExecutorController(BrokerDataContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [Route("GetNewPackagesAndFiles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> GetNewPackagesAndFiles()
    {
        var newPackages = await _context.Packages
            .Where(p => p.Sent == Status.Waiting)
            .ToArrayAsync();
        
        if (newPackages.Length == 0)
            return NoContent();

        var result = JsonSerializer.Serialize(new
        {
            PackageIds = newPackages.Select(p => p.Id).ToArray(),
            FileIds = newPackages.Select(p => p.FileId).ToArray()
        });
        
        return Ok(result);
    }

    // Можно добавить проверку на статус файла, если он уже был отправлен, то сообщать об этом...
    [HttpGet]
    [Route("GetPackageById/{packageId:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> GetPackageById(int packageId)
    {
        var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == packageId);
        if (package is null) return NotFound();

        package.Sent = Status.Sent;
        await _context.SaveChangesAsync();

        return Ok(package.Content);
    }
    
    [HttpGet]
    [Route("GetFile/{fileId:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(File))]
    public async Task<IActionResult> GetFileById(int fileId)
    {
        var file = await _context.Files.FirstOrDefaultAsync(f => f.Id == fileId);
        
        return file is null ? NotFound() : Ok(File(file.Content, file.ContentType));
    }
}