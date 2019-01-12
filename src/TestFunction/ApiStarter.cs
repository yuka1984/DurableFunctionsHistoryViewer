using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Sample
{
    public static class ApiStarter
    {
        public const string Name = "Starter_Api";

        [FunctionName(Name)]
        public static async Task<HttpResponseMessage> Func([HttpTrigger(AuthorizationLevel.Function, "Get", Route = "start")]HttpRequestMessage request ,[OrchestrationClient]DurableOrchestrationClientBase client)
        {
            var instanceId = await client.StartNewAsync(SampleOrchestrator.Name, null);
            return client.CreateCheckStatusResponse(request, instanceId);
        }
    }
}
