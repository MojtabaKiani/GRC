using FluentEmail.Core;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace GRC.IdentityProvider.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IFluentEmail _email;

        public EmailSender(IFluentEmail email)
        {
            _email = email;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await _email.To(email)
                        .Subject(subject)
                        .Body(htmlMessage)
                        .SendAsync();
        }
    }
}
