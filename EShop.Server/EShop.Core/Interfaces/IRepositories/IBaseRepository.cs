using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Entities;
using EShop.Core.Interfaces.Others;

namespace EShop.Core.Interfaces.IRepositories
{
    public interface IBaseRepository<TModel, TId> where TModel : BaseEntity<TId>
    {
        Task<TModel> GetById(int id);
        Task<IReadOnlyList<TModel>> GetAllList();
        Task<TModel> GetEntityWithSpec(ISpecification<TModel> spec);
        Task<IReadOnlyList<TModel>> ListWithSpec(ISpecification<TModel> spec);
        Task<int> CountWithSpec(ISpecification<TModel> spec);
        IQueryable<TModel> GetAll();
        IQueryable<TModel> GetAllWithSpecAsync(ISpecification<TModel> spec);
        Task Insert(TModel entity);
        Task InsertRange(IEnumerable<TModel> entity);
        void Update(TModel entity);
        void UpdateRange(IEnumerable<TModel> entity);
        void Remove(TModel entity);
        Task RemoveAsync(int id);
        void RemoveRange(List<TModel> entities);
        void Save();
        Task SaveAsync();
    }
}
