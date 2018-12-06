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
using System.Net;

namespace Analytics.RealTimeAnalytics.FraudDetection
{
    public static class powerbipushstreampushfunction
    {
        private static readonly string APICONFIG = "powerBiApiUrl";
        private static readonly HttpClient client = new HttpClient();

        [FunctionName("powerbipushstreampushfunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            //string powerBiApiUrl = Environment.GetEnvironmentVariable(APICONFIG, EnvironmentVariableTarget.Process);
            string powerBiApiUrl = @"https://api.powerbi.com/beta/72f988bf-86f1-41af-91ab-2d7cd011db47/datasets/c785bceb-1185-44b0-b58c-610009185bbb/rows?key=DgUXn4%2FSwTgCOsRdbKNcc5k7GifZB19QCLylxMBF7BL%2BBGX6xo9x39Y%2FSTXHHBoars8LYUVDOBgyyTxryKLpjQ%3D%3D";
            if (string.IsNullOrEmpty(powerBiApiUrl)) {
                throw new WebException($"{APICONFIG} not defined");
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            log.LogInformation($"Posting data to {powerBiApiUrl}");
            return await PostData(powerBiApiUrl, data, log);
        }

        static async Task<HttpResponseMessage> PostData(String powerBIurl, object data, ILogger log)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(powerBIurl, data);
            response.EnsureSuccessStatusCode();
            log.LogInformation($"Response: {response.ReasonPhrase}");
            return response;
        }     
    }
}
