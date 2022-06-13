using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Services.Models.Responses
{
    public class ResponseStatus
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("message")]
        [JsonIgnore]
        public string Message { get; set; }
    }
}
