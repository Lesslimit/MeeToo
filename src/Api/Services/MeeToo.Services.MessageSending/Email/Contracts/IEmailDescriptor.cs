using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeeToo.Services.MessageSending.Email
{
    public interface IEmailDescriptor
    {
        string Subject { get; }
        IEnumerable<string> Recipients { get; }
        IEmailTemplateSource TemplateSource { get; }
        object TemplateModel { get; set; }
        string TemplateName { get; set; }

        IEmailDescriptor AddSubject(string value);
        IEmailDescriptor AddRecipients(string[] value);
        IEmailDescriptor SetTemplate(IEmailTemplateSource value, string name, object model);

        Task SendAsync();
    }
}
