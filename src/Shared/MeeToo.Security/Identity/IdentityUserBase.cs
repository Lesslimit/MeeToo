using System;
using System.Collections.Generic;
using Newtonsoft.Json;

// ReSharper disable VirtualMemberCallInConstructor

namespace MeeToo.Security.Identity
{
    [JsonObject]
    public abstract class IdentityUserBase<TKey, TUserClaim, TUserRole, TUserLogin> where TKey : IEquatable<TKey>
    {
        protected IdentityUserBase() { }

        protected IdentityUserBase(string userName) : this()
        {
            UserName = userName;
        }

        [JsonProperty("id")]
        public virtual TKey Id { get; set; }

        public virtual string Email { get; set; }

        [JsonProperty("userName")]
        public virtual string UserName { get; set; }

        [JsonProperty("emailConfirmed")]
        public virtual bool EmailConfirmed { get; set; }

        [JsonProperty("passwordHash")]
        public virtual string PasswordHash { get; set; }

        [JsonProperty("securityStamp")]
        public virtual string SecurityStamp { get; set; }

        [JsonProperty("concurrencyStamp")]
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("phoneNumber")]
        public virtual string PhoneNumber { get; set; }

        [JsonProperty("phoneNumberConfirmed")]
        public virtual bool PhoneNumberConfirmed { get; set; }

        [JsonProperty("twoFactorEnabled")]
        public virtual bool TwoFactorEnabled { get; set; }

        [JsonProperty("lockoutEnd")]
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        [JsonProperty("lockoutEnabled")]
        public virtual bool LockoutEnabled { get; set; }

        [JsonProperty("accessFailedCount")]
        public virtual int AccessFailedCount { get; set; }

        [JsonProperty("roles")]
        public virtual ICollection<TUserRole> Roles { get; } = new List<TUserRole>();

        [JsonProperty("claims")]
        public virtual ICollection<TUserClaim> Claims { get; } = new List<TUserClaim>();

        [JsonProperty("logins")]
        public virtual ICollection<TUserLogin> Logins { get; } = new List<TUserLogin>();

        public override string ToString()
        {
            return UserName;
        }
    }
}
