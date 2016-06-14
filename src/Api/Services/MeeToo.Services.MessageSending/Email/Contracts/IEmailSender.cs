using System.Threading.Tasks;

namespace MeeToo.Services.MessageSending.Email
{
    public interface IEmailSender
    {
        IEmailDescriptor To(params string[] recipients);
        Task SendAsync(IEmailDescriptor emailDescriptor);
    }
}
