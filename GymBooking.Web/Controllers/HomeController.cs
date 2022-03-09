using GymBooking.Web.Clients;
using GymBooking.Web.Models;
using GymBooking.Web.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace GymBooking.Web.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBookingClient bookingClient;

        public HomeController(ILogger<HomeController> logger,
                              UserManager<ApplicationUser> userManager,
                              IBookingClient bookingClient)
        {
            _logger = logger;
            this.userManager = userManager;
            this.bookingClient = bookingClient;
        }

       // [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
             var all = await bookingClient.GetAllAsync(CancellationToken.None);
             var name = all.ToList()[0].Name;
            var one = await bookingClient.GetAsync(CancellationToken.None, name);


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}