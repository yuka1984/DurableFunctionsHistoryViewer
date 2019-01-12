using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RazorLightCustom.DependencyInjection;

namespace RazorLightCustom.Extensions
{
	public static class ServiceCollectionExtensions
	{
        public static void AddRazorLight(this IServiceCollection services, Func<IRazorLightEngine> engineFactoryProvider)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if(engineFactoryProvider == null)
            {
                throw new ArgumentNullException(nameof(engineFactoryProvider));
            }

            services.AddSingleton<PropertyInjector>();
			services.AddSingleton<IEngineHandler, EngineHandler>();
            services.AddSingleton<IRazorLightEngine>(p => 
            {
                var engine = engineFactoryProvider();
                AddEngineRenderCallbacks(engine, p);

                return engine;
            });
        }

		private static void AddEngineRenderCallbacks(IRazorLightEngine engine, IServiceProvider services)
		{
			var injector = services.GetRequiredService<PropertyInjector>();

			engine.Options.PreRenderCallbacks.Add(template => injector.Inject(template));
		}
	}
}
