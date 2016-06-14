namespace MeeToo.Services.MessageSending.Email
{
    public interface IEmailTemplate
    {
        string Render<TModel>(TModel model);
    }
}
