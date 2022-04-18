 using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Repository.Generic;
using System;
using AutoMapper;
using Application.Utils;
using Application.DTO.Base;
using Infrastructure.Helpers;

namespace Application.Services.Generic
{
    public class GenericService : IGenericService
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<T> GetById<T>(int id) where T : class
        {
            dynamic castedModel = GetCastedModel<T>();
            dynamic result = await _genericRepository.GetById(id, castedModel);

            return _mapper.Map<T>(result);
        }

        public async Task<T> GetByName<T>(string name) where T : BaseDTO
        {
            dynamic castedModel = GetCastedModel<T>();
            dynamic result = await _genericRepository.FirstOrDefault<T>(f => f.Name == name);

            return _mapper.Map<T>(result);
        }

        public async Task<List<T>> GetAll<T>() where T : class
        {
            dynamic castedModel = GetCastedModel<T>();

            dynamic result = await _genericRepository.GetAll(castedModel);
            return _mapper.Map<List<T>>(result);
        }

        public async Task<PageList<T>> GetAll<T>(PageParams pageParams = null) where T : BaseDTO
        {
            if(pageParams == null) pageParams = new ();

            dynamic castedModel = GetCastedModel<T>();

            dynamic all = await _genericRepository.GetAllPaginated(pageParams, castedModel);

            PageList<T> result = _mapper.Map<PageList<T>>(all);
            
            return Pagination.Map(all, result);
        }

        public async Task<dynamic> Add<T>(T entity) where T : class
        {
            dynamic castedModel = GetCastedModel<T>();

            dynamic model = _mapper.Map(entity, castedModel);

            if (await _genericRepository.Add(model))
            {
                dynamic result = await _genericRepository.GetById(model.Id, castedModel);

                return _mapper.Map(result, GetCastedModel<T>(true));
            }
            return null;
        }

        public async Task<dynamic> Update<T>(int id, T entity) where T : class
        {
            dynamic castedModel = GetCastedModel<T>();

            dynamic model = await _genericRepository.GetById(id, castedModel);
            if (model == null) return null;

            _mapper.Map(entity, model);

            if (await _genericRepository.Update(model))
            {
                dynamic result = await _genericRepository.GetById(id, castedModel);
                return _mapper.Map(result, GetCastedModel<T>(true));
            }
            return null;
        }

        public async Task<bool> Delete<T>(int id) where T : class
        {
            dynamic castedModel = GetCastedModel<T>();

            dynamic model = await _genericRepository.GetById(id, castedModel);
            if (model == null) return false;

            return await _genericRepository.Delete(model);
        }

        public async Task<bool> Exists<T>(int id) where T : BaseDTO
        {
            dynamic castedModel = GetCastedModel<T>();
            return await _genericRepository.Any<T>(t => t.Id == id);
        }

        public async Task<int> CountAll<T>() where T : class
        {
            dynamic castedModel = GetCastedModel<T>();
            return await _genericRepository.CountAll(castedModel);
        }

        private static dynamic GetCastedModel<T>(bool toDTO = false) where T : class
        {
            Type type = toDTO ?
                Mapping.toDTO[typeof(T)] : Mapping.toModel[typeof(T)];

            return Activator.CreateInstance(type);
        }

        
    }
}
