using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using VC_IMS.Data;
using VC_IMS.Models.Notifications;
using VC_IMS.Services.Notifications;

namespace VC_IMS.Web.Endpoints;

public static class PushEndpoints
{
    public static IEndpointRouteBuilder MapVC_IMSPushEndpoints(this IEndpointRouteBuilder app)
    {
        var grp = app.MapGroup("me/push").RequireAuthorization();

        static int Me(ClaimsPrincipal u) => int.Parse(u.FindFirstValue(ClaimTypes.NameIdentifier)!);

        grp.MapGet("vapid", (IConfiguration cfg) =>
        {
            var pub = cfg["WebPush:PublicKey"] ?? "";
            return Results.Ok(new { publicKey = pub });
        });

        grp.MapPost("subscribe", async (HttpContext http, VC_IMSIdentityDbContext db, PushSubscribeDto body) =>
        {
            var me = Me(http.User);
            if (string.IsNullOrWhiteSpace(body.endpoint) ||
                string.IsNullOrWhiteSpace(body.p256dh) ||
                string.IsNullOrWhiteSpace(body.auth))
                return Results.BadRequest(new { error = "invalid subscription" });

            var existing = await db.PushSubscriptions.FirstOrDefaultAsync(x => x.Endpoint == body.endpoint);
            if (existing is null)
            {
                db.PushSubscriptions.Add(new UserPushSubscription
                {
                    Id = Guid.NewGuid(),
                    UserId = me,
                    Endpoint = body.endpoint,
                    P256dh = body.p256dh,
                    Auth = body.auth,
                    UserAgent = http.Request.Headers.UserAgent.ToString(),
                    CreatedUtc = DateTime.UtcNow,
                    LastSeenUtc = DateTime.UtcNow,
                    IsActive = true
                });
            }
            else
            {
                existing.UserId = me;
                existing.P256dh = body.p256dh;
                existing.Auth = body.auth;
                existing.UserAgent = http.Request.Headers.UserAgent.ToString();
                existing.IsActive = true;
                existing.LastSeenUtc = DateTime.UtcNow;
            }

            await db.SaveChangesAsync();
            return Results.Ok(new { ok = true });
        });

        grp.MapPost("unsubscribe", async (VC_IMSIdentityDbContext db, PushUnsubDto body) =>
        {
            if (string.IsNullOrWhiteSpace(body.endpoint)) return Results.BadRequest();
            var s = await db.PushSubscriptions.FirstOrDefaultAsync(x => x.Endpoint == body.endpoint);
            if (s != null) { s.IsActive = false; await db.SaveChangesAsync(); }
            return Results.Ok(new { ok = true });
        });

        grp.MapPost("test", async (HttpContext http, IWebPushSender push) =>
        {
            var me = Me(http.User);
            await push.SendToUserAsync(me, new
            {
                title = "VC_IMS",
                body = "Push is working 🎉",
                url = "/"
            });
            return Results.Ok(new { ok = true });
        });

        return app;
    }

    public record PushSubscribeDto(string endpoint, string p256dh, string auth);
    public record PushUnsubDto(string endpoint);
}
