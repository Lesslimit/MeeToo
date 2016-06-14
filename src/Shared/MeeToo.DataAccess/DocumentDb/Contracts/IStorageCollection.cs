using System.Threading;
using System.Threading.Tasks;
using MeeToo.Domain.Contracts;

namespace MeeToo.DataAccess.DocumentDb
{
    public interface IStorageCollection<T> where T : IDocument
    {
        IDocumentDbQquery<T> Query();
        Task AddAsync(T document, CancellationToken cancelToken = default(CancellationToken));
        Task UpdateAsync(T document);
        Task DeleteAsync(T document);
    }
}
