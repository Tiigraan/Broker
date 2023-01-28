using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebAPIPracticeProject.Controllers;
using WebAPIPracticeProject.Data;
namespace BrokerWebAPITests;

public static class ControllerCreator
{
    private const string ConnectingString 
        = "Server=localhost;Database=testdb;Port=5432;User Id=testuser;Password=13927";
    
    private static BrokerDataContext CreateDbContext()
    {
        return new BrokerDataContext(
            new DbContextOptionsBuilder<BrokerDataContext>()
                .UseNpgsql(ConnectingString)
                .Options);
    }
    
    // Default file size limit 2Mb
    public static CustomerController CreateCustomerController(long fileSizeLimit = 2097152)
    {
        // Create config and DbContext
        var dbContext = CreateDbContext();
        var config = new ConfigurationManager();
        config.Bind("FileSizeLimit", fileSizeLimit);

        return new CustomerController(dbContext, config);
    }

    public static (CustomerController, BrokerDataContext) CreateCustomerControllerAndGetDbContext(
        long fileSizeLimit = 2097152)
    {
        var dbContext = CreateDbContext();
        var config = new ConfigurationManager();
        config.Bind("FileSizeLimit", fileSizeLimit);
        
        return (new CustomerController(dbContext, config), dbContext);

    }
}