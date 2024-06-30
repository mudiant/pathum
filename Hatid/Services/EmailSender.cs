using FluentEmail.Core.Models;
using FluentEmail.Core;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MimeKit.Text;
using Hatid.Data;

namespace Hatid.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IFluentEmail _fluentEmail;
        public EmailSender(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail ?? throw new ArgumentNullException(nameof(fluentEmail));
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await _fluentEmail.To(to)
                    .Subject(subject)
                    .Body(body, true)
                    .SendAsync();
        }
    }
}
