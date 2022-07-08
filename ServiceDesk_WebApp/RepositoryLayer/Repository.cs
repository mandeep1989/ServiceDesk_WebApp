using Microsoft.EntityFrameworkCore;
using ServiceDesk_WebApp.Data;
using ServiceDesk_WebApp.Models;
using System.Linq.Expressions;

namespace ServiceDesk_WebApp.RepositoryLayer
{
    public class Repository : IRepository
    {
        private readonly ServiceDesk_WebAppContext _context;
      
        public Repository(ServiceDesk_WebAppContext context)
        {
            _context = context;
        
        }


        #region GET Methods
        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class,IAudit
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> filters) where T : class, IAudit
        {
            return await _context.Set<T>().Where(x => x.IsDeleted==0).Where(filters).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit
        {
            var data = _context.Set<T>().Where(x => x.IsDeleted == 0);
            foreach (var property in includeProperties)
                await data.Include(property).LoadAsync();

            return await data.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit
        {
            var filteredData = _context.Set<T>().Where(x => x.IsDeleted == 0).Where(filters);
            if (includeProperties != null)
                foreach (var property in includeProperties)
                    await filteredData.Include(property).LoadAsync();


            return await filteredData.ToListAsync();

        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> filters) where T : class, IAudit
        {
            return await _context.Set<T>().Where(x => x.IsDeleted == 0).Where(filters).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit
        {
            foreach (var property in includeProperties)
                await _context.Set<T>().Where(x => x.IsDeleted == 0).Include(property).LoadAsync();

            return await _context.Set<T>().FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit
        {
            var filteredData = _context.Set<T>().Where(x => x.IsDeleted == 0).Where(filters);
            if (includeProperties != null)
                foreach (var property in includeProperties)
                    await filteredData.Include(property).LoadAsync();


            return await filteredData.FirstOrDefaultAsync();
        }

        #endregion


        #region GET Methods Included deleted records
        public async Task<IEnumerable<T>> GetAllDeletedIncludeAsync<T>() where T : class, IAudit
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllDeletedIncludeAsync<T>(Expression<Func<T, bool>> filters) where T : class, IAudit
        {
            return await _context.Set<T>().Where(filters).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllDeletedIncludeAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit
        {
            var data = _context.Set<T>();
            foreach (var property in includeProperties)
                await data.Include(property).LoadAsync();

            return await data.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllDeletedIncludeAsync<T>(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit
        {
            var filteredData = _context.Set<T>().Where(filters);
            if (includeProperties != null)
                foreach (var property in includeProperties)
                    await filteredData.Include(property).LoadAsync();


            return await filteredData.ToListAsync();

        }

        public async Task<T> GetDeletedIncludeAsync<T>(Expression<Func<T, bool>> filters) where T : class, IAudit
        {
            return await _context.Set<T>().Where(filters).FirstOrDefaultAsync();
        }

        public async Task<T> GetDeletedIncludeAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit
        {
            foreach (var property in includeProperties)
                await _context.Set<T>().Include(property).LoadAsync();

            return await _context.Set<T>().FirstOrDefaultAsync();
        }

        public async Task<T> GetDeletedIncludeAsync<T>(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit
        {
            var filteredData = _context.Set<T>().Where(filters);
            if (includeProperties != null)
                foreach (var property in includeProperties)
                    await filteredData.Include(property).LoadAsync();


            return await filteredData.FirstOrDefaultAsync();
        }
        #endregion


        #region Add, Update and Delete Methods
        public async Task<T> AddAsync<T>(T entity, int createdBy) where T : class, IAudit
        {
            entity.CreatedBy = createdBy;
            entity.CreatedOn = DateTime.Now.ToString();
            entity.ModifiedBy = createdBy;
            entity.ModifiedOn = DateTime.Now.ToString();

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;

        }

        public async Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> entities, int createdBy) where T : class, IAudit
        {
            foreach (var entity in entities)
            {
                entity.CreatedBy = createdBy;
                entity.CreatedOn = DateTime.Now.ToString();
                entity.ModifiedBy = createdBy;
                entity.ModifiedOn = DateTime.Now.ToString();
            }

            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return entities;
        }




        public async Task<T> UpdateAsync<T>(T entity, int updatedBy) where T : class, IAudit
        {
            entity.ModifiedBy = updatedBy;
            entity.ModifiedOn = DateTime.Now.ToString();

            _context.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }


        public async Task<bool> DeleteAsync<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;


        }

        public async Task<bool> DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            _context.Set<T>().RemoveRange(entities);
            return await _context.SaveChangesAsync() > 0;

        }

        #region Soft Delete
        public async Task<bool> RemoveAsync<T>(T entity, int updatedBy) where T : class, IAudit
        {
            entity.IsDeleted = 0;
            entity.ModifiedBy = updatedBy;
            entity.ModifiedOn = DateTime.Now.ToString();

            _context.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRangeAsync<T>(IEnumerable<T> entities, int updatedBy) where T : class, IAudit
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = 0;
                entity.ModifiedBy = updatedBy;
                entity.ModifiedOn = DateTime.Now.ToString();
                _context.Update(entity);
            }

            return await _context.SaveChangesAsync() > 0;
        }
        #endregion


        #endregion


        #region Extensions or Miscellaneous Methods

        public async Task<bool> IsExistsAsync<T>(Expression<Func<T, bool>> filter, bool includeDeleted = false) where T : class, IAudit
        {
            if (includeDeleted)
                return await _context.Set<T>().Where(filter).AnyAsync();
            else
                return await _context.Set<T>().Where(x => x.IsDeleted==0).Where(filter).AnyAsync();
        }

        public async Task<int> CountAsync<T>(Expression<Func<T, bool>> filter, bool includeDeleted = false) where T : class, IAudit
        {
            if (includeDeleted)
                return await _context.Set<T>().Where(filter).CountAsync();
            else
                return await _context.Set<T>().Where(x => x.IsDeleted==0).Where(filter).CountAsync();
        }

        #endregion
    }
}
