// Services/Email/IdentityEmailSender.cs
using Microsoft.AspNetCore.Identity.UI.Services;
using VC_IMS.Models.Email;
using VC_IMS.Services.Email;

public sealed class IdentityEmailSender : IEmailSender
{
    private readonly IEmailService _emails;
    public IdentityEmailSender(IEmailService emails) => _emails = emails;
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
        => _emails.SendAsync(new EmailAddress(email), subject, htmlMessage);
}
