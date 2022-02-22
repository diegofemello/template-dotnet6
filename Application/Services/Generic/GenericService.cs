using Domain.Model.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Repository.Generic;
using System;
using AutoMapper;
using Application.Utils;
using Domain.Model;
using System.Linq.Expressions;

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

        public async Task<List<T>> GetAll<T>() where T : class
        {
            dynamic castedModel = GetCastedModel<T>();

            dynamic result = await _genericRepository.GetAll(castedModel);
            return _mapper.Map<List<T>>(result);
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

        public async Task<int> CountAll<T>() where T : class
        {
            dynamic castedModel = GetCastedModel<T>();
            return await _genericRepository.CountAll(castedModel);
        }

        private static dynamic GetCastedModel<T>(bool toVO = false) where T : class
        {
            Type type = toVO ?
                Mapping.toVO[typeof(T)] : Mapping.toModel[typeof(T)];

            return Activator.CreateInstance(type);
        }

    }
}
