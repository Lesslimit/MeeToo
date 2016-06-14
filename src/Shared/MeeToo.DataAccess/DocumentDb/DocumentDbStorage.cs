using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using MeeToo.Domain.Attributes;
using MeeToo.Domain.Contracts;

namespace MeeToo.DataAccess.DocumentDb
{
    public sealed class DocumentDbStorage : IStorage, IStorageDb
    {
        #region Fields

        private readonly IOptions<DocumentDbOptions> dbOptions;
        private readonly Task<Database> databaseFuture;

        private readonly DocumentClient documentClient;

        #endregion Fields

        public DocumentDbStorage(DocumentClient documentClient,
                                 IOptions<DocumentDbOptions> dbOptions)
        {
            this.dbOptions = dbOptions;
            this.documentClient = documentClient;
        }

        private DocumentDbStorage(DocumentClient documentClient,
                                  IOptions<DocumentDbOptions> dbOptions,
                                  Task<Database> databaseFuture)
        {
            this.dbOptions = dbOptions;
            this.databaseFuture = databaseFuture;
            this.documentClient = documentClient;
        }

        public IStorageDb Db(string id)
        {
            id = id ?? dbOptions.Value.DefaultDbId;

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return new DocumentDbStorage(documentClient, dbOptions, GetOrCreateDatabaseAsync(id));
        }

        public async Task CreateIfNotExists(string id)
        {
            try
            {
                await documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(id))
                                    .ConfigureAwait(false);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await documentClient.CreateDatabaseAsync(new Database { Id = id })
                                        .ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IStorageCollection<T>> CollectionAsync<T>(string id = null) where T : IDocument
        {
            var db = await databaseFuture.ConfigureAwait(false);
            var collectionId = id ?? GetCollectionId(typeof(T));

            var response = await documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(db.Id, collectionId));

            return new DocumentDbStorageCollection<T>(documentClient, db, response.Resource);
        }

        #region Private Stuff

        private async Task<Database> GetOrCreateDatabaseAsync(string id)
        {
            return documentClient.CreateDatabaseQuery()
                                 .Where(db => db.Id == id)
                                 .ToArray()
                                 .FirstOrDefault() ??
                   await documentClient.CreateDatabaseAsync(new Database {Id = id})
                                       .ConfigureAwait(false);
        }

        private static string GetCollectionId(MemberInfo resourceType)
        {
            var dbCollectionAttribute = resourceType.GetCustomAttribute<DbCollectionAttribute>();

            return dbCollectionAttribute != null
                ? dbCollectionAttribute.Id
                : resourceType.Name.Camelize().Pluralize();
        }

        #endregion Private Stuff

        #region IDisposable

        public void Dispose()
        {
            try
            {
                documentClient.Dispose();
            }
            catch
            {
                //ignore
            }
        }

        #endregion
    }
}
