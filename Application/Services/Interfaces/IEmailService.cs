using Domain.VO;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions, string path, bool updated = false);

        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions, string path);
    }
}