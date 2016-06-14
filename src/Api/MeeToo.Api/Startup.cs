using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MeeToo.Api.Options;
using MeeToo.DataAccess.AzureStorage;
using MeeToo.DataAccess.DocumentDb;
using MeeToo.Security.Identity;
using MeeToo.Security.Options;
using MeeToo.Services.MessageSending.Email.Options;
using SimpleInjector;
using SimpleInjector.Integration.AspNet;

namespace MeeToo.Api
{
    public class Startup
    {
        private readonly Container container = new Container();

        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(container));

            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(container));

            services.AddSingleton(sp => container.GetInstance<CloudBlobClient>());
            services.AddSingleton(sp => container.GetInstance<IStorage>());

            services.AddIdentity<MeeTooUser, string>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.Email;
                options.SignIn.RequireConfirmedEmail = true;
                options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
                options.User.RequireUniqueEmail = true;
            })
                .AddUserStore<UserStore>()
                .AddRoleStore<UserStore>()
                .AddDefaultTokenProviders();

            services.AddOptions();
            services.AddDataProtection();

            services.Configure<DocumentDbOptions>(Configuration.GetSection("DocumentDb"), true);
            services.Configure<AzureStorageOptions>(Configuration.GetSection("AzureStorage"), true);
            services.Configure<WebsiteCookiesOptions>(Configuration.GetSection("WebsiteCookies"), true);
            services.Configure<UserIdentityOptions>(Configuration.GetSection("UserIdentity"), true);
            services.Configure<EmailOptions>(Configuration.GetSection("Email"), true);

            services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    });

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            container.Options.DefaultScopedLifestyle = new AspNetRequestLifestyle();
            app.UseSimpleInjectorAspNetRequestScoping(container);

            InitializeContainer(app);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "api",
                SlidingExpiration = true,
                AutomaticChallenge = false,
                CookieName = "MeeToo.API"
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "emailConfirmation",
                SlidingExpiration = false,
                AutomaticChallenge = false,
                CookieName = "MeeToo.Token"
            });

            app.UseIdentity();
            app.UseMvc();
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Cross-wire ASP.NET services (if any). For instance:
            container.CrossWire<ILoggerFactory>(app);
            container.CrossWire<IDataProtectionProvider>(app);
            container.CrossWire<UserManager<MeeTooUser>>(app);

            container.Register(app.ApplicationServices.GetService<IOptions<DocumentDbOptions>>, Lifestyle.Singleton);
            container.Register(app.ApplicationServices.GetService<IOptions<AzureStorageOptions>>, Lifestyle.Singleton);
            container.Register(app.ApplicationServices.GetService<IOptions<WebsiteCookiesOptions>>, Lifestyle.Singleton);
            container.Register(app.ApplicationServices.GetService<IOptions<UserIdentityOptions>>, Lifestyle.Singleton);
            container.Register(app.ApplicationServices.GetService<IOptions<EmailOptions>>, Lifestyle.Singleton);

            container.RegisterPackages(new[] { typeof(Startup).Assembly });
        }
    }
}
