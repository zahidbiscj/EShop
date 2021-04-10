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
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> GetById(int id);
        Task<IReadOnlyList<T>> GetAllList();
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListWithSpec(ISpecification<T> spec);
        Task<int> CountWithSpec(ISpecification<T> spec);
        IQueryable<T> GetAll();
        Task Insert(T entity);
        Task InsertRange(IEnumerable<T> entity);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task<int> Count(Expression<Func<T, bool>> filter);
    }
}
