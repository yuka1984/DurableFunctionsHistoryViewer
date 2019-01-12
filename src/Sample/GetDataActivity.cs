using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Sample
{
    public static class GetDataActivity
    {
        public const string Name = "Activity_GetData";

        [FunctionName(Name)]
        public static string[] Func([ActivityTrigger] DurableActivityContext context)
        {
            return new[] {"a", "b", "c"};
        }
    }
}
