using System.Threading;
using System.Threading.Tasks;

namespace VC_IMS.Services.Notifications;

public interface IWebPushSender
{
    Task SendToUserAsync(int userId, object payload, CancellationToken cancellationToken = default);
}
