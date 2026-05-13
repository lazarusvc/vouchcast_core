using VC_IMS.Models.Outbox;

namespace VC_IMS.Services.Outbox;

public interface IEmailOutbox
{
    Task<Guid> EnqueueAsync(string to, string subject, string? html, string? text = null, string? cc = null, string? bcc = null, string? headersJson = null);
    Task<int> DispatchBatchAsync(int take, CancellationToken ct = default);
}
