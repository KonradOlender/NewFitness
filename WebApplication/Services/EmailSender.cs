using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;
using MailKit.Net.Smtp;
using System.IO;
using MailKit.Security;

namespace WebApplication.Areas
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly IHostingEnvironment _env;

        public EmailSender(
            IOptions<EmailSettings> emailSettings,
            IHostingEnvironment env)
        {
            _emailSettings = emailSettings.Value;
            _env = env;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
                mimeMessage.To.Add(MailboxAddress.Parse(email));
                mimeMessage.Subject = subject;

                var builder = new BodyBuilder { HtmlBody = htmlMessage };
                mimeMessage.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    if (_env.IsDevelopment())
                    {
                        await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, SecureSocketOptions.StartTls);
                        //   await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, true);
                    }
                    else
                    {
                        await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, SecureSocketOptions.StartTls);
                        // await client.ConnectAsync(_emailSettings.MailServer);
                    }
                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception exp)
            {
                throw new InvalidOperationException(exp.Message);
            }
        }
    }
}