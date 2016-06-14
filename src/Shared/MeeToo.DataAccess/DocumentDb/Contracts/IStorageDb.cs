using System.Threading.Tasks;
using MeeToo.Domain.Contracts;

namespace MeeToo.DataAccess.DocumentDb
{
    public interface IStorageDb
    {
        Task CreateIfNotExists(string id);
        Task<IStorageCollection<T>> CollectionAsync<T>(string id = null) where T : IDocument;
    }
}
