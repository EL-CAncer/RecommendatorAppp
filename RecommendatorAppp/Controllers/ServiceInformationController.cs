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
    public class ServiceInformationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceInformationController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ServiceInformation
        public async Task<IActionResult> Index()
        {
            var ApplicationDbContext = _context.ServiceInformation.Include(p => p.Informations).Include(p => p.Services);
            return View(await ApplicationDbContext.ToListAsync());
        }

        // GET: ServiceInformation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ServiceInformation = await _context.ServiceInformation
                .Include(p => p.Informations)
                .Include(p => p.Services)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ServiceInformation == null)
            {
                return NotFound();
            }

            return View(ServiceInformation);
        }

        // GET: ServiceInformation/Create
        public IActionResult Create()
        {
            ViewData["InformationId"] = new SelectList(_context.Information, "Id", "Name");
            ViewData["ServicesId"] = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // POST: ServiceInformation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServicesId,InformationId")] ServiceInformation ServiceInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ServiceInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["InformationId"] = new SelectList(_context.Information, "Id", "Name", ServiceInformation.InformationId);
            ViewData["ServicesId"] = new SelectList(_context.Services, "Id", "Name", ServiceInformation.ServiceId);
            return View(ServiceInformation);
        }

        // GET: ServiceInformation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ServiceInformation = await _context.ServiceInformation.SingleOrDefaultAsync(m => m.Id == id);
            if (ServiceInformation == null)
            {
                return NotFound();
            }
            ViewData["InformationId"] = new SelectList(_context.Information, "Id", "Name", ServiceInformation.InformationId);
            ViewData["ServicesId"] = new SelectList(_context.Services, "Id", "Name", ServiceInformation.ServiceId);
            return View(ServiceInformation);
        }

        // POST: ServiceInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServicesId,InformationId")] ServiceInformation ServiceInformation)
        {
            if (id != ServiceInformation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ServiceInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceInformationExists(ServiceInformation.Id))
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
            ViewData["InformationId"] = new SelectList(_context.Information, "Id", "Name", ServiceInformation.InformationId);
            ViewData["ServicesId"] = new SelectList(_context.Services, "Id", "Name", ServiceInformation.ServiceId);
            return View(ServiceInformation);
        }

        // GET: ServiceInformation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ServiceInformation = await _context.ServiceInformation
                .Include(p => p.Informations)
                .Include(p => p.Services)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ServiceInformation == null)
            {
                return NotFound();
            }

            return View(ServiceInformation);
        }

        // POST: ServiceInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ServiceInformation = await _context.ServiceInformation.SingleOrDefaultAsync(m => m.Id == id);
            _context.ServiceInformation.Remove(ServiceInformation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ServiceInformationExists(int id)
        {
            return _context.ServiceInformation.Any(e => e.Id == id);
        }
    }
}
