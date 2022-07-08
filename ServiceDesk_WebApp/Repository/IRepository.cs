
using ServiceDesk_WebApp.Models;
using System.Linq.Expressions;

namespace Repository
{
    public interface IBaseRepository
    {
        #region GET Methods
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class, IAudit;
        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> filters) where T : class, IAudit;
        Task<IEnumerable<T>> GetAllAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit;
        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit;
        Task<T> GetAsync<T>(Expression<Func<T, bool>> filters) where T : class, IAudit;
        Task<T> GetAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit;
        Task<T> GetAsync<T>(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit;
        #endregion

        #region GET Methods Including deleted records
        Task<IEnumerable<T>> GetAllDeletedIncludeAsync<T>() where T : class, IAudit;
        Task<IEnumerable<T>> GetAllDeletedIncludeAsync<T>(Expression<Func<T, bool>> filters) where T : class, IAudit;

        Task<IEnumerable<T>> GetAllDeletedIncludeAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit;

        Task<IEnumerable<T>> GetAllDeletedIncludeAsync<T>(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit;

        Task<T> GetDeletedIncludeAsync<T>(Expression<Func<T, bool>> filters) where T : class, IAudit;

        Task<T> GetDeletedIncludeAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit;

        Task<T> GetDeletedIncludeAsync<T>(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includeProperties) where T : class, IAudit;
        #endregion

        #region Add, Update and Delete Methods
        Task<T> AddAsync<T>(T entity, int createdBy) where T : class, IAudit;
        Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> entities, int createdBy) where T : class, IAudit;
        Task<T> UpdateAsync<T>(T entity, int updatedBy) where T : class, IAudit;
        Task<bool> DeleteAsync<T>(T entity) where T : class;
        Task<bool> DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class;

        #region Soft Delete
        Task<bool> RemoveAsync<T>(T entity, int updatedBy) where T : class, IAudit;
        Task<bool> RemoveRangeAsync<T>(IEnumerable<T> entities, int updatedBy) where T : class, IAudit;
        #endregion

        #endregion


        #region Extensions or Miscellaneous Methods
        Task<bool> IsExistsAsync<T>(Expression<Func<T, bool>> filter, bool includeDeleted = false) where T : class, IAudit;

        Task<int> CountAsync<T>(Expression<Func<T, bool>> filter, bool includeDeleted = false) where T : class, IAudit;

        #endregion
    }
}