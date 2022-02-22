using Domain.VO;
using Domain.VO.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserVO> Create(UserCreateVO model);

        Task<UserVO> Update(Guid id, UserUpdateVO model);

        Task<bool> Delete(Guid id);

        Task<UserVO> ConfirmEmailAsync(Guid id, string token);

        Task<UserVO> ResetPasswordAsync(ResetPasswordVO model);

        Task<List<UserVO>> GetAll();

        Task<UserVO> GetById(Guid id);

        Task<UserVO> GetByUserName(string userName);

        Task<UserVO> GetByEmail(string userName);

        Task<UserVO> CheckExist(UserVO model, Guid notThis);
    }
}
