using System.Net;
using Microsoft.AspNetCore.Mvc;
using WebAPIPracticeProject.Data.Model;
using File = WebAPIPracticeProject.Data.Model.File;

namespace BrokerWebAPITests.CustomerControllerTest;

public static class GetPackageStatusTests
{
    [Fact]
    public static void SendingRequestWithNonexistentPackageId()
    {
        var packageId = 0;
        var controller = ControllerCreator.CreateCustomerController();


        var result = controller.GetPackageStatus(packageId).Result;
        var nfResult = result as NotFoundObjectResult;
        
        
        Assert.NotNull(nfResult);
        Assert.Equal((int)HttpStatusCode.NotFound, nfResult.StatusCode);
        Assert.Equal($"Package with id - {packageId} not found", nfResult.Value);    }
    
    [Theory]
    [InlineData(Status.Waiting)]
    [InlineData(Status.Sent)]
    public static void SuccessGettingStatus(Status status)
    {
        var (controller, dbContext) = ControllerCreator.CreateCustomerControllerAndGetDbContext();
        var package = new Package { Content = "", FileId = 0, Sent = status };        
        
        dbContext.Packages.Add(package);
        dbContext.SaveChanges();
        var result = controller.GetPackageStatus(package.Id).Result;
        var goodResult = result as OkObjectResult;
        dbContext.Packages.Remove(package);
        
        
        Assert.NotNull(goodResult);
        Assert.Equal((int)HttpStatusCode.OK, goodResult.StatusCode);
        Assert.Equal(status, goodResult.Value);
    }
}