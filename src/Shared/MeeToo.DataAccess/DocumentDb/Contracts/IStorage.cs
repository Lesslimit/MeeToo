using System;

namespace MeeToo.DataAccess.DocumentDb
{
    public interface IStorage : IDisposable
    {
        IStorageDb Db(string id = null);
    }
}
