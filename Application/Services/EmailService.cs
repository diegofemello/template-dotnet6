using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Net.Mail;
using Domain.VO;
using Application.Services.Interfaces;
using Application.Utils.Configurations;

namespace Application.Services
{
    public class EmailService : IEmailService
    {
        private const string templatePath = "/EmailTemplate/{0}.html";
        private readonly SmtpConfiguration _smtpConfig;

        public async Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions, string path, bool updated)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Olá {{UserName}}, Confirme seu email.", userEmailOptions.PlaceHolders);

            string file = updated ? "EmailChanged" : "EmailConfirm";

            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody(file, path), userEmailOptions.PlaceHolders);
            await SendEmail(userEmailOptions);
        }

        public async Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions, string path)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Olá {{UserName}}, Resete sua senha.", userEmailOptions.PlaceHolders);

            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword", path), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }

        public EmailService(IOptions<SmtpConfiguration> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new()
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML
            };


            foreach (string toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }

            NetworkCredential networkCredential = new(_smtpConfig.UserName, _smtpConfig.Password);

            SmtpClient smtpClient = new()
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential
            };

            mail.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mail);
        }

        private static string GetEmailBody(string templateName, string path)
        {
            string body = File.ReadAllText(string.Format(path + templatePath, templateName));
            return body;
        }

        private static string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (KeyValuePair<string, string> placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }
    }
}
