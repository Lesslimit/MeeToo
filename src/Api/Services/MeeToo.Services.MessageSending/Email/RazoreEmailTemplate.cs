using RazorEngine.Templating;

namespace MeeToo.Services.MessageSending.Email
{
    public class RazoreEmailTemplate : IRazorEmailTemplate
    {
        private readonly IRazorEngineService engineService;

        public string Key { get; set; }
        public string Template { get; set; }

        public RazoreEmailTemplate(IRazorEngineService engineService)
        {
            this.engineService = engineService;
        }

        public string Render<TModel>(TModel model)
        {
            return engineService.RunCompile(Template, Key, null, model);
        }
    }
}
