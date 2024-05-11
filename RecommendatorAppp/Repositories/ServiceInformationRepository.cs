using Microsoft.EntityFrameworkCore;
using RecommendatorAppp.Data;
using RecommendatorAppp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendatorAppp.Repositories
{
    public class ServiceInformationRepository : IServiceInformationRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceInformationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ServiceInformation> ServiceInformation => _context.ServiceInformation.Include(x => x.Services).Include(x => x.Informations); //include here

        public void Add(ServiceInformation serviceInformation)
        {
            _context.ServiceInformation.Add(serviceInformation);
        }

        public IEnumerable<ServiceInformation> GetAll()
        {
            return _context.ServiceInformation.ToList();
        }

        public async Task<IEnumerable<ServiceInformation>> GetAllAsync()
        {
            return await _context.ServiceInformation.ToListAsync();
        }

        public ServiceInformation GetById(int? id)
        {
            return _context.ServiceInformation.FirstOrDefault(p => p.Id == id);
        }

        public async Task<ServiceInformation> GetByIdAsync(int? id)
        {
            return await _context.ServiceInformation.FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.ServiceInformation.Any(p => p.Id == id);
        }

        public void Remove(ServiceInformation serviceInformation)
        {
            _context.ServiceInformation.Remove(serviceInformation);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(ServiceInformation serviceInformation)
        {
            _context.ServiceInformation.Update(serviceInformation);
        }
    }
}
