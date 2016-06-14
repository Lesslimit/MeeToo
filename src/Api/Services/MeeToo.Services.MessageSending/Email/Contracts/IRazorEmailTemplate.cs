namespace MeeToo.Services.MessageSending.Email
{
    public interface IRazorEmailTemplate : IEmailTemplate
    {
        string Key { get; set; }
        string Template { get; set; }
    }
}
