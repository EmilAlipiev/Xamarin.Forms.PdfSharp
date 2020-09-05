using Plugin.Xamarin.Forms.PdfSharp.Attributes;
using PdfSharpCore.Drawing;
using Xamarin.Forms;

namespace Plugin.Xamarin.Forms.PdfSharp.Renderers
{
	[PdfRenderer(ViewType = typeof(BoxView))]
	public class PdfBoxViewRenderer : PdfRendererBase<BoxView>
	{
		public override void CreatePDFLayout(XGraphics page, BoxView box, XRect bounds, double scaleFactor)
		{
			if (box.BackgroundColor != default(Color))
				page.DrawRectangle(box.BackgroundColor.ToXBrush(), bounds);

			if (box.Color != default(Color))
				page.DrawRectangle(box.Color.ToXBrush(), bounds);
		}
	}
}
