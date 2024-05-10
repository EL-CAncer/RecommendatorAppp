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
        IEnumerable<Services> ServicesOfTheWeek { get; }

        Services GetById(int? id);
        Task<Services> GetByIdAsync(int? id);

        Services GetByIdIncluded(int? id);
        Task<Services> GetByIdIncludedAsync(int? id);

        bool Exists(int id);

        IEnumerable<Services> GetAll();
        Task<IEnumerable<Services>> GetAllAsync();

        IEnumerable<Services> GetAllIncluded();
        Task<IEnumerable<Services>> GetAllIncludedAsync();

        void Add(Services pizza);
        void Update(Services pizza);
        void Remove(Services pizza);

        void SaveChanges();
        Task SaveChangesAsync();

    }
}
