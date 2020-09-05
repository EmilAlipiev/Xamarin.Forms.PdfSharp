using Plugin.Xamarin.Forms.PdfSharp.Delegates;
using PdfSharpCore.Drawing;
using Xamarin.Forms;

namespace Plugin.Xamarin.Forms.PdfSharp.Renderers
{
	public class PDFListViewRenderer : PdfRendererBase<ListView>
	{
		public override void CreatePDFLayout(XGraphics page, ListView listView, XRect bounds, double scaleFactor)
		{
			var listViewDelegate = listView.GetValue(PdfRendererAttributes.ListRendererDelegateProperty) as PdfListViewRendererDelegate;

			int numSections = listViewDelegate.GetNumberOfSections(listView);

			XPoint offset = bounds.TopLeft;

			for (int section = 0; section < listViewDelegate.GetNumberOfSections(listView); section++)
			{
				if (listView.HeaderTemplate != null)
				{
					var headerBound = new XRect(bounds.X + offset.X,
												bounds.Y + offset.Y,
												offset.X + bounds.Width,
												offset.Y + listViewDelegate.GetHeaderHeight(listView, section) * scaleFactor);

					listViewDelegate.DrawHeader(listView, section, page, headerBound, scaleFactor);
					offset.Y += headerBound.Height;
				}
				for (int row = 0; row < listViewDelegate.GetNumberOfRowsInSection(listView, section); row++)
				{
					var rowbound = new XRect(bounds.X + offset.X,
											 bounds.Y + offset.Y,
											 offset.X + bounds.Width,
											 offset.Y + listViewDelegate.GetCellHeight(listView, section, row) * scaleFactor);

					listViewDelegate.DrawCell(listView, section, row, page, bounds, scaleFactor);
					offset.Y += rowbound.Height;
				}
				if (listView.FooterTemplate != null)
				{
					var footerBound = new XRect(bounds.X + offset.X,
												bounds.Y + offset.Y,
												offset.X + bounds.Width,
												offset.Y + listViewDelegate.GetFooterHeight(listView, section) * scaleFactor);

					listViewDelegate.DrawFooter(listView, section, page, footerBound, scaleFactor);
					offset.Y += footerBound.Height;
				}
			}
		}
	}
}
