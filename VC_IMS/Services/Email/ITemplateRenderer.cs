using System.Threading.Tasks;

namespace VC_IMS.Services.Email;

public interface ITemplateRenderer
{
    Task<EmailTemplate> RenderAsync(string templateKey, object model);
}
