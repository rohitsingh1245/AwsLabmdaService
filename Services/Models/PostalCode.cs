using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Services.Models
{
    public class PostalCode
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("area")]
        public string Area { get; set; }
    }
}
