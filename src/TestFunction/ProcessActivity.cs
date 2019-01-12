using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Sample
{
    public static class ProcessActivity
    {
        public const string Name = "Activity_Process";

        [FunctionName(Name)]
        public static bool Func([ActivityTrigger] string input)
        {
            return input != "b";
        }
    }
}
