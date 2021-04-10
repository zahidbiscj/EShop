using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Entities;
using EShop.Core.Interfaces.IRepositories;
using EShop.Core.Interfaces.Others;
using EShop.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace EShop.Data.Repositories
{
    public class BaseRepository<TModel, TId> : IBaseRepository<TModel, TId> where TModel : BaseEntity<TId>
    {
        protected readonly EShopDbContext _context;
        private readonly DbSet<TModel> _dbSet;
        public BaseRepository(EShopDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TModel>();
        }

        public async Task<TModel> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IReadOnlyList<TModel>> GetAllList()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TModel> GetEntityWithSpec(ISpecification<TModel> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<TModel>> ListWithSpec(ISpecification<TModel> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> CountWithSpec(ISpecification<TModel> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<TModel> ApplySpecification(ISpecification<TModel> spec)
        {
            return SpecificationEvaluator<TModel, TId>.GetQuery(_dbSet.AsQueryable(), spec);
        }

        public IQueryable<TModel> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task Insert(TModel entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task InsertRange(IEnumerable<TModel> entity)
        {
            await _dbSet.AddRangeAsync(entity);
            //await _context.SaveChangesAsync();
        }

        public void Update(TModel entity)
        {
            _dbSet.Update(entity);
        }
        public void UpdateRange(IEnumerable<TModel> entity)
        {
            _dbSet.UpdateRange(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            Delete(entity);
        }

        public void Delete(TModel entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TModel> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
