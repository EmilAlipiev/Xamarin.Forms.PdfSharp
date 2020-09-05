using Plugin.Xamarin.Forms.PdfSharp.Attributes;
using PdfSharpCore.Drawing;
using Xamarin.Forms;

namespace Plugin.Xamarin.Forms.PdfSharp.Renderers
{
	[PdfRenderer(ViewType = typeof(ContentView))]
	public class PdfContentViewRenderer : PdfRendererBase<ContentView>
	{
		public override void CreatePDFLayout(XGraphics page, ContentView view, XRect bounds, double scaleFactor)
		{
			if (view.BackgroundColor != null)
				page.DrawRectangle(view.BackgroundColor.ToXBrush(), bounds);
		}
	}
}
