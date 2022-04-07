using Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Infrastructure.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();

        Task<User> GetById(Guid id);

        
    }
}