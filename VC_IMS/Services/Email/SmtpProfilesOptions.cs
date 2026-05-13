namespace VC_IMS.Services.Email;

public sealed class SmtpProfilesOptions
{
    public string? ActiveProfile { get; init; } = "Microsoft365"; // default
    public Dictionary<string, SmtpConfiguration>? Profiles { get; init; }
}
