using LogViewer.Business.Interfaces;
using LogViewer.Controllers;
using LogViewer.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace LogViewerTest
{
    public class LogsControllerTest
    {
        private readonly LogsController _controller;
        private readonly Mock<IAccessLogBusiness> _business;

        private List<AccessLogViewModel> _accessLogs;

        public LogsControllerTest()
        {
            #region AccessLOg
            _accessLogs = new List<AccessLogViewModel>
            {
                new AccessLogViewModel
                {
                    Id = 1,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.Now,
                    RemoteAddress = "1.1.1.1",
                    RemoteUser = "-",
                    BytesSent = 1000,
                    HttpReferer = "",
                    RequestUrl = "http://example.com",
                    UserAgent = "Mozila Firefox",
                    Status = 200,
                    Time = DateTime.UtcNow.AddMinutes(-120)
                },
                new AccessLogViewModel
                {
                    Id = 2,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.Now,
                    RemoteAddress = "1.1.1.1",
                    RemoteUser = "teste",
                    BytesSent = 1000,
                    HttpReferer = "",
                    RequestUrl = "http://example2.com",
                    UserAgent = "Google Chrome",
                    Status = 400,
                    Time = DateTime.UtcNow.AddMinutes(-110)
                },
                new AccessLogViewModel
                {
                    Id = 3,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.Now,
                    RemoteAddress = "1.1.1.1",
                    RemoteUser = "alx",
                    BytesSent = 1000,
                    HttpReferer = "",
                    RequestUrl = "http://example.com",
                    UserAgent = "Mozila Firefox",
                    Status = 200,
                    Time = DateTime.UtcNow.AddMinutes(-10)
                }
            };
            #endregion

            // Set up the business layer
            _business = new Mock<IAccessLogBusiness>();
            _business.Setup(b => b.GetAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(_accessLogs.AsEnumerable()));

            _controller = new LogsController(_business.Object);
        }

        #region Get

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            var result = _controller.Get().Result;
            var okResult = result as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }


        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            var okResult = _controller.Get().Result as OkObjectResult;

            var items = Assert.IsType<List<AccessLogViewModel>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        #endregion

        #region Create

        [Fact]
        public void Post_WhenCalled_CreateANewLog()
        {
            var fakeModel = new AccessLogViewModel
            {
                Id = 4,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.Now,
                RemoteAddress = "1.1.1.1",
                RemoteUser = "-",
                BytesSent = 1000,
                HttpReferer = "",
                RequestUrl = "http://example.com",
                UserAgent = "Mozila Firefox",
                Status = 200,
                Time = DateTime.UtcNow.AddMinutes(-120)
            };

            var business = new Mock<IAccessLogBusiness>();
            business.Setup(b => b.GetAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(_accessLogs.AsEnumerable()));
            business.Setup(b => b.AddAsync(It.IsAny<AccessLogViewModel>())).Returns(Task.FromResult(fakeModel));

            var controller = new LogsController(business.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var result = controller.CreateAsync(fakeModel).Result as CreatedResult;

            Assert.NotNull(result);

            Assert.IsType<AccessLogViewModel>(result.Value);
            Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)result.StatusCode);
            Assert.Equal(fakeModel.Id, ((AccessLogViewModel)result.Value).Id);
            Assert.True(((AccessLogViewModel)result.Value).Active);
        }

        #endregion
    }
}
