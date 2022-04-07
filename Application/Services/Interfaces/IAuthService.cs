using Domain.Model;
using Application.DTO;
using Application.DTO.Request;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDTO> ValidateCredentials(AuthRequestDTO userCredentials);

        Task<TokenDTO> ValidateCredentials(string accessToken, string refreshToken);

        Task<bool> RevokeToken(string email);

        Task GenerateForgotPasswordTokenAsync(UserDTO user);

        Task GenerateEmailConfirmationTokenAsync(User user);

        Task GenerateEmailConfirmationByUser(UserDTO user);

        string PasswordHash(string input);
    }
}