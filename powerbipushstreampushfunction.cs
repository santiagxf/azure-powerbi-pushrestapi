using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace Analytics.RealTimeAnalytics.FraudDetection
{
    public static class powerbipushstreampushfunction
    {
        private static readonly string APICONFIG = "powerBiApiUrl";

        [FunctionName("powerbipushstreampushfunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string powerBiApiUrl = System.Environment.GetEnvironmentVariable(APICONFIG, EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(powerBiApiUrl)) {
                throw new WebException($"{APICONFIG} not defined");
            }
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return PostData(powerBiApiUrl, data ,log)
        }

        static async Task<httpresponsemessage> PostData(String powerBIurl, dynamic data, TraceWriter log)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(powerBIurl, data);
            response.EnsureSuccessStatusCode();
            log.Info($"{NAME} response: {response.ReasonPhrase}");
            return response;
        }     
    }
}
