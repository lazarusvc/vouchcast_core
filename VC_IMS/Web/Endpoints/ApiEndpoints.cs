using Microsoft.AspNetCore.Routing;
using VC_IMS.Web.Endpoints;          // Core/Messaging/Notifications/Operations/Push extensions

namespace VC_IMS.Web.Endpoints;

public static class ApiEndpoints
{
    /// <summary>
    /// Single entry point to register ALL API endpoints under /api/v1.
    /// </summary>
    public static IEndpointRouteBuilder MapVC_IMSApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");
        var v1 = api.MapGroup("/v1");

        // Meta (feeds the dashboard)
        v1.MapMetaEndpoints();

        // Core app APIs
        v1.MapVC_IMSCoreEndpoints();
        v1.MapVC_IMSMessagingEndpoints();
        v1.MapVC_IMSNotificationsEndpoints();
        v1.MapVC_IMSOperationsEndpoints();
        v1.MapVC_IMSPushEndpoints();

        return app;
    }
}
