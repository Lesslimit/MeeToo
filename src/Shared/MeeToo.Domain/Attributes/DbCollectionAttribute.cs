using System;

namespace MeeToo.Domain.Attributes
{
    public class DbCollectionAttribute : Attribute
    {
        public string Id { get; private set; }

        public DbCollectionAttribute(string id)
        {
            this.Id = id;
        }
    }
}
