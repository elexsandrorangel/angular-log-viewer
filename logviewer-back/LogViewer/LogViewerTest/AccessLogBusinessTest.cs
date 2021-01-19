using LogViewer.Business.Interfaces;
using LogViewer.Controllers;
using LogViewer.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LogViewerTest
{
    public class AccessLogBusinessTest
    {
   
        [Fact]
        public async Task Should_Upload_Single_File()
        {
            //Arrange
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = @"8.8.8.8 - richard [24/Jan/2020:01:46:32 -0800] ""POST http://cart.example.com/vest HTTP/1.1"" 201 3332 ""http://afternoon.com/thunder"" ""Mozilla/5.0 (Windows NT 10.0; Win64; x64; Xbox; Xbox One) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299""";
            var fileName = "log.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            // Business response
            IEnumerable<AccessLogViewModel> models = new List<AccessLogViewModel>()
            {
                new AccessLogViewModel
                {
                    Active = true,
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; Xbox; Xbox One) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299",
                    RemoteAddress = "8.8.8.8",
                    RemoteUser = "richard",
                    Time = new DateTime(2020,01,24,01,46,32),
                    RequestUrl= "POST http://cart.example.com/vest HTTP/1.1",
                    Status = 201,
                    BytesSent = 3332,
                    HttpReferer = "http://afternoon.com/thunder",
                }
            };
            ResultViewModel resultViewModel = new ResultViewModel { Message = "", Success = true };
            // Set up the business layer
            Mock<IAccessLogBusiness> mockBusiness = new Mock<IAccessLogBusiness>();
            mockBusiness.Setup(b => b.ParseLogEntryAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(resultViewModel));

            // Controller
            LogsController controller = new LogsController(mockBusiness.Object);
            var file = fileMock.Object;

            //Act
            var result = await controller.UploadSingle(new[] { file });

            //Assert
            Assert.NotNull(result);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.IsType<List<ResultViewModel>>(okResult.Value);

            Assert.Equal(new List<ResultViewModel> { resultViewModel }, okResult.Value);
        }


        [Fact]
        public void Should_CreateANew_LogBatch()
        {
            List<AccessLogViewModel> fakeModels = new List<AccessLogViewModel>();
            Random rnd = new Random();

            for (int i = 1; i <= 100; i++)
            {
                var fakeModel = new AccessLogViewModel
                {
                    Id = i,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.Now,
                    RemoteAddress = $"1.1.1.{i}",
                    RemoteUser = "-",
                    BytesSent = rnd.Next(1, 1000),
                    HttpReferer = "",
                    RequestUrl = "http://example.com",
                    UserAgent = "Mozila Firefox",
                    Status = 200,
                    Time = DateTime.UtcNow.AddMinutes(-120)
                };
                fakeModels.Add(fakeModel);
            }

            var business = new Mock<IAccessLogBusiness>();

            business.Setup(b => b.AddAsync(It.IsAny<IEnumerable<AccessLogViewModel>>())).Returns(Task.FromResult(fakeModels.AsEnumerable()));

            var result = business.Object.AddAsync(fakeModels).Result;

            Assert.NotNull(result);

            Assert.Equal(100, fakeModels.Count());
            Assert.Equal(fakeModels.AsEnumerable(), result);
        }

    }
}
