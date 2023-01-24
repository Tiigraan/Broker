using System.Net;
using Microsoft.EntityFrameworkCore;
using WebAPIPracticeProject.Data.Model;
using File = WebAPIPracticeProject.Data.Model.File;

namespace WebAPIPracticeProject.Data;

public class BrokerDataContext : DbContext
{
    public BrokerDataContext(DbContextOptions<BrokerDataContext> options) :
        base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();
    }

    public DbSet<Package> Packages { get; set; }
    
    public DbSet<File> Files { get; set; }
}