using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecommendatorAppp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RecommendatorAppp.Data;

namespace RecommendatorAppp.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            var reviews = await _context.Reviews.Include(r => r.Service).Include(r => r.User).ToListAsync();
            return View(reviews);
        }

        // GET: Reviews
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
            {
                var allReviews = _context.Reviews.Include(r => r.Service).Include(r => r.User).ToList();
                return View(allReviews);
            }
            else
            {
                var reviews = _context.Reviews.Include(r => r.Service).Include(r => r.User)
                    .Where(r => r.User == user).ToList();
                return View(reviews);
            }
        }

        // GET: Reviews
        [AllowAnonymous]
        public async Task<IActionResult> ListAll()
        {
            var reviews = await _context.Reviews.Include(r => r.Service).Include(r => r.User).ToListAsync();
            return View(reviews);
        }

        private async Task<List<Reviews>> SortReviews(string sortBy, bool isDescending)
        {
            var reviewsList = _context.Reviews.Include(r => r.Service).Include(r => r.User);
            IQueryable<Reviews> result;

            if (sortBy == null || sortBy == "")
            {
                result = reviewsList;
            }

            if (isDescending == false)
            {
                result = sortBy.ToLower() switch
                {
                    "date" => reviewsList.OrderBy(x => x.Date),
                    "grade" => reviewsList.OrderBy(x => x.Grade),
                    "title" => reviewsList.OrderBy(x => x.Title),
                    
                };
            }
            else
            {
                result = sortBy.ToLower() switch
                {
                    "date" => reviewsList.OrderByDescending(x => x.Date),
                    "grade" => reviewsList.OrderByDescending(x => x.Grade),
                    "title" => reviewsList.OrderByDescending(x => x.Title),
                    
                };
            }

            //Partial view?
            return await result.ToListAsync();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AjaxListReviews(string sortBy, bool isDescending)
        {
            var listOfReviews = await SortReviews(sortBy, isDescending);

            return PartialView(listOfReviews);
        }

        // GET: Reviews
        [AllowAnonymous]
        public async Task<IActionResult> ServicesReviews(int? ServicesId)
        {
            if (ServicesId == null)
            {
                return NotFound();
            }
            var Services = _context.Services.FirstOrDefault(x => x.Id == ServicesId);
            if (Services == null)
            {
                return NotFound();
            }
            var reviews = await _context.Reviews.Include(r => r.Service).Include(r => r.User).Where(x => x.Service.Id == Services.Id).ToListAsync();
            if (reviews == null)
            {
                return NotFound();
            }
            ViewBag.ServicesName = Services.Name;
            ViewBag.ServicesId = Services.Id;

            return View(reviews);
        }

        // GET: Reviews/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r => r.Service)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (reviews == null)
            {
                return NotFound();
            }

            return View(reviews);
        }

        // GET: Reviews/Create
        public IActionResult CreateWithServices(int? ServicesId)
        {
            var review = new Reviews();

            if (ServicesId == null)
            {
                return NotFound();
            }

            var Services = _context.Services.FirstOrDefault(p => p.Id == ServicesId);
            
            if (Services == null)
            {
                return NotFound();
            }

            review.Service = Services;
            review.ServiceId = Services.Id;
            ViewData["ServicesId"] = new SelectList(_context.Services.Where(p => p.Id == ServicesId), "Id", "Name");
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Grade"] = new SelectList(listOfGrades, "Id", "Value");

            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWithServices(int ServicesId, Reviews reviews)
        {
            if (ServicesId != reviews.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                reviews.UserId = userId;
                reviews.Date = DateTime.Now;

                _context.Add(reviews);
                await _context.SaveChangesAsync();
                return Redirect($"ServicesReviews?ServicesId={ServicesId}");
            }
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Grade"] = new SelectList(listOfGrades, "Id", "Value", reviews.Grade);
            ViewData["ServicesId"] = new SelectList(_context.Services.Where(p => p.Id == ServicesId), "Id", "Name", reviews.ServiceId);
            return View(reviews);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Grade"] = new SelectList(listOfGrades, "Id", "Value");
            ViewData["ServicesId"] = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Grade,ServicesId")] Reviews reviews)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                reviews.UserId = userId;

                reviews.Date = DateTime.Now;
                _context.Add(reviews);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Grade"] = new SelectList(listOfGrades, "Id", "Value", reviews.Grade);
            ViewData["ServicesId"] = new SelectList(_context.Services, "Id", "Name", reviews.ServiceId);
            
            return View(reviews);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(user);
            bool isAdmin = userRoles.Any(r => r == "Admin");

            if (reviews == null)
            {
                return NotFound();
            }

            if (isAdmin == false)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                if (reviews.UserId != userId)
                {
                    return BadRequest("You do not have permissions to edit this review.");
                }
            }
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Grade"] = new SelectList(listOfGrades, "Id", "Value", reviews.Grade);
            ViewData["ServicesId"] = new SelectList(_context.Services, "Id", "Name", reviews.ServiceId);
            return View(reviews);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Grade,Date,ServicesId")] Reviews reviews)
        {
            if (id != reviews.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                try
                {
                    if (reviews.Date == DateTime.MinValue)
                    {
                        reviews.Date = DateTime.Now;
                    }
                    reviews.UserId = userId;

                    _context.Update(reviews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewsExists(reviews.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Grade"] = new SelectList(listOfGrades, "Id", "Value", reviews.Grade);
            ViewData["ServicesId"] = new SelectList(_context.Services, "Id", "Name", reviews.ServiceId);
            return View(reviews);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r => r.Service)
                .SingleOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(user);
            bool isAdmin = userRoles.Any(r => r == "Admin");

            if (reviews == null)
            {
                return NotFound();
            }

            if (isAdmin == false)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                if (reviews.UserId != userId)
                {
                    return BadRequest("You do not have permissions to edit this review.");
                }
            }

            return View(reviews);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reviews = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);
            _context.Reviews.Remove(reviews);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ReviewsExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }

    }
}
