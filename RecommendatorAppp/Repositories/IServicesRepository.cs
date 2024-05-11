using RecommendatorAppp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendatorAppp.Repositories
{
    public interface IServicesRepository
    {
        IEnumerable<Services> Services { get; }
        

        Services GetById(int? id);
        Task<Services> GetByIdAsync(int? id);

        Services GetByIdIncluded(int? id);
        Task<Services> GetByIdIncludedAsync(int? id);

        bool Exists(int id);

        IEnumerable<Services> GetAll();
        Task<IEnumerable<Services>> GetAllAsync();

        IEnumerable<Services> GetAllIncluded();
        Task<IEnumerable<Services>> GetAllIncludedAsync();

        void Add(Services services);
        void Update(Services services);
        void Remove(Services services);

        void SaveChanges();
        Task SaveChangesAsync();

    }
}
