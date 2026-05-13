namespace VC_IMS.Models.Email;

public readonly record struct EmailAddress(string Address, string? DisplayName = null);
