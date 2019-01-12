using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Sample
{
    public static class ErrorNotifyActivity
    {
        public const string Name = "Activity_ErrorNotify";

        [FunctionName(Name)]
        public static void Func(
            [ActivityTrigger] DurableActivityContext context
            ,ILogger logger)
        {
            logger.LogError("Notify Error");
        }
    }
}
