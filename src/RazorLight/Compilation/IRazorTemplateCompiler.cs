using System.Threading.Tasks;

namespace RazorLightCustom.Compilation
{
	public interface IRazorTemplateCompiler
	{
		ICompilationService CompilationService { get; }

		Task<CompiledTemplateDescriptor> CompileAsync(string templateKey);
	}
}