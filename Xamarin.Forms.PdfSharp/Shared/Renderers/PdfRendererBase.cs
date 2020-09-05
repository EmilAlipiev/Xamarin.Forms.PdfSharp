using PdfSharpCore.Drawing;
using Xamarin.Forms;

namespace Plugin.Xamarin.Forms.PdfSharp.Renderers
{
	public abstract class PdfRendererBase<T> : PdfRendererBase where T : View
	{

		internal override void CreateLayout(XGraphics page, object view, XRect bound, double scaleFactor)
		{
			CreatePDFLayout(page, (T)view, bound, scaleFactor);
		}

		public abstract void CreatePDFLayout(XGraphics page, T view, XRect bounds, double scaleFactor);
	}

	public abstract class PdfRendererBase
	{
		internal abstract void CreateLayout(XGraphics page, object view, XRect bound, double scaleFactor);
	}
}
