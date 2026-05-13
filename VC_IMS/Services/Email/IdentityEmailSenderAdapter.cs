using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VC_IMS.Models;
using VC_IMS.Models.Email;

namespace VC_IMS.Services.Email;

/// <summary>
/// Adapter so ASP.NET Identity can send through VC_IMS email service.
/// Targets your existing user type: VC_IMS.Models.VC_user
/// </summary>
public sealed class IdentityEmailSenderAdapter : IEmailSender<VC_user>
{
    private readonly IEmailService _emails;

    public IdentityEmailSenderAdapter(IEmailService emails) => _emails = emails;

    public Task SendConfirmationLinkAsync(VC_user user, string email, string confirmationLink)
        => _emails.SendTemplateAsync(TemplateKeys.ConfirmEmail, new EmailAddress(email), new { ConfirmationLink = confirmationLink, FirstName = user?.FirstName });

    public Task SendPasswordResetLinkAsync(VC_user user, string email, string resetLink)
        => _emails.SendTemplateAsync(TemplateKeys.ResetPassword, new EmailAddress(email), new { ResetLink = resetLink, FirstName = user?.FirstName });

    public Task SendPasswordResetCodeAsync(VC_user user, string email, string resetCode)
        => _emails.SendTemplateAsync(TemplateKeys.TwoFactorCode, new EmailAddress(email), new { Code = resetCode, FirstName = user?.FirstName });
}
