using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using MeeToo.Domain.Contracts;

namespace MeeToo.DataAccess.DocumentDb
{
    public class DocumentDbStorageCollection<T> : IStorageCollection<T> where T : IDocument
    {
        private readonly DocumentClient documentClient;
        private readonly Database db;
        private readonly DocumentCollection collection;

        private Uri CollectionUri => UriFactory.CreateDocumentCollectionUri(db.Id, collection.Id);

        public DocumentDbStorageCollection(DocumentClient documentClient, Database db, DocumentCollection collection)
        {
            this.documentClient = documentClient;
            this.db = db;
            this.collection = collection;
        }

        public IDocumentDbQquery<T> Query()
        {
            return new DocumentDbQuery<T>(documentClient, db, collection);
        }

        public async Task AddAsync(T document, CancellationToken cancelToken = new CancellationToken())
        {
            ResourceResponse<Document> response = await documentClient.CreateDocumentAsync(CollectionUri, document, new RequestOptions
            {
                
            })
                                                                      .ConfigureAwait(false);
        }

        public async Task UpdateAsync(T document)
        {
            var response = await documentClient.UpsertDocumentAsync(CollectionUri, document)
                                               .ConfigureAwait(false);
        }

        public async Task DeleteAsync(T document)
        {
            var response = await documentClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(db.Id, collection.Id, document.Id))
                                               .ConfigureAwait(false);
        }
    }
}
