using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DurableFunctionsHistoryViewer;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RazorLightCustom;

[assembly: WebJobsStartup(typeof(Startup))]

namespace DurableFunctionsHistoryViewer
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<Dfhv>()
                .BindOptions<Option>()
                .Services.AddSingleton<IConnectionStringResolver, WebJobsConnectionStringProvider>()
                .AddSingleton(c =>
                {
                    IRazorLightEngine engine = new RazorLightEngineBuilder()
                        .UseEmbeddedResourcesProject(typeof(Startup))
                        .UseMemoryCachingProvider()
                        .SetOperatingAssembly(Assembly.GetExecutingAssembly())
                        .Build();
                    return engine;
                });
        }
    }
}
