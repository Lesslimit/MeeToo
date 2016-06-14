namespace MeeToo.Services.MessageSending.Email.Options
{
    public class EmailOptions
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public TemplateSource TemplateSource { get; set; }
    }

    public class TemplateSource
    {
        public BlobSource Blob { get; set; }
    }

    public class BlobSource
    {
        public string Container { get; set; }
    }
}
