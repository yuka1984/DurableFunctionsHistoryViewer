using System;

namespace RazorLightCustom.Compilation
{
    public interface ITemplateFactoryProvider
    {
		Func<ITemplatePage> CreateFactory(CompiledTemplateDescriptor templateDescriptor);
	}
}