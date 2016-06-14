using Newtonsoft.Json;
using MeeToo.Domain.Attributes;
using MeeToo.Domain.Contracts;

// ReSharper disable VirtualMemberCallInConstructor

namespace MeeToo.Security.Identity
{
    [DbCollection("identities")]
    [JsonObject(MemberSerialization.OptIn)]
    public class MeeTooUser : IdentityUserBase<string, IdentityUserClaim<string>, string, IdentityUserLogin<string>>, IDocument
    {
        public override string Email => Id;

        public MeeTooUser(string email) : base(email)
        {
            Id = email;
            UserName = email;
        }
    }
}
