using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrokerWebAPITests.CustomerControllerTest;

public static class SendFileTests
{
    
    [Fact]
    public static void SendingLargeFile()
    {
        long fileSizeLimit = 200;
        IFormFile file = new FormFile(
            new MemoryStream(),
            0,
            fileSizeLimit + 1,
            "FakeFileForTest",
            "FakeFileForTest");
        var controller = ControllerCreator.CreateCustomerController(fileSizeLimit);

        
        var result = controller.SendFile(file).Result;
        var badResult = result as BadRequestObjectResult;
        
        
        Assert.NotNull(badResult);
        Assert.Equal(400, badResult.StatusCode);
        Assert.Equal("The file is too large", badResult.Value);
    }

    // Dont work!!!
    [Fact]
    public static void SendingFileAcceptableSize()
    {
        long fileSizeLimit = 200;
        IFormFile file = new FormFile(
            new MemoryStream(),
            0,
            fileSizeLimit - 1,
            "FakeFileForTest",
            "FakeFileForTest");
        var controller = ControllerCreator.CreateCustomerController(fileSizeLimit);

        
        var result = controller.SendFile(file).Result;
        var goodResult = result as AcceptedResult;
        
        
        Assert.NotNull(goodResult);
        Assert.Equal((int)HttpStatusCode.Accepted, goodResult.StatusCode);
        Assert.IsType<int>(goodResult.Value);
    }
}