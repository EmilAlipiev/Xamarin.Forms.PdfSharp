﻿using Plugin.Xamarin.Forms.PdfSharp.Attributes;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using Xamarin.Forms;

namespace Plugin.Xamarin.Forms.PdfSharp.Renderers
{
	[PdfRenderer(ViewType = typeof(Label))]
	public class PDFLabelRenderer : PdfRendererBase<Label>
	{
		public override void CreatePDFLayout(XGraphics page, Label label, XRect bounds, double scaleFactor)
		{
			XFont font = new XFont(label.FontFamily ?? GlobalFontSettings.FontResolver.DefaultFontName, label.FontSize * scaleFactor);
			Color textColor = label.TextColor != default(Color) ? label.TextColor : Color.Black;

			if (label.BackgroundColor != default(Color))
				page.DrawRectangle(label.BackgroundColor.ToXBrush(), bounds);

			if (!string.IsNullOrEmpty(label.Text))
				page.DrawString(label.Text, font, textColor.ToXBrush(), bounds,
					new XStringFormat() {
						Alignment = label.HorizontalTextAlignment.ToXStringAlignment(),
						LineAlignment = label.VerticalTextAlignment.ToXLineAlignment(),
					});
		}
	}
}
