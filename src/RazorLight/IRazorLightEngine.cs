using System;
using System.Dynamic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RazorLight
{
	public interface IRazorLightEngine
    {
		RazorLightOptions Options { get; }

        Task<string> CompileRenderAsync<T>(string key, T model, ExpandoObject viewBag = null, bool isEncoding = true);
		Task<string> CompileRenderAsync(string key, object model, Type modelType, ExpandoObject viewBag = null, bool isEncoding = true);

		Task<string> CompileRenderStringAsync<T>(string key, string content, T model, ExpandoObject viewBag = null, bool isEncoding = true);

		Task<ITemplatePage> CompileTemplateAsync(string key, bool isEncoding = true);

        Task<string> RenderTemplateAsync(ITemplatePage templatePage, object model, Type modelType, ExpandoObject viewBag = null);
        Task<string> RenderTemplateAsync<T>(ITemplatePage templatePage, T model, ExpandoObject viewBag = null);
        Task RenderTemplateAsync(ITemplatePage templatePage, object model, Type modelType, TextWriter textWriter, ExpandoObject viewBag = null);
	}
}