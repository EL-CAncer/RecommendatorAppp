using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecommendatorAppp.Models;
using Microsoft.AspNetCore.Authorization;
using RecommendatorAppp.Data;

namespace RecommendatorAppp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InformationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InformationController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Information
        public async Task<IActionResult> Index()
        {
            return View(await _context.Information.ToListAsync());
        }

        // GET: Information/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Information = await _context.Information
                .SingleOrDefaultAsync(m => m.Id == id);
            if (Information == null)
            {
                return NotFound();
            }

            return View(Information);
        }

        // GET: Information/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Information/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Information Information)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Information);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Information);
        }

        // GET: Information/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Information = await _context.Information.SingleOrDefaultAsync(m => m.Id == id);
            if (Information == null)
            {
                return NotFound();
            }
            return View(Information);
        }

        // POST: Information/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Information Information)
        {
            if (id != Information.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Information);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InformationExists(Information.Id))
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
            return View(Information);
        }

        // GET: Information/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Information = await _context.Information
                .SingleOrDefaultAsync(m => m.Id == id);
            if (Information == null)
            {
                return NotFound();
            }

            return View(Information);
        }

        // POST: Information/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Information = await _context.Information.SingleOrDefaultAsync(m => m.Id == id);
            _context.Information.Remove(Information);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InformationExists(int id)
        {
            return _context.Information.Any(e => e.Id == id);
        }
    }
}
