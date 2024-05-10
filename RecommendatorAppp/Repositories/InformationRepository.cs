using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecommendatorAppp.Models;
using Microsoft.EntityFrameworkCore;
using RecommendatorAppp.Data;

namespace RecommendatorAppp.Repositories
{
    public class InformationRepository : IInformationRepository
    {
        private readonly ApplicationDbContext _context;

        public InformationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Information> Information => _context.Information.Include(x => x.ServiceInformation); //include here

        public void Add(Information information)
        {
            _context.Information.Add(information);
        }

        public IEnumerable<Information> GetAll()
        {
            return _context.Information.ToList();
        }

        public async Task<IEnumerable<Information>> GetAllAsync()
        {
            return await _context.Information.ToListAsync();
        }

        public Information GetById(int? id)
        {
            return _context.Information.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Information> GetByIdAsync(int? id)
        {
            return await _context.Information.FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.Information.Any(p => p.Id == id);
        }

        public void Remove(Information information)
        {
            _context.Information.Remove(information);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Information information)
        {
            _context.Information.Update(information);
        }
    }
}
