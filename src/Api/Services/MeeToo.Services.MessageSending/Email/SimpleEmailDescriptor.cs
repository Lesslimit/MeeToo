using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeeToo.Services.MessageSending.Email
{
    public class SimpleEmailDescriptor : IEmailDescriptor
    {
        private readonly IEmailSender sender;

        public string Subject { get; private set; }
        public IEnumerable<string> Recipients { get; private set; } = Enumerable.Empty<string>();
        public IEmailTemplateSource TemplateSource { get; private set; }
        public object TemplateModel { get; set; }
        public string TemplateName { get; set; }

        public SimpleEmailDescriptor(IEmailSender sender)
        {
            this.sender = sender;
        }

        public IEmailDescriptor AddSubject(string value)
        {
            this.Subject = value;

            return this;
        }

        public IEmailDescriptor AddRecipients(string[] value)
        {
            this.Recipients = Recipients.Concat(value);

            return this;
        }

        public IEmailDescriptor SetTemplate(IEmailTemplateSource value, string name, object model)
        {
            this.TemplateName = name;

            this.TemplateModel = model;
            this.TemplateSource = value;

            return this;
        }

        public async Task SendAsync()
        {
            await sender.SendAsync(this).ConfigureAwait(false);
        }
    }
}
