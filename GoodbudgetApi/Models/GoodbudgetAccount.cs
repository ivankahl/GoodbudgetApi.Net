using System;
using GoodbudgetApi.Enums;
using Newtonsoft.Json;

namespace GoodbudgetApi.Models
{
    public class GoodbudgetAccount
    {
        [JsonProperty("Uuid")]
        public string Uuid { get; internal set; }

        [JsonProperty("Id")]
        public long Id { get; internal set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("AccountType")]
        public GoodbudgetAccountType AccountType { get; set; }

        [JsonProperty("CurrentBalance")]
        public double CurrentBalance { get; internal set; }
    }
}
