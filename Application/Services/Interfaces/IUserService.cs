using Application.DTO;
using Application.DTO.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> Create(UserCreateDTO model);

        Task<UserDTO> Update(Guid id, UserUpdateDTO model);

        Task<bool> Delete(Guid id);

        Task<UserDTO> ConfirmEmailAsync(Guid id, string token);

        Task<UserDTO> ResetPasswordAsync(ResetPasswordDTO model);

        Task<List<UserDTO>> GetAll();

        Task<UserDTO> GetById(Guid id);

        Task<UserDTO> GetByUserName(string userName);

        Task<UserDTO> GetByEmail(string email);

        Task<UserDTO> CheckExist(UserDTO model, Guid notThis);
    }
}
