using System;

namespace Plugin.Xamarin.Forms.PdfSharp.Attributes
{
	internal class PdfRendererAttribute : Attribute
	{
		public Type ViewType { get; set; }
	}
}
