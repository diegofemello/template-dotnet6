using AutoMapper;
using Domain.VO;
using Domain.Model;
using Infrastructure.Repository.Interfaces;
using Infrastructure.Repository.Generic;
using System;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.VO.Request;
using System.Collections.Generic;

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

        public async Task<UserVO> Create(UserCreateVO body)
        {
            User user = _mapper.Map<User>(body);

            user.Role = "Cliente";
            user.EmailToken = Guid.NewGuid().ToString()[..8];

            user.Password = _userRepository.PasswordHash(user.Password);

            if (await _genericRepository.Add(user))
            {
                User result = await _genericRepository
                    .FirstOrDefault<User>(u => u.Uid == user.Uid);
                    
                await _authService.GenerateEmailConfirmationTokenAsync(result);

                return _mapper.Map<UserVO>(result);
            }
            return null;
        }

        public async Task<UserVO> Update(Guid id, UserUpdateVO body)
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
                body.Password = _userRepository.PasswordHash(body.Password);
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

            if(body.Role == null)
            {
                body.Role = user.Role;
                user.UserRole.Name = user.Role;
            }

            _mapper.Map(body, user);


            if (await _genericRepository.Update(user))
            {
                User userRetorno = await _userRepository
                    .GetById(user.Uid);

                return _mapper.Map<UserVO>(userRetorno);
            }
            return null;
        }


        public async Task<bool> Delete(Guid id)
        {
            User user = await _genericRepository
                .FirstOrDefault<User>(u => u.Uid == id);

            if (user == null)
                throw new Exception("Não existe usuário com o ID informado");

            return await _userRepository.Delete(user);
        }


        public async Task<UserVO> ConfirmEmailAsync(Guid uid, string token)
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
                User result = await _userRepository.RefreshUserInfo(user);
                return _mapper.Map<UserVO>(result);
            }
            else
            {
                throw new Exception("Token Inválido ou expirado");
            }
        }


        public async Task<UserVO> ResetPasswordAsync(ResetPasswordVO body)
        {
            User user = await _userRepository.GetById(body.UserUid);
            if (user == null) throw new Exception("Usuário não encontrado");

            if (user.EmailToken == body.Token)
            {
                string pass = _userRepository.PasswordHash(body.NewPassword);

                user.Password = pass;
                user.EmailToken = null;
                User result = await _userRepository.RefreshUserInfo(user);
                return _mapper.Map<UserVO>(result);
            }
            else
            {
                throw new Exception("Token Inválido");
            }
        }


        public async Task<List<UserVO>> GetAll()
        {
            List<User> users = await _userRepository.GetAll();

            if (users == null) return null;

            List<UserVO> resultado = _mapper.Map<List<UserVO>>(users);

            return resultado;
        }


        public async Task<UserVO> GetById(Guid id)
        {
            User user = await _userRepository.GetById(id);
            if (user == null) return null;

            UserVO resultado = _mapper.Map<UserVO>(user);

            return resultado;
        }


        public async Task<UserVO> GetByUserName(string userName)
        {
            User user = await _genericRepository
                .FirstOrDefault<User>(x => x.UserName == userName);

            if (user == null) return null;

            UserVO resultado = _mapper.Map<UserVO>(user);

            return resultado;
        }

        public async Task<UserVO> GetByEmail(string email)
        {
            User user = await _genericRepository
                .FirstOrDefault<User>(x => x.Email == email || x.ChangedEmail == email);

            if (user == null) return null;

            UserVO resultado = _mapper.Map<UserVO>(user);

            return resultado;
        }


        public async Task<UserVO> CheckExist(UserVO body, Guid notThis)
        {
            User check = await _genericRepository.FirstOrDefault<User>(
                u => (u.UserName == body.UserName ||
                u.Email == body.Email ||
                u.ChangedEmail == body.Email) &&
                u.Uid != notThis);

            if (check != null)
                return _mapper.Map<UserVO>(check);

            return null;
        }
    }
}
