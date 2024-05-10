using Microsoft.EntityFrameworkCore;
using RecommendatorAppp.Data;
using RecommendatorAppp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendatorAppp.Repositories
{
    public class ServicesRepository : IServicesRepository
    {
        private readonly ApplicationDbContext _context;

        public ServicesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Services> Services => _context.Services.Include(p => p.Category).Include(p => p.Reviews).Include(p => p.ServiceInformation); //include here

        public IEnumerable<Services> ServicesOfTheWeek => _context.Services.Where(p => p.IsPizzaOfTheWeek).Include(p => p.Category);

        public void Add(Services pizza)
        {
            _context.Add(pizza);
        }

        public IEnumerable<Services> GetAll()
        {
            return _context.Services.ToList();
        }

        public async Task<IEnumerable<Services>> GetAllAsync()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<IEnumerable<Services>> GetAllIncludedAsync()
        {
            return await _context.Services.Include(p => p.Category).Include(p => p.Reviews).Include(p => p.ServiceInformation).ToListAsync();
        }

        public IEnumerable<Services> GetAllIncluded()
        {
            return _context.Services.Include(p => p.Category).Include(p => p.Reviews).Include(p => p.ServiceInformation).ToList();
        }

        public Services GetById(int? id)
        {
            return _context.Services.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Services> GetByIdAsync(int? id)
        {
            return await _context.Services.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Services GetByIdIncluded(int? id)
        {
            return _context.Services.Include(p => p.Category).Include(p => p.Reviews).Include(p => p.ServiceInformation).FirstOrDefault(p => p.Id == id);
        }

        public async Task<Services> GetByIdIncludedAsync(int? id)
        {
            return await _context.Services.Include(p => p.Category).Include(p => p.Reviews).Include(p => p.ServiceInformation).FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.Services.Any(p => p.Id == id);
        }

        public void Remove(Services pizza)
        {
            _context.Remove(pizza);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Services pizza)
        {
            _context.Update(pizza);
        }

    }
}
