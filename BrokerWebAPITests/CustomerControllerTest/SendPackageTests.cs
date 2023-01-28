using System.Net;
using Microsoft.AspNetCore.Mvc;
using File = WebAPIPracticeProject.Data.Model.File;

namespace BrokerWebAPITests.CustomerControllerTest;

public static class SendPackageTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("Type\":\"png\",\"Version\":12}")]
    public static void SendBrokenAndNullPackages(string package)
    {
        var controller = ControllerCreator.CreateCustomerController();
        
        
        // The fileId doesn't make sense in this test
        var result = controller.SendPackage(package, 0).Result;
        var badResult = result as BadRequestObjectResult;
        
        
        Assert.NotNull(badResult);
        Assert.Equal((int)HttpStatusCode.BadRequest, badResult.StatusCode);
        Assert.Equal("Package failed deserialization", badResult.Value);
    }

    [Theory]
    [InlineData("{\"Version\":12}", "Type")]
    [InlineData("{\"Type\":\"png\"}", "Version")]
    public static void SendingPackageWithoutRequiredField(string package, string missingFild)
    {
        var controller = ControllerCreator.CreateCustomerController();

        
        // The fileId doesn't make sense in this test
        var result = controller.SendPackage(package, 0).Result;
        var ueResult = result as UnprocessableEntityObjectResult;
        
        
        Assert.NotNull(ueResult);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, ueResult.StatusCode);
        Assert.Equal(@$"missing field '{missingFild}'", ueResult.Value);
    }

    // Заменить id
    [Fact]
    public static void SendingPackageWithNonexistentFileId()
    {
        var controller = ControllerCreator.CreateCustomerController();
        
        
        var result = controller.SendPackage("{\"Type\":\"png\",\"Version\":12}", 0).Result;
        var nfResult = result as NotFoundObjectResult;
        
        
        Assert.NotNull(nfResult);
        Assert.Equal((int)HttpStatusCode.NotFound, nfResult.StatusCode);
        Assert.Equal("Not found file with id - 0", nfResult.Value);
    }

    // Сделать чистку бд безопаснее
    [Fact]
    public static void SuccessSendingPackage()
    {
        var (controller, dbContext) = ControllerCreator.CreateCustomerControllerAndGetDbContext();
        var file = new File { Content = new byte[1], ContentType = "Image/png"};
        
        
        dbContext.Files.Add(file);
        dbContext.SaveChanges();
        var result = controller.SendPackage("{\"Type\":\"png\",\"Version\":12}", file.Id).Result;
        var goodResult = result as AcceptedResult;
        dbContext.Files.Remove(file);
        dbContext.Packages.Remove(dbContext.Packages.Find(goodResult.Value));
        dbContext.SaveChanges();


        Assert.NotNull(goodResult);
        Assert.Equal((int)HttpStatusCode.Accepted, goodResult.StatusCode);
    }
    
}