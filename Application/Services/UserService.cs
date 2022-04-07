using AutoMapper;
using Application.DTO;
using Domain.Model;
using Infrastructure.Repository.Interfaces;
using Infrastructure.Repository.Generic;
using System;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Application.DTO.Request;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public UserService(
            IGenericRepository genericRepository,
            IUserRepository userRepository,
            IAuthService authService,
            IMapper mapper)
        {
            _genericRepository = genericRepository;
            _userRepository = userRepository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<UserDTO> Create(UserCreateDTO body)
        {
            User user = _mapper.Map<User>(body);

            user.EmailToken = Guid.NewGuid().ToString()[..8];
            user.Password = _authService.PasswordHash(user.Password);
            if (await _genericRepository.Add(user))
            {
                User result = await _genericRepository
                    .FirstOrDefault<User>(u => u.Uid == user.Uid);

                await _authService.GenerateEmailConfirmationTokenAsync(result);

                return _mapper.Map<UserDTO>(result);
            }
            return null;
        }

        public async Task<UserDTO> Update(Guid id, UserUpdateDTO body)
        {
            User user = await _genericRepository
                .FirstOrDefault<User>(u => u.Uid == id);

            if (user == null) return null;

            if (body.Password == null)
            {
                body.Password = user.Password;
            }
            else
            {
                body.Password = _authService.PasswordHash(body.Password);
            }

            if (body.UserName != user.UserName)
            {
                User checkExist = await _genericRepository
                    .FirstOrDefault<User>(u => u.UserName == body.UserName);

                if (checkExist != null && checkExist.Uid != user.Uid)
                    throw new Exception("Usuário informado já está cadastrado");
            }

            if (body.Email != user.Email)
            {
                User checkExist = await _genericRepository
                    .FirstOrDefault<User>(u => u.Email == body.Email);

                if (checkExist != null && checkExist.Uid != user.Uid)
                    throw new Exception("Email informado já está cadastrado");

                user.ChangedEmail = body.Email;
                body.Email = user.Email;
                user.EmailToken = Guid.NewGuid().ToString()[..8];
                await _authService.GenerateEmailConfirmationTokenAsync(user);
            }
            else
            {
                user.ChangedEmail = null;
                user.EmailToken = null;
            }

            if (body.UserRole == null) body.UserRole = user.UserRole;

            _mapper.Map(body, user);

            if (await _genericRepository.Update(user))
            {
                User result = await _userRepository
                    .GetById(user.Uid);

                return _mapper.Map<UserDTO>(result);
            }
            return null;
        }


        public async Task<bool> Delete(Guid id)
        {
            User user = await _genericRepository
                .FirstOrDefault<User>(u => u.Uid == id);

            if (user == null)
                throw new Exception("Não existe usuário com o ID informado");

            return await _genericRepository.Delete(user);
        }


        public async Task<UserDTO> ConfirmEmailAsync(Guid uid, string token)
        {
            User user = await _userRepository.GetById(uid);
            if (user.EmailToken == token)
            {
                if (user.ChangedEmail != null)
                {
                    user.Email = user.ChangedEmail;
                    user.ChangedEmail = null;
                }

                user.EmailConfirmed = true;
                user.EmailToken = null;
                if (await _genericRepository.Update(user))
                {
                    return _mapper.Map<UserDTO>(user);
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao resetar senha");
                }
            }
            else
            {
                throw new Exception("Token Inválido ou expirado");
            }
        }

        public async Task<UserDTO> ResetPasswordAsync(ResetPasswordDTO body)
        {
            User user = await _userRepository.GetById(body.UserUid);
            if (user == null) throw new Exception("Usuário não encontrado");

            if (user.EmailToken != body.Token)
                throw new Exception("Token Inválido");

            string pass = _authService.PasswordHash(body.NewPassword);

            user.Password = pass;
            user.EmailToken = null;
            user.EmailConfirmed = true;

            if (await _genericRepository.Update(user))
            {
                return _mapper.Map<UserDTO>(user);
            }
            else
            {
                throw new Exception("Ocorreu um erro ao resetar senha");
            }
        }


        public async Task<List<UserDTO>> GetAll()
        {
            List<User> users = await _userRepository.GetAll();

            if (users == null) return null;

            List<UserDTO> result = _mapper.Map<List<UserDTO>>(users);

            return result;
        }


        public async Task<UserDTO> GetById(Guid id)
        {
            User user = await _userRepository.GetById(id);
            if (user == null) return null;

            UserDTO result = _mapper.Map<UserDTO>(user);

            return result;
        }


        public async Task<UserDTO> GetByUserName(string userName)
        {
            User user = await _genericRepository
                .FirstOrDefault<User>(x => x.UserName == userName);

            if (user == null) return null;

            UserDTO resultado = _mapper.Map<UserDTO>(user);

            return resultado;
        }

        public async Task<UserDTO> GetByEmail(string email)
        {
            User user = await _genericRepository
                .FirstOrDefault<User>(x => x.Email == email);

            if (user == null) return null;

            UserDTO result = _mapper.Map<UserDTO>(user);

            return result;
        }

        public async Task<UserDTO> CheckExist(UserDTO body, Guid notThis)
        {
            User check = await _genericRepository.FirstOrDefault<User>(
                u => (u.UserName == body.UserName ||
                u.Email == body.Email ||
                u.ChangedEmail == body.Email) &&
                u.Uid != notThis);

            if (check != null)
                return _mapper.Map<UserDTO>(check);

            return null;
        }
    }
}
