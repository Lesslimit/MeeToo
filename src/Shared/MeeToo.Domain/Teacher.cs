using Newtonsoft.Json;
using MeeToo.Domain.Attributes;

namespace MeeToo.Domain
{
    [DbCollection("teachers")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Teacher: User
    {
    }
}
