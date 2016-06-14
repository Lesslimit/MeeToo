using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using MeeToo.Services.MessageSending.Email.Options;

namespace MeeToo.Services.MessageSending.Email
{
    public class BlobStorageEmailTemplateSource : IEmailTemplateSource
    {
        private readonly CloudBlobClient blobClient;
        private readonly Func<IRazorEmailTemplate> templateFactory;
        private readonly IOptions<EmailOptions> options;

        public BlobStorageEmailTemplateSource(CloudBlobClient blobClient,
                                              Func<IRazorEmailTemplate> templateFactory,
                                              IOptions<EmailOptions> options)
        {
            this.blobClient = blobClient;
            this.templateFactory = templateFactory;
            this.options = options;
        }

        public async Task<IEmailTemplate> RetriveAsync(string key, CancellationToken cancelToken = default(CancellationToken))
        {
            var container = blobClient.GetContainerReference(options.Value.TemplateSource.Blob.Container);
            var blob = container.GetBlockBlobReference(key);

            var html = await blob.DownloadTextAsync(cancelToken).ConfigureAwait(false);
            var template = templateFactory();

            template.Key = key;
            template.Template = html;

            return template;
        }
    }
}
