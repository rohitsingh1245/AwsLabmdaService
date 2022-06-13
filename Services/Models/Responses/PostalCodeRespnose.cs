using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Services.Models.Responses
{
   public class PostalCodeRespnose: ResponseStatus
    {
      
        [JsonPropertyName("result")]
        public List<string> result { get; set; }
    }
}
