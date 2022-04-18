using Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Infrastructure.Helpers;

namespace Infrastructure.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<PageList<User>> GetAll(PageParams pageParams);

        Task<User> GetById(Guid id);
    }
}