using AutoMapper;
using GymBooking.Web.Data;
using GymBooking.Web.Data.AutoMapper;
using GymBooking.Web.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace GymBooking.Tests.Controllers
{

    [TestClass]
    public class GymClassesControllerTests
    {
        private static Mapper mapper;
        private static ApplicationDbContext context;

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassSetUp(TestContext testContext)
        {
            testContext.WriteLine("GymClasses Test starts");
            mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.ConstructServicesUsing(c => new AttendingResolver(Mock.Of<IHttpContextAccessor>()));
                cfg.AddProfile<GymClassProfile>();
            }));

            //mapper.ConfigurationProvider.AssertConfigurationIsValid();
            context = CreateContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.AddRange(
                     new GymClass
                     {
                         Name = "Uppcomming GymClass",
                         Description = "lorem",
                         Duration = new TimeSpan(0, 45, 0),
                         StartDate = DateTime.Now.AddDays(1)

                     }, new GymClass
                     {
                         Name = "Present GymClass",
                         Description = "lorem",
                         Duration = new TimeSpan(0, 45, 0),
                         StartDate = DateTime.Now.AddDays(-1)

                     });

            context.SaveChanges();

        }



        [TestInitialize]
        public void TestSetUp()
        {
            TestContext.WriteLine($"TestInit starts");
        }

        [TestMethod]
        public async Task Index_NotAuthorized_ShouldReturnIndexViewModel()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");



        }

        private static ApplicationDbContext CreateContext()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                                                            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-Booking.Tests;Trusted_Connection=True;MultipleActiveResultSets=true")
                                                            .Options;

            return new ApplicationDbContext(contextOptions);
        }


    }
}