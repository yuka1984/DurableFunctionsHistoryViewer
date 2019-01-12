using Microsoft.AspNetCore.Razor.Language;
using RazorLightCustom.Razor;

namespace RazorLightCustom.Generation
{
	public interface IGeneratedRazorTemplate
	{
		string TemplateKey { get; }

		string GeneratedCode { get; }

		RazorLightProjectItem ProjectItem { get; set; }
	}
}