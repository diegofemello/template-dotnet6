using Application.DTO.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Helpers;

namespace Application.Services.Generic
{
    public interface IGenericService
    {
        Task<T> GetById<T>(int id) where T : class;
        Task<T> GetByName<T>(string name) where T : BaseDTO;
        Task<List<T>> GetAll<T>() where T : class;
        Task<PageList<T>> GetAll<T>(PageParams pageParams) where T : BaseDTO;

        Task<dynamic> Add<T>(T entity) where T : class;
        Task<dynamic> Update<T>(int id, T body) where T : class;
        Task<bool> Delete<T>(int id) where T : class;

        Task<bool> Exists<T>(int id) where T : BaseDTO;
        Task<int> CountAll<T>() where T : class;
    }
}
