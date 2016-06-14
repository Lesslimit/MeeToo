using System;
using System.Net;
using Microsoft.Extensions.Options;
using MeeToo.Services.MessageSending.Email;
using MeeToo.Services.MessageSending.Email.Options;
using RazorEngine.Templating;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace MeeToo.Services.MessageSending.IoC
{
    public class RegistrationPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            #region Email

            container.Register<IEmailSender, SendGridEmailSender>();
            container.RegisterSingleton<Func<IEmailSender, IEmailDescriptor>>(() => es => new SimpleEmailDescriptor(es));
            container.Register<IEmailTemplateSource, BlobStorageEmailTemplateSource>();
            container.RegisterSingleton<Func<IRazorEmailTemplate>>(() => new RazoreEmailTemplate(container.GetInstance<IRazorEngineService>()));
            container.Register<SendGrid.ISendGrid>(() => new SendGrid.SendGridMessage());
            container.Register<SendGrid.ITransport>(() =>
            {
                var options = container.GetInstance<IOptions<EmailOptions>>();
                var credentials = new NetworkCredential(options.Value.UserName, options.Value.Password);

                return new SendGrid.Web(credentials);
            }, Lifestyle.Singleton);
            container.Register(() => RazorEngine.Engine.Razor);

            #endregion
        }
    }
}
