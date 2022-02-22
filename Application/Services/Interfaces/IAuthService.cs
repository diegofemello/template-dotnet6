using Domain.Model;
using Domain.VO;
using Domain.VO.Request;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenVO> ValidateCredentials(AuthVO userCredentials);

        Task<TokenVO> ValidateCredentials(TokenVO token);

        Task<bool> RevokeToken(string userName);

        Task GenerateForgotPasswordTokenAsync(UserVO user);

        Task GenerateEmailConfirmationTokenAsync(User user);

        Task GenerateEmailConfirmationByUser(UserVO user);
    }
}