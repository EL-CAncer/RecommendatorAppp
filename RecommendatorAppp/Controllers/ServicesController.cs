using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecommendatorAppp.Models;
using RecommendatorAppp.Repositories;
using Microsoft.AspNetCore.Authorization;
using RecommendatorAppp.ViewModels;
using RecommendatorAppp.Data;

namespace RecommendatorAppp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServicesRepository _ServicesRepo;
        private readonly ICategoryRepository _categoryRepo;

        public ServicesController(ApplicationDbContext context, IServicesRepository ServicesRepo, ICategoryRepository categoryRepo)
        {
            _context = context;
            _ServicesRepo = ServicesRepo;
            _categoryRepo = categoryRepo;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            return View(await _ServicesRepo.GetAllIncludedAsync());
        }

        // GET: Services
        [AllowAnonymous]
        public async Task<IActionResult> ListAll()
        {
            var model = new SearchServicesViewModel()
            {
                ServicesList = await _ServicesRepo.GetAllIncludedAsync(),
                SearchText = null
            };

            return View(model);
        }

        private async Task<List<Services>> GetServicesearchList(string userInput)
        {
            var userInputTrimmed = userInput?.ToLower()?.Trim();

            if (string.IsNullOrWhiteSpace(userInputTrimmed))
            {
                return await _context.Services.Include(p => p.Category)
                    .Select(p => p).OrderBy(p => p.Name)
                    .ToListAsync();
            }
            else
            {
                return await _context.Services.Include(p => p.Category)
                    .Where(p => p
                    .Name.ToLower().Contains(userInputTrimmed))
                    .Select(p => p).OrderBy(p => p.Name)
                    .ToListAsync();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AjaxSearchList(string searchString)
        {
            var ServicesList = await GetServicesearchList(searchString);
            
            return PartialView(ServicesList);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
       

        // GET: Services
        
        public async Task<IActionResult> ListCategory(string categoryName)
        {
            bool categoryExtist = _context.Categories.Any(c => c.Name == categoryName);
            if (!categoryExtist)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);

            if (category == null)
            {
                return NotFound();
            }

            bool anyServices = await _context.Services.AnyAsync(x => x.Category == category);
            if (!anyServices)
            {
                return NotFound($"No Services were found in the category: {categoryName}");
            }

            var Services = _context.Services.Where(x => x.Category == category)
                .Include(x => x.Category).Include(x => x.Reviews);

            ViewBag.CurrentCategory = category.Name;
            return View(Services);
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Services = await _ServicesRepo.GetByIdIncludedAsync(id);

            if (Services == null)
            {
                return NotFound();
            }

            return View(Services);
        }

        // GET: Services/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> DisplayDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Services = await _ServicesRepo.GetByIdIncludedAsync(id);

            var listOfInformation = await _context.ServiceInformation.Where(x => x.ServicesId == id).Select(x => x.Information.Name).ToListAsync();
            ViewBag.ServiceInformation = listOfInformation;

            //var listOfReviews = await _context.Reviews.Where(x => x.ServicesId == id).Select(x => x).ToListAsync();
            //ViewBag.Reviews = listOfReviews;
            double score;
            if (_context.Reviews.Any(x => x.ServicesId == id))
            {
                var review = _context.Reviews.Where(x => x.ServicesId == id);
                score = review.Average(x => x.Grade);
                score = Math.Round(score, 2);
            }
            else
            {
                score = 0;
            }
            ViewBag.AverageReviewScore = score;

            if (Services == null)
            {
                return NotFound();
            }

            return View(Services);
        }

        // GET: Services
        [AllowAnonymous]
        public async Task<IActionResult> SearchServices()
        {
            var model = new SearchServicesViewModel()
            {
                ServicesList = await _ServicesRepo.GetAllIncludedAsync(),
                SearchText = null
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
       

        // GET: Services/Create
        public IActionResult Create()
        {
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,ImageUrl,IsServicesOfTheWeek,CategoriesId")] Services Services)
        {
            if (ModelState.IsValid)
            {
                _ServicesRepo.Add(Services);
                await _ServicesRepo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", Services.CategoriesId);
            return View(Services);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Services = await _ServicesRepo.GetByIdAsync(id);

            if (Services == null)
            {
                return NotFound();
            }
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", Services.CategoriesId);
            return View(Services);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,ImageUrl,IsServicesOfTheWeek,CategoriesId")] Services Services)
        {
            if (id != Services.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ServicesRepo.Update(Services);
                    await _ServicesRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicesExists(Services.Id))
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
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", Services.CategoriesId);
            return View(Services);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Services = await _ServicesRepo.GetByIdIncludedAsync(id);

            if (Services == null)
            {
                return NotFound();
            }

            return View(Services);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Services = await _ServicesRepo.GetByIdAsync(id);
            _ServicesRepo.Remove(Services);
            await _ServicesRepo.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ServicesExists(int id)
        {
            return _ServicesRepo.Exists(id);
        }
    }
}
