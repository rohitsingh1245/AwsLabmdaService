using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Services.Models;
using Services.Models.Responses;
using Services.Services;
using Services.Util;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PostalCodeDetails
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<Response> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            GlobalSettings.ContextLoggerHandler(context, string.Format("input:{0}", input));

            input.QueryStringParameters.TryGetValue("postalCode", out var postalCode);
            var response = new Response() { Status = (int)HttpStatusCode.OK };
            if (string.IsNullOrEmpty(postalCode))
                return new Response() { Status = (int)HttpStatusCode.BadRequest, Message = "Invalid request" };

            try
            {
                var url = await GlobalSettings.GetGlobalConfigVariable("postalcode.url", false, context);

                if (string.IsNullOrEmpty(url))
                    return new Response() { Status = (int)HttpStatusCode.InternalServerError, Message = "Url not found" };
                var parameter = new SearchParameter { PostalCode = postalCode, Url = url};
                var payload = await PostalCodeService.GetPostalCodeDetails(parameter);
                GlobalSettings.ContextLoggerHandler(context, string.Format("payload executed"));
                if (payload == null)
                {
                    return new Response() { Status = (int)HttpStatusCode.NotFound, Message = "Record Not Found" };
                }
      
                var strPayload = JsonSerializer.Serialize(payload);
                GlobalSettings.ContextLoggerHandler(context, string.Format("PayLoand:{0}", strPayload));
               
                response = new Response()
                {
                    Status = (int)HttpStatusCode.OK,
                    Payload = payload
                };

            }
            catch (Exception e)
            {
                GlobalSettings.ContextLoggerHandler(context, string.Format("Exception:{0},{1}", e.Source, e.Message));
                response = new Response()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Message = e.Message
                };

            }
            return response;
        }
        

    }
}
