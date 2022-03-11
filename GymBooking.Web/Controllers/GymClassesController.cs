#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymBooking.Web.Data;
using GymBooking.Web.Models.Entities;
using GymBooking.Web.Clients;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using GymBooking.Web.Extensions;
using AutoMapper;
using GymBooking.Web.Models.ViewModels;
using AutoMapper.QueryableExtensions;

namespace GymBooking.Web.Controllers
{
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        { 
         
            db = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        // GET: GymClasses
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var data = await db.GymClass.ToListAsync();
                var mapped = mapper.Map<IEnumerable<GymClassesViewModel>>(data);
                return View(mapped);
            }

            var userId = userManager.GetUserId(User);

            var model = mapper.ProjectTo<GymClassesViewModel>
                (db.GymClass.Include(g => g.AttendingMembers), new {id= userId});

            var m = db.GymClass.Include(g => g.AttendingMembers).ProjectTo<GymClassesViewModel>(mapper.ConfigurationProvider, new { id = userId });


            return View();
        }

        public async Task<IActionResult> BookingToggle(int? id)
        {
            if (id is null) return BadRequest();

            var userId = userManager.GetUserId(User);
            //Check for null

            //var currentGymClass = await db.GymClass.Include(g => g.AttendingMembers)
            //    .FirstOrDefaultAsync(a => a.Id == id);

            //var attending = currentGymClass?.AttendingMembers
            //                            .FirstOrDefault(a => a.ApplicationUserId == userId);

            var attending = await db.AppUserGymClass.FindAsync(userId, id);

            if(attending == null)
            {
                var booking = new ApplicationUserGymClass
                {
                    ApplicationUserId = userId,
                    GymClassId = (int)id
                };

                db.AppUserGymClass.Add(booking);
            }
            else
            {
                db.Remove(attending);
            }

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await db.GymClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // GET: GymClasses/Create
        public IActionResult Create()
        {
            if(Request.IsAjax())
            {
                return PartialView("CreatePartial");
            }
            return View();
        } 
        
        public IActionResult CreateFetch()
        {
                return PartialView("CreatePartial");
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                db.Add(gymClass);
                await db.SaveChangesAsync();

                if (Request.IsAjax())
                {
                    return PartialView("GymClassPartial", gymClass);
                }

                return RedirectToAction(nameof(Index));
            }

            if (Request.IsAjax())
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return PartialView("CreatePartial", gymClass);
            }


            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await db.GymClass.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(gymClass);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await db.GymClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymClass = await db.GymClass.FindAsync(id);
            db.GymClass.Remove(gymClass);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
            return db.GymClass.Any(e => e.Id == id);
        }
    }
}
