using System;
using System.Dynamic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using RazorLight.Caching;
using RazorLight.Compilation;

namespace RazorLight
{
	public interface IEngineHandler
	{
		ICachingProvider Cache { get; }
		IRazorTemplateCompiler Compiler { get; }
		ITemplateFactoryProvider FactoryProvider { get; }

		RazorLightOptions Options { get; }
		bool IsCachingEnabled { get; }

		Task<ITemplatePage> CompileTemplateAsync(string key, bool isEncoding = true);

		Task<string> CompileRenderAsync<T>(string key, T model, ExpandoObject viewBag = null, bool isEncoding = true);
		Task<string> CompileRenderStringAsync<T>(string key, string content, T model, ExpandoObject viewBag = null, bool isEncoding = true);

		Task<string> RenderTemplateAsync<T>(ITemplatePage templatePage, T model, ExpandoObject viewBag = null);
		Task RenderTemplateAsync<T>(ITemplatePage templatePage, T model, TextWriter textWriter, ExpandoObject viewBag = null);
		Task RenderIncludedTemplateAsync<T>(ITemplatePage templatePage, T model, TextWriter textWriter, ExpandoObject viewBag, TemplateRenderer templateRenderer);
	}
}