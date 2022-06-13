using Services.Models.Requests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;

namespace Services.Util
{
    public class HttpClientWrapper
    {
       
        public static async Task<T> GetAsync<T>(Request request)
        {
         
            var obj = new object();
            if (string.IsNullOrWhiteSpace(request.Url))
                throw new ArgumentException("url is required.");


            using (var client = new HttpClient())
            {
                HttpResponseMessage response = new HttpResponseMessage();

                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = client.GetAsync(request.Url).Result;
                //if (response.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    token = GetToken().Result;
                //}
                //else
                //{
                //    break;
                //}


                var content = response.Content;
                string data = await content.ReadAsStringAsync();

                if (data == null) return (T)obj;
                var responseData = JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return (T)responseData;
            }

        }
    }
}
