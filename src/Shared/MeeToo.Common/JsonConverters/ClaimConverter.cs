using System;
using System.Collections.Generic;
using System.Security.Claims;
using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MeeToo.Common.JsonConverters
{
    public class ClaimConverter : JsonConverter
    {
        private readonly IDictionary<string, string> propertiesMap = new Dictionary<string, string>
        {
            {nameof(Claim.Value), nameof(Claim.Value).Camelize()},
            {nameof(Claim.Type), nameof(Claim.Type).Camelize()},
            {nameof(Claim.ValueType), nameof(Claim.ValueType).Camelize()},
            {nameof(Claim.Issuer), nameof(Claim.Issuer).Camelize()},
            {nameof(Claim.OriginalIssuer), nameof(Claim.OriginalIssuer).Camelize()}
        };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Claim));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var claim = (Claim)value;
            var jo = new JObject
            {
                {propertiesMap[nameof(Claim.Type)], claim.Type},
                {propertiesMap[nameof(Claim.Value)], IsJson(claim.Value) ? new JRaw(claim.Value) : new JValue(claim.Value)},
                {propertiesMap[nameof(Claim.ValueType)], claim.ValueType},
                {propertiesMap[nameof(Claim.Issuer)], claim.Issuer},
                {propertiesMap[nameof(Claim.OriginalIssuer)], claim.OriginalIssuer}
            };

            jo.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            JToken token = jo[propertiesMap[nameof(Claim.Value)]];

            var type = jo.Value<string>(propertiesMap[nameof(Claim.Type)]);
            var value = token.Type == JTokenType.String ? (string)token : token.ToString(Formatting.None);
            var valueType = jo.Value<string>(propertiesMap[nameof(Claim.ValueType)]);
            var issuer = jo.Value<string>(propertiesMap[nameof(Claim.Issuer)]);
            var originalIssuer = jo.Value<string>(propertiesMap[nameof(Claim.OriginalIssuer)]);

            return new Claim(type, value, valueType, issuer, originalIssuer);
        }

        private static bool IsJson(string val)
        {
            return (!string.IsNullOrEmpty(val) &&
                    ((val.StartsWith("[") && val.EndsWith("]")) ||
                     (val.StartsWith("{") && val.EndsWith("}"))));
        }
    }
}
