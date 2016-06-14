using System;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using MeeToo.DataAccess.DocumentDb;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace MeeToo.DataAccess.IoC
{
    public class RegistrationPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<IStorage, DocumentDbStorage>(Lifestyle.Singleton);

            container.RegisterSingleton(() =>
            {
                var options = container.GetInstance<IOptions<DocumentDbOptions>>();
                var serviceEndpoint = new Uri(options.Value.ServiceEndpoint);

                return new DocumentClient(serviceEndpoint, options.Value.AuthKey, ConnectionPolicy.Default);
            });
        }
    }
}
