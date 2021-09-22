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
            return await GetAllWithSpecAsync(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<TModel>> ListWithSpec(ISpecification<TModel> spec)
        {
            return await GetAllWithSpecAsync(spec).ToListAsync();
        }

        public async Task<int> CountWithSpec(ISpecification<TModel> spec)
        {
            return await GetAllWithSpecAsync(spec).CountAsync();
        }

        public IQueryable<TModel> GetAllWithSpecAsync(ISpecification<TModel> spec)
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
        }

        public void Update(TModel entity)
        {
             _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<TModel> entity)
        {
            _dbSet.UpdateRange(entity);
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await GetById(id);
            Remove(entity);
        }

        public void Remove(TModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);
        }

        public void RemoveRange(List<TModel> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
