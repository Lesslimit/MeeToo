using System;

namespace MeeToo.Security.Identity
{
    public class IdentityUserLogin<TKey> where TKey : IEquatable<TKey>
    {
        public virtual string LoginProvider { get; set; }

        public virtual string ProviderKey { get; set; }

        public virtual string ProviderDisplayName { get; set; }

        public virtual TKey UserId { get; set; }
    }
}
