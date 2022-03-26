using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TinkrCommon
{
    public interface IRepository<T> where T : IEntity
    {
        Task<IReadOnlyCollection<T>> GetAsyncAll();
        Task<IReadOnlyCollection<T>> GetAsyncAll(Expression<Func<T,bool>> filter);
        Task<T> GetAsyncById(Guid id);
        Task<T> GetAsyncById(Expression<Func<T,bool>> filter);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(Guid id);
    }
}