using AutoMapper;
using GymBooking.Tests.Helpers;
using GymBooking.Web.Controllers;
using GymBooking.Web.Data;
using GymBooking.Web.Data.AutoMapper;
using GymBooking.Web.Models.Entities;
using GymBooking.Web.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking.Tests.Controllers
{

    [TestClass]
    public class GymClassesControllerTests
    {
        private static Mapper mapper;
        private static ApplicationDbContext context;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private GymClassesController controller;

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

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            controller = new GymClassesController(context, mockUserManager.Object, mapper);

            //För att mocka Delete/Create/Update
            //var ls = new List<ApplicationUser>();
            //mockUserManager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            //mockUserManager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());
            //mockUserManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            //mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<ApplicationUser, string>((a,p) => ls.Add(a));
            //mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
        }

        [TestMethod]
        public async Task Index_NotAuthorized_ShouldReturnIndexViewModel()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");

            controller.SetUserIsAutenticated(false);
            var result = (ViewResult)(await controller.Index(new IndexViewModel { ShowHistory = false }));

            Assert.IsInstanceOfType(result.Model, typeof(IndexViewModel));

        }

        [TestMethod]
        public async Task Index_Authorized_ShouldNotReturnHistory()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");

            controller.SetUserIsAutenticated(true);
            var result = (ViewResult)(await controller.Index(new IndexViewModel { ShowHistory = false }));

            var history = ((IndexViewModel)result.Model).GymClasses.Where(g => g.StartDate < DateTime.Now).Count();


            Assert.IsInstanceOfType(result.Model, typeof(IndexViewModel));
            Assert.AreEqual(0, history);

            //CollectionAssert
            //StringAssert
        }

        [TestMethod]
        public void Create_ReturnsDefaultView_ShouldReturnNull()
        {
            controller.SetAjaxRequest(false);

            var actual = controller.Create() as ViewResult;

            Assert.IsNull(actual.ViewName);
        } 
        
        [TestMethod]
        public void Create_WithAjaxReturnsPartialView_ShouldNotReturnNull()
        {
            controller.SetAjaxRequest(true);
            const string correctPartialViewName = "CreatePartial";

            var actual = controller.Create() as PartialViewResult;


            Assert.IsNotNull(actual.ViewName);
            Assert.AreEqual(correctPartialViewName, actual.ViewName);
        }

        [TestMethod]
        public async Task Create_WithCorrectModel_ShouldSaveNewGymClass()
        {
            const string name = "TestGym";
            controller.SetAjaxRequest(true);

            var actual = await controller.Create(new GymClass
            {
                StartDate = DateTime.Now.AddDays(1),
                Duration = new TimeSpan(0, 55, 0),
                Name = name
            }) as ViewResult;

            context.ChangeTracker.Clear();
            var saved = context.GymClass.Single(g => g.Name == name);

            context.GymClass.Remove(saved);
            await context.SaveChangesAsync();

            Assert.AreEqual(name, saved.Name);
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