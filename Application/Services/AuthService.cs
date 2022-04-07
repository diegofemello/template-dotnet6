using Application.Utils.Configurations;
using Application.DTO;
using Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Services.Interfaces;
using Application.DTO.Request;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Domain.Model;
using Microsoft.AspNetCore.Hosting;
using Infrastructure.Repository.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
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

        public async Task<TokenDTO> ValidateCredentials(AuthRequestDTO userCredentials)
        {
            var pass = PasswordHash(userCredentials.Password);

            User user = await _genericRepository.FirstOrDefault<User>(
                u => (u.Email == userCredentials.Login || u.UserName == userCredentials.Login) &&
                (u.Password == pass)) ;

            if (user == null) return null;

            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            DateTime createDate = DateTime.Now;
            user.LastAccess = createDate;
            DateTime expirationDate = createDate.AddMinutes(_tokenConfiguration.Minutes);

            string accessToken = _tokenService.GenerateAccessToken(claims);
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = createDate.AddDays(_tokenConfiguration.DaysToExpiry);

            if (await _genericRepository.Update(user))
            {
                User result = await _userRepository.GetById(user.Uid);

                UserDTO mappedUser = _mapper.Map<UserDTO>(result);

                TokenDTO tokenResult = new()
                {
                    Authenticated = true,
                    Created = createDate,
                    Expiration = expirationDate,
                    AccessToken = accessToken,
                    User = mappedUser
                };

                return tokenResult;
            }
            else
            {
                throw new Exception("Ocorreu um erro ao realizar login");
            }
        }

        public async Task<TokenDTO> ValidateCredentials(string accessToken, string refreshToken)
        {

            ClaimsPrincipal principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            User user = await _genericRepository.FirstOrDefault<User>(
                u => u.UserName == principal.Identity.Name);

            if (user == null ||
                user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now) return null;

            DateTime createDate = DateTime.Now;

            accessToken = _tokenService.GenerateAccessToken(principal.Claims);

            user.LastAccess = createDate;
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = createDate.AddDays(_tokenConfiguration.DaysToExpiry);

            if (await _genericRepository.Update(user))
            {
                User result = await _userRepository.GetById(user.Uid);

                UserDTO mappedUser = _mapper.Map<UserDTO>(result);

                DateTime expirationDate = createDate.AddMinutes(_tokenConfiguration.Minutes);

                TokenDTO tokenResult = new()
                {
                    Authenticated = true,
                    Created = createDate,
                    Expiration = expirationDate,
                    AccessToken = accessToken,
                    User = mappedUser
                };

                return tokenResult;
            }
            else
            {
                throw new Exception("Ocorreu um erro ao realizar login");
            }
        }

        public async Task GenerateForgotPasswordTokenAsync(UserDTO model)
        {
            User user = await _userRepository.GetById(model.Uid);
            user.EmailToken = Guid.NewGuid().ToString()[..8];

            string wwwPath = _hostEnvironment.WebRootPath;

            if (await _genericRepository.Update(user))
            {
                string section = _configuration.GetSection("Application:ForgotPassword").Value;
                UserEmailOptionsDTO options = SendEmailByTemplate(user, user.EmailToken, section);
                await _emailService.SendEmailForForgotPassword(options, wwwPath);
            }
            else
            {
                throw new Exception("Ocorreu um erro ao gerar token de reset de senha.");
            }
        }


        public async Task GenerateEmailConfirmationByUser(UserDTO user)
        {
            User mapped = await _userRepository.GetById(user.Uid);

            mapped.EmailToken = Guid.NewGuid().ToString()[..8];
            if (await _genericRepository.Update(user))
            {
                await GenerateEmailConfirmationTokenAsync(mapped);
            }
            else
            {
                throw new Exception("Ocorreu um erro ao gerar token de confirmação de email.");
            }
            
        }

        public async Task GenerateEmailConfirmationTokenAsync(User user)
        {
            if (user == null) throw new Exception("Erro ao tentar gerar token");

            string wwwPath = _hostEnvironment.WebRootPath;

            if (!string.IsNullOrEmpty(user.EmailToken))
            {
                string section = _configuration.GetSection("Application:EmailConfirmation").Value;
                UserEmailOptionsDTO options = SendEmailByTemplate(user, user.EmailToken, section);
                await _emailService.SendEmailForEmailConfirmation(options, wwwPath);
            }
        }

        private UserEmailOptionsDTO SendEmailByTemplate(User user, string token, string section)
        {
            string webDomain = _configuration.GetSection("Application:WebDomain").Value;
            string apiDomain = _configuration.GetSection("Application:ApiDomain").Value;
            string supportEmail = _configuration.GetSection("Application:SupportEmail").Value;

            string[] nameSplit = user.FullName.Split(' ');
            string name = nameSplit.FirstOrDefault();

            if (nameSplit.Length > 1) name = nameSplit[0] + " " + nameSplit[1]; 

            UserEmailOptionsDTO options = new()
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", name),
                    new KeyValuePair<string, string>("{{WebDomain}}", webDomain),
                    new KeyValuePair<string, string>("{{ApiDomain}}", apiDomain),
                    new KeyValuePair<string, string>("{{SupportEmail}}", supportEmail),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(webDomain + section, user.Uid, token)),
                }
            };

            return options;
        }

        public async Task<bool> RevokeToken(string email)
        {
            User user = await _genericRepository
                .FirstOrDefault<User>(u => u.Email == email);

            if (user is null) return false;

            user.RefreshToken = null;
            return await _genericRepository.Update(user);
        }

        public string PasswordHash(string input) => Hash(input);

        private static string Hash(string input)
        {
            SHA256 algorithm = SHA256.Create();

            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes);
        }


    }
}