using Amazon.Lambda.Core;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Util
{
    /// <summary>
    /// Stores logs and returns Parameter Store
    /// </summary>
    public class GlobalSettings
    {

 
        private static readonly AmazonSimpleSystemsManagementClient AmazonSsm = new AmazonSimpleSystemsManagementClient();
        /// <summary>
        /// returns the parameters created in aws cloud
        /// </summary>

        public static async Task<string> GetGlobalConfigVariable(string name, bool refreshCache = false, ILambdaContext context = null)
        {

            string parameterValue = "";
            try
            {

                parameterValue = await GetCacheAsync(name, refreshCache);

            }
            catch (Exception ex)
            {
                ContextLoggerHandler(context, string.Format("GetGlobalConfigVariable:Error{0}", ex.Message));
            }
         
            return parameterValue;
        }
        /// <summary>
        /// returns the Parameter Store
        /// </summary>

        public static async Task<string> GetCacheAsync(string key, bool refreshCache, string cacheStorage = "ssm")
        {
           
            string keyValue = null;
            try
            {
                var getParameterRequest = new GetParameterRequest
                {
                    WithDecryption = refreshCache,
                    Name = key
                };
                var parameterResponse = await AmazonSsm.GetParameterAsync(getParameterRequest);
                if (parameterResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    keyValue = parameterResponse.Parameter.Value;
                }

            }
            catch (Exception ex) {
                throw ex;
            }
         
            return keyValue;
        }
        public static void ContextLoggerHandler(ILambdaContext context, string log)
        {
            context.Logger.LogLine(string.Format("[PostalCode-ContextLogger] Function name:AutocompletePostalCode{0}:{1} ", context.FunctionName, log));
        }
    }
}
