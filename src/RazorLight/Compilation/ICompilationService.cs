using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using RazorLightCustom.Generation;
using System.Reflection;
using System.Threading.Tasks;

namespace RazorLightCustom.Compilation
{
    public interface ICompilationService
    {
        CSharpCompilationOptions CSharpCompilationOptions { get; }
        EmitOptions EmitOptions { get; }
        CSharpParseOptions ParseOptions { get; }
        Assembly OperatingAssembly { get; }        
        (byte[] AssemblyStream, byte[] PdbStream) Compile(IGeneratedRazorTemplate razorTemplate);
        Assembly Emit(byte[] assemblyStream, byte[] pdbStream);
    }
}