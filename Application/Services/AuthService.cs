using Application.Utils.Configurations;
using Domain.VO;
using Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Services.Interfaces;
using Domain.VO.Request;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Domain.Model;
using Microsoft.AspNetCore.Hosting;
using Infrastructure.Repository.Generic;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private readonly TokenConfiguration _tokenConfiguration;

        private readonly IGenericRepository _genericRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AuthService(TokenConfiguration tokenConfiguration,
                            IGenericRepository genericRepository,
                            IUserRepository repository,
                            ITokenService tokenService,
                            IMapper mapper,
                            IEmailService emailService,
                            IConfiguration configuration,
                            IWebHostEnvironment hostEnvironment)
        {
            _tokenConfiguration = tokenConfiguration;
            _genericRepository = genericRepository;
            _userRepository = repository;
            _tokenService = tokenService;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<TokenVO> ValidateCredentials(AuthVO userCredentials)
        {
            User user = await _userRepository.ValidateCredentials(userCredentials);
            if (user == null) return null;

            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            DateTime createDate = DateTime.Now;
            user.LastAccess = createDate;
            DateTime expirationDate = createDate.AddMinutes(_tokenConfiguration.Minutes);

            string accessToken = _tokenService.GenerateAccessToken(claims);
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_tokenConfiguration.DaysToExpiry);
            User result = await _userRepository.RefreshUserInfo(user);
            UserVO mappedUser = _mapper.Map<UserVO>(result);

            return new TokenVO(
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                user.RefreshToken,
                mappedUser
                );
        }

        public async Task<TokenVO> ValidateCredentials(TokenVO token)
        {
            string accessToken = token.AccessToken;
            string refreshToken = token.RefreshToken;

            ClaimsPrincipal principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            User user = await _genericRepository.FirstOrDefault<User>(
                u => u.UserName == principal.Identity.Name);

            if (user == null ||
                user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now) return null;

            accessToken = _tokenService.GenerateAccessToken(principal.Claims);
            user.RefreshToken = _tokenService.GenerateRefreshToken();

            User result = await _userRepository.RefreshUserInfo(user);
            UserVO mappedUser = _mapper.Map<UserVO>(result);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_tokenConfiguration.Minutes);

            return new TokenVO(
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                user.RefreshToken,
                mappedUser
                );
        }

        public async Task GenerateForgotPasswordTokenAsync(UserVO model)
        {
            User user = await _userRepository.GetById(model.Uid);
            user.EmailToken = Guid.NewGuid().ToString()[..8];
            user.ChangedEmail = null;
            string wwwPath = _hostEnvironment.WebRootPath;
            User result = await _userRepository.RefreshUserInfo(user);

            string section = _configuration.GetSection("Application:ForgotPassword").Value;
            UserEmailOptions options = SendEmailByTemplate(result, user.EmailToken, section);
            await _emailService.SendEmailForForgotPassword(options, wwwPath);
        }

        public async Task GenerateEmailConfirmationByUser(UserVO user)
        {
            User mapped = await _userRepository.GetById(user.Uid);

            mapped.EmailToken = Guid.NewGuid().ToString()[..8];
            User result = await _userRepository.RefreshUserInfo(mapped);
            await GenerateEmailConfirmationTokenAsync(result);
        }

        public async Task GenerateEmailConfirmationTokenAsync(User user)
        {
            if (user == null) throw new Exception("Erro ao tentar gerar token");

            string wwwPath = _hostEnvironment.WebRootPath;

            bool updated = false;
            if (user.ChangedEmail != null)
            {
                user.Email = user.ChangedEmail;
                updated = true;
            }

            if (!string.IsNullOrEmpty(user.EmailToken))
            {
                string section = _configuration.GetSection("Application:EmailConfirmation").Value;
                UserEmailOptions options = SendEmailByTemplate(user, user.EmailToken, section);
                await _emailService.SendEmailForEmailConfirmation(options, wwwPath, updated);
            }
        }

        private UserEmailOptions SendEmailByTemplate(User user, string token, string section)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;

            UserEmailOptions options = new()
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.UserName),
                    new KeyValuePair<string, string>("{{Domain}}", appDomain),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + section, user.Uid, token)),

                }
            };
            return options;
        }

        public async Task<bool> RevokeToken(string userName)
        {
            return await _userRepository.RevokeToken(userName);
        }


    }
}