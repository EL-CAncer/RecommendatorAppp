using RecommendatorAppp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendatorAppp.Repositories
{
    public interface IServiceInformationRepository
    {
        IEnumerable<ServiceInformation> ServiceInformation { get; }

        ServiceInformation GetById(int? id);
        Task<ServiceInformation> GetByIdAsync(int? id);

        bool Exists(int id);

        IEnumerable<ServiceInformation> GetAll();
        Task<IEnumerable<ServiceInformation>> GetAllAsync();

        void Add(ServiceInformation serviceInformation);
        void Update(ServiceInformation serviceInformation);
        void Remove(ServiceInformation serviceInformation);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
