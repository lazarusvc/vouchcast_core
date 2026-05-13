using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using VC_IMS.Data;
using VC_IMS.Services.Messaging;
using System.Security.Claims;

namespace VC_IMS.Web.Hubs;

[Authorize]
public sealed class ChatsHub : Hub
{
    private readonly VC_IMSIdentityDbContext _db;
    private readonly IChatPresence _presence;
    public ChatsHub(VC_IMSIdentityDbContext db, IChatPresence presence)
    {
        _db = db;
        _presence = presence;
    }

    public async Task Join(Guid conversationId)
    {
        var idStr = Context.User?.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (!int.TryParse(idStr, out var userId))
            throw new HubException("Unable to resolve current user id.");

        var isMember = await _db.ConversationMembers.AsNoTracking()
            .AnyAsync(x => x.ConversationId == conversationId && x.UserId == userId);

        if (!isMember) throw new HubException("Not a member of this conversation.");

        await Groups.AddToGroupAsync(Context.ConnectionId, $"c:{conversationId}");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var idStr = Context.User?.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (int.TryParse(idStr, out var userId))
            await _presence.LeaveAsync(userId, Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

}
