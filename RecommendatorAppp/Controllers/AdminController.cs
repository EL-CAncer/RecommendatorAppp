using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RecommendatorAppp.Repositories;
using RecommendatorAppp.Models;
using RecommendatorAppp.Data;

namespace RecommendatorAppp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminRepository _adminRepo;

        public AdminController(ApplicationDbContext context, IAdminRepository adminRepo)
        {
            _context = context;
            _adminRepo = adminRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ClearDatabase()
        {
            _adminRepo.ClearDatabase();
            return RedirectToAction("Index", "Services", null);
        }

        public IActionResult SeedDatabase()
        {
            _adminRepo.ClearDatabase();
            _adminRepo.SeedDatabase();
            return RedirectToAction("Index", "Services", null);
        }

    }
}