using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoodbudgetApi.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GoodbudgetAccountType
    {
        [EnumMember(Value = "ASSET")]
        Asset,
        [EnumMember(Value = "LIAB")]
        Liability
    }
}
