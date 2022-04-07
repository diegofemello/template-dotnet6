using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Generic
{
    public interface IBaseRepository
    {
        Task<T> GetById<T>(int id, T type = null) where T : BaseEntity;
        Task<T> FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class;

        Task<bool> Add<T>(T entity, T type = null) where T : class;
        Task<bool> AddRange<T>(IEnumerable<T> entity, T type = null) where T : class;
        Task<bool> Update<T>(T entity, T type = null) where T : class;
        Task<bool> Delete<T>(T entity, T type = null) where T : class;
        Task<bool> DeleteRange<T>(IEnumerable<T> entities, T type = null) where T : class;
        Task<bool> SoftDelete<T>(T entity, T type = null) where T : SoftDeleteEntity;

        Task<List<T>> GetAll<T>(T type = null) where T : class;
        Task<IEnumerable<T>> GetWhere<T>(Expression<Func<T, bool>> predicate) where T: class;

        Task<int> CountAll<T>(T type = null) where T : class;
        Task<int> CountWhere<T>(Expression<Func<T, bool>> predicate) where T : class;

        Task<bool> Any<T>(Expression<Func<T, bool>> predicate) where T : class;
    }

    public interface IGenericRepository : IBaseRepository { }

    public interface IHostedRepository : IBaseRepository { }

}
