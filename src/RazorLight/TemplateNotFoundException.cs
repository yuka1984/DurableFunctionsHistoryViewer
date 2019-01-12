using System;

namespace RazorLightCustom
{
    public class TemplateNotFoundException : RazorLightException
    {
		public TemplateNotFoundException(string message) : base(message) { }

		public TemplateNotFoundException(string message, Exception exception) : base(message, exception) { }
	}
}
