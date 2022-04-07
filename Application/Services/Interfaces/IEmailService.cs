using Application.DTO.Request;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailForEmailConfirmation(UserEmailOptionsDTO userEmailOptions, string path, bool updated = false);

        Task SendEmailForForgotPassword(UserEmailOptionsDTO userEmailOptions, string path);
    }
}