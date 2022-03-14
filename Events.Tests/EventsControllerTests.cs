using AutoMapper;
using Bogus;
using Events.API;
using Events.Core.Entities;
using Events.Core.Paging;
using Events.Core.Repositories;
using Events.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events.Tests
{
    [TestClass]
    public class EventsControllerTests
    {
        private CodeEventsController controller;
        private Mock<IEventRepository> mockEventRepo;

        [TestInitialize]
        public void TestSetUp()
        {
            mockEventRepo = new Mock<IEventRepository>();
            var mockUoW = new Mock<IUnitOfWork>();

            mockUoW.Setup(u => u.EventRepo).Returns(mockEventRepo.Object);

            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            }));

            var mockHttpContext = new Mock<HttpContext>();
            var mockResponse = new Mock<HttpResponse>();
            var headers = new Mock<IHeaderDictionary>();
            mockResponse.SetupGet(r => r.Headers).Returns(headers.Object);
            mockHttpContext.SetupGet(c => c.Response).Returns(mockResponse.Object);

            controller = new CodeEventsController(mockUoW.Object, mapper)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object
                }
            };
        }


        [TestMethod]
        public async Task GetCodeEvent_ShouldReturn_OkObjectResult()
        {
            const int pageSize = 2;
            const int pageNumber = 1;
            const bool includeLectures = false;

            var codeEvents = GetCodeEvents(10).ToList();

            var pagingParams = new PagingParams { PageNumber = pageNumber,PageSize = pageSize };
            var pagingResult = new PagingResult<CodeEvent>(codeEvents, codeEvents.Count(),  pagingParams.PageSize, pagingParams.PageNumber);

            mockEventRepo.Setup(e => e.GetAsync(includeLectures, pagingParams)).ReturnsAsync(pagingResult);

            var actual = await controller.GetCodeEvent(includeLectures, pagingParams);

            var responseHeaders = controller.Response.Headers["X-Pagination"];

            Assert.IsInstanceOfType(actual.Result, typeof(OkObjectResult));
          
            
        }

        private IEnumerable<CodeEvent> GetCodeEvents(int number)
        {
            return DummyData.GenerateEvents.Generate(number);
        }
    }

    public class DummyData
    {
        public static Faker<CodeEvent> GenerateEvents => new Faker<CodeEvent>().Rules((faker, codeEvent) =>
        {
            codeEvent.Id = Guid.NewGuid();
            codeEvent.EventDate = DateTime.Now.AddDays(faker.Random.Int(-50, 0));
            codeEvent.Name = faker.Company.CompanyName();
            codeEvent.Length = faker.Random.Int(0, 200);
            codeEvent.Lectures = GenerateLectures(codeEvent.Id).Generate(faker.Random.Int(0, 5));
            codeEvent.Location = GenerateLocation(codeEvent.Id).Generate();
        });

        public static Faker<Location> GenerateLocation(Guid id) => new Faker<Location>().Rules((faker, location) =>
        {
            location.Id = Guid.NewGuid();
            location.Country = faker.Address.Country();
            location.CityTown = faker.Address.City();
            location.Address = faker.Address.FullAddress();
            location.PostalCode = faker.Address.ZipCode();
            location.CodeEventId = id;
        });

        public static Faker<Lecture> GenerateLectures(Guid id) => new Faker<Lecture>().Rules((faker, lecture) =>
        {
            lecture.Id = Guid.NewGuid();
            lecture.Level = faker.Random.Int(0, 200);
            lecture.Title = faker.Company.CompanyName();
            lecture.CodeEventId = id;
        });
    }
}