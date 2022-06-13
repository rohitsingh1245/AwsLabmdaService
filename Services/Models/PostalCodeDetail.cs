using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Services.Models
{
    public class PostalCodeDetail
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("region")]
        public string Region { get; set; }
        [JsonPropertyName("adminDistrict")]
        public string AdminDistrict { get; set; }
        [JsonPropertyName("parliamentaryConstituency")]
        public string ParliamentaryConstituency { get; set; }
        [JsonPropertyName("area")]
        public string Area { get; set; }
    }
}
