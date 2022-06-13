using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class SearchParameter
    {
        public string Url { get; set; }
        public int MaxLimit { get; set; }
        public string PostalCode { get; set; }
    }
}
