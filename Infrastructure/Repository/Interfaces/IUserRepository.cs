using Domain.VO.Request;
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

        Task<bool> Delete(User user);

        Task<User> ValidateCredentials(AuthVO user);

        Task<bool> RevokeToken(string userName);

        Task<User> RefreshUserInfo(User user);

        string PasswordHash(string input);
    }
}