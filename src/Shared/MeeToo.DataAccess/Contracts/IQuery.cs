using System.Linq;

namespace MeeToo.DataAccess.Contracts
{
    public interface IQuery<T> : IOrderedQueryable<T>
    {
    }
}
