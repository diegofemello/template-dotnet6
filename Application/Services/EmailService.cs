using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Application.Services.Interfaces;
using Application.Utils.Configurations;
using Application.DTO.Request;
using System.Collections.Generic;
using System.IO;

namespace Application.Services
{
    public class EmailService : IEmailService
    {
        private const string templatePath = "/EmailTemplate/{0}.html";
        private readonly SmtpConfiguration _smtpConfig;

        public async Task SendEmailForEmailConfirmation(UserEmailOptionsDTO userEmailOptions, string path, bool updated = false)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Olá {{UserName}}, Confirme seu email.", userEmailOptions.PlaceHolders);

            string file = updated ? "EmailChanged" : "EmailConfirm";

            userEmailOptions.Body = UpdatePlaceHolders(GetBodyTemplate(file, templatePath, path), userEmailOptions.PlaceHolders);
            await SendEmail(userEmailOptions);
        }

        public async Task SendEmailForForgotPassword(UserEmailOptionsDTO userEmailOptions, string path)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Olá {{UserName}}, Resete sua senha.", userEmailOptions.PlaceHolders);

            userEmailOptions.Body = UpdatePlaceHolders(GetBodyTemplate("ForgotPassword", templatePath, path), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }

        public async Task SendEmailAlert(UserEmailOptionsDTO userEmailOptions, string path)
        {
            userEmailOptions.Subject = $"Alerta: {userEmailOptions.Subject}";

            userEmailOptions.Body = UpdatePlaceHolders(GetBodyTemplate("Alert", templatePath, path), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }

        public EmailService(IOptions<SmtpConfiguration> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        private async Task SendEmail(UserEmailOptionsDTO userEmailOptions)
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

        public static string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
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

        public static string GetBodyTemplate(string templateName, string path, string wwwpath)
        {
            string body = File.ReadAllText(string.Format(wwwpath + path, templateName));
            return body;
        }
    }
}
