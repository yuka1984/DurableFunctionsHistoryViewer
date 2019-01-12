using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;


namespace Sample
{
    public static class SampleOrchestrator
    {
        public const string Name = "Orhestrator_Sample";

        [FunctionName(Name)]
        public static async Task Func([OrchestrationTrigger]DurableOrchestrationContextBase context)
        {
            var data = await context.CallActivityAsync<string[]>(GetDataActivity.Name, null);

            await context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(60), CancellationToken.None);

            var tasks = data.Select(x => context.CallActivityAsync<bool>(ProcessActivity.Name, x));

            var results = await Task.WhenAll(tasks);

            if (results.Any(x => !x))
            {
                await context.CallActivityAsync(ErrorNotifyActivity.Name, null);
            }
        }
    }
}
