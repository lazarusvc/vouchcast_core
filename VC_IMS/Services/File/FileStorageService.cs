using Microsoft.Graph.Models;
using Microsoft.Win32.SafeHandles;
using SimpleImpersonation;
using System.Security.Principal;
using VC_IMS.Services.Email;

public interface IFileStorageService
{
    Task<FileSaveResult> SaveFileAsync(byte[] content, string fileName);
}

public class FileSaveResult
{
    public bool IsSuccess { get; private init; }
    public string? Error { get; private init; }

    public static FileSaveResult Success() => new() { IsSuccess = true };
    public static FileSaveResult Failure(string error) => new() { IsSuccess = false, Error = error };
}

public class FileStorageService : IFileStorageService
{
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emails;

    public FileStorageService(IConfiguration configuration, IEmailService emails)
    {
        _configuration = configuration;
        _emails = emails;
    }

    public async Task<FileSaveResult> SaveFileAsync(byte[] content, string fileName)
    {
        if (content is null || content.Length == 0 || string.IsNullOrWhiteSpace(fileName))
            return FileSaveResult.Failure("Invalid details provided.");

        string? fileStore = _configuration.GetValue<string>("FileStorage:Location");
        string? fileDomain = _configuration.GetValue<string>("FileStorage:Domain");
        string? fileUsername = _configuration.GetValue<string>("FileStorage:Username");
        string? filePassword = _configuration.GetValue<string>("FileStorage:Password");

        if (string.IsNullOrWhiteSpace(fileStore))
            return FileSaveResult.Failure("File storage location is not configured.");

        var credentials = new UserCredentials(fileDomain, fileUsername, filePassword);

        #pragma warning disable CA1416 // Validate platform compatibility
        using SafeAccessTokenHandle userHandle = credentials.LogonUser(SimpleImpersonation.LogonType.NewCredentials);

        await WindowsIdentity.RunImpersonatedAsync(userHandle, async () =>
        {
            string filePath = Path.Combine(fileStore, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await stream.WriteAsync(content, 0, content.Length);
        });
        #pragma warning restore CA1416 // Validate platform compatibility

        await _emails.SendTemplateAsync(
            TemplateKeys.ResetPassword,
            new VC_IMS.Models.Email.EmailAddress("lazarusa@dominica.gov.dm"),
            new
            {
                SubjectLine = "Reset your VC_IMS password",
                BodyIntro = "A request was received to reset the password for your VC_IMS account. If this was you, use the button below to continue.",
                MainParagraph = "For your protection, the reset link will expire after a short time and can be used only once. If you did not request a password reset, you may safely ignore this message and your password will remain unchanged.",

                ShowCTA = true,
                ActionLabel = "Reset Password",
                // ActionUrl = callbackUrl,              // formerly ResetLink

                SupportEmail = "support.apps@gov.dm",
                SupportPhone = "(767) 266-3310",
                // ReferenceId = referenceId
            });

        return FileSaveResult.Success();
    }
}