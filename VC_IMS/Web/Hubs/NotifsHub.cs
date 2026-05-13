using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace VC_IMS.Web.Hubs;

[Authorize]
public sealed class NotifsHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var uidStr = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(uidStr, out var uid))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"u:{uid}");
        }
        await base.OnConnectedAsync();
    }
}
