namespace VC_IMS.Services.Diagnostics.Sessions;

public interface ISessionLogger
{
    Task OnSignedInAsync(int userId, string username, string sessionId, string? ip, string? userAgent);
    Task OnHeartbeatAsync(int userId, string sessionId);
    Task OnSignedOutAsync(int userId, string sessionId);
}
