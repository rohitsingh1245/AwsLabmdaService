using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Services.Models.Responses
{
    public class Response: ResponseStatus
    {
     
        [JsonPropertyName("result")]
        public object Payload { get; set; }

    }
}
