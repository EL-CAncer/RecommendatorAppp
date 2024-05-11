using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RecommendatorAppp.Data;
using RecommendatorAppp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecommendatorAppp.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public AdminRepository(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public void SeedDatabase()
        {
            var _roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = _serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var cat1 = new Categories { Name = "Standard", Description = "The Plumer's Standard Services all year around." };
            var cat2 = new Categories { Name = "Spcialities", Description = "The Plumer's Speciality Services only for a limited time." };
            

            var cats = new List<Categories>()
            {
                cat1, cat2, 
            };

            var piz1 = new Services { Name = "Plumer", Price = 70.00M, Category = cat1, Description = "ready to prepare.", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/2a/Pizza_capricciosa.jpg",  };
            
           

            var pizs = new List<Services>()
            {
                piz1, 
            };

            var user1 = new IdentityUser { UserName = "user1@gmail.com", Email = "user1@gmail.com" };
           

            string userPassword = "Password123";

            var users = new List<IdentityUser>()
            {
                user1, 
            };

            foreach (var user in users)
            {
                _userManager.CreateAsync(user, userPassword).Wait();
            }

            var revs = new List<Reviews>()
            {
                new Reviews { User = user1, Title ="Best Pizza with mushrooms", Description="I love this Pizza with mushrooms on it.", Grade=4, Date=DateTime.Now },
               
            };

            var ing1 = new Information { Name = "very good" };
           

            var ings = new List<Information>()
            {
                ing1, 
            };

            var pizIngs = new List<ServiceInformation>()
            {
                new ServiceInformation { Informations = ing1, Services = piz1 },
               
            };

           

            

          

            _context.Categories.AddRange(cats);
            _context.Services.AddRange(pizs);
            _context.Reviews.AddRange(revs);
            _context.Information.AddRange(ings);
            _context.ServiceInformation.AddRange(pizIngs);

            _context.SaveChanges();
        }

        public void ClearDatabase()
        {
            var ServiceInformation = _context.ServiceInformation.ToList();
            _context.ServiceInformation.RemoveRange(ServiceInformation);

            var Information = _context.Information.ToList();
            _context.Information.RemoveRange(Information);

            var reviews = _context.Reviews.ToList();
            _context.Reviews.RemoveRange(reviews);

           

            var users = _context.Users.ToList();
            var userRoles = _context.UserRoles.ToList();

            foreach (var user in users)
            {
                if (!userRoles.Any(r => r.UserId == user.Id))
                {
                    _context.Users.Remove(user);
                }
            }

           

           

            var Services = _context.Services.ToList();
            _context.Services.RemoveRange(Services);

            var categories = _context.Categories.ToList();
            _context.Categories.RemoveRange(categories);

            _context.SaveChanges();
        }

    }
}
