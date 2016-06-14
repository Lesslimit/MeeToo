using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;

namespace MeeToo.Services.MessageSending.Email
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly Func<IEmailSender, IEmailDescriptor> descriptorFactory;
        private readonly ISendGrid sendGrid;
        private readonly ITransport transport;

        public SendGridEmailSender(Func<IEmailSender, IEmailDescriptor> descriptorFactory,
                                   ISendGrid sendGrid,
                                   ITransport transport)
        {
            this.descriptorFactory = descriptorFactory;
            this.sendGrid = sendGrid;
            this.transport = transport;
        }

        public IEmailDescriptor To(params string[] recipients)
        {
            var emailDescriptor = descriptorFactory(this);

            return emailDescriptor.AddRecipients(recipients);
        }

        public async Task SendAsync(IEmailDescriptor descriptor)
        {
            var template = await descriptor.TemplateSource.RetriveAsync(descriptor.TemplateName);

            sendGrid.AddTo(descriptor.Recipients);
            sendGrid.From = new MailAddress("roman.viytovych@gmail.com", "MeeToo");
            sendGrid.Subject = descriptor.Subject;
            sendGrid.Html = template.Render(descriptor.TemplateModel);

            await transport.DeliverAsync(sendGrid).ConfigureAwait(false);
        }
    }
}
