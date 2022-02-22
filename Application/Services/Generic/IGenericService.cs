using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services.Generic
{
    public interface IGenericService
    {
        Task<T> GetById<T>(int id) where T : class;
        Task<List<T>> GetAll<T>() where T : class;

        Task<dynamic> Add<T>(T entity) where T : class;
        Task<dynamic> Update<T>(int id, T body) where T : class;
        Task<bool> Delete<T>(int id) where T : class;

        Task<int> CountAll<T>() where T : class;
    }
}
