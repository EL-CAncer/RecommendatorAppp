using RecommendatorAppp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendatorAppp.Repositories
{
    public interface IInformationRepository
    {
        IEnumerable<Information> Information { get; }

        Information GetById(int? id);
        Task<Information> GetByIdAsync(int? id);

        bool Exists(int id);

        IEnumerable<Information> GetAll();
        Task<IEnumerable<Information>> GetAllAsync();

        void Add(Information information);
        void Update(Information information);
        void Remove(Information information);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
