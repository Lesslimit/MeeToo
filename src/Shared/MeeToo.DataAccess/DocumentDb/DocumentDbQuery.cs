using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace MeeToo.DataAccess.DocumentDb
{
    public class DocumentDbQuery<T> : IDocumentDbQquery<T>
    {
        private readonly DocumentClient documentClient;
        private readonly Database db;
        private readonly DocumentCollection collection;
        private readonly IQueryable<T> queryable;

        public Expression Expression => queryable.Expression;
        public Type ElementType => queryable.ElementType;
        public IQueryProvider Provider => queryable.Provider;

        public DocumentDbQuery(DocumentClient documentClient, Database db, DocumentCollection collection)
            : this(documentClient, db, collection, documentClient.CreateDocumentQuery<T>(collection.DocumentsLink))
        {
        }

        public DocumentDbQuery(DocumentClient documentClient, Database db, DocumentCollection collection, IQueryable<T> queryable)
        {
            this.documentClient = documentClient;
            this.db = db;
            this.collection = collection;
            this.queryable = queryable;
        }

        public IDocumentDbQquery<T> Where(Expression<Func<T, bool>> predicate)
        {
            return new DocumentDbQuery<T>(documentClient, db, collection, queryable.Where(predicate));
        }

        public async Task<T> FindAsync(string id)
        {
            try
            {
                var response = await documentClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(db.Id, collection.Id, id))
                                                   .ConfigureAwait(false);

                T document = (T)(dynamic)response.Resource;

                return document;
            }
            catch (DocumentClientException ex)
            {
                throw new InvalidOperationException("");
            }
        }

        public T FirstOrDefault()
        {
            return queryable.AsEnumerable()
                            .FirstOrDefault();
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            return queryable.Where(predicate)
                            .AsEnumerable()
                            .FirstOrDefault();
        }

        public IDocumentDbQquery<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return new DocumentDbQuery<TResult>(documentClient, db, collection, queryable.Select(selector));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return queryable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
