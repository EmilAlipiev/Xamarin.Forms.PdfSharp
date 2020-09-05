using System;
using PdfSharpCore;
using PdfSharpCore.Drawing;

namespace Plugin.Xamarin.Forms.PdfSharp.Utils
{
	public class SizeUtils
	{
		public static XSize GetPageSize(PageSize pageSize)
		{
			switch (pageSize)
			{
				case PageSize.A0:
					return new XSize(841, 1189);
				case PageSize.A1:
					return new XSize(594, 841);
				case PageSize.A2:
					return new XSize(420, 594);
				case PageSize.A3:
					return new XSize(297, 420);
				case PageSize.A4:
					return new XSize(210, 297);
				case PageSize.A5:
					return new XSize(148, 210);
				case PageSize.B0:
					return new XSize(1000, 1414);
				case PageSize.B1:
					return new XSize(707, 1000);
				case PageSize.B2:
					return new XSize(500, 707);
				case PageSize.B3:
					return new XSize(353, 500);
				case PageSize.B4:
					return new XSize(250, 353);
				case PageSize.B5:
					return new XSize(176, 250);
				case PageSize.Crown:
					return new XSize(508, 381);
				case PageSize.Demy:
					return new XSize(572, 445);
				case PageSize.DoubleDemy:
					return new XSize(889, 597);
				case PageSize.Elephant:
					return new XSize(711, 584);
				case PageSize.Executive:
					return new XSize(267, 184);
				case PageSize.Folio:
					return new XSize(216, 330);
				case PageSize.Foolscap:
					return new XSize(330, 203);
				case PageSize.GovernmentLetter:
					return new XSize(267, 203);
				case PageSize.LargePost:
					return new XSize(533, 419);
				case PageSize.Ledger:
					return new XSize(432, 279);
				case PageSize.Legal:
					return new XSize(356, 216);
				case PageSize.Letter:
					return new XSize(279, 216);
				case PageSize.Medium:
					return new XSize(584, 457);
				case PageSize.Post:
					return new XSize(489, 394);
				case PageSize.QuadDemy:
					return new XSize(1143, 889);
				case PageSize.Quarto:
					return new XSize(254, 203);
				case PageSize.RA0:
					return new XSize(860, 1220);
				case PageSize.RA1:
					return new XSize(610, 860);
				case PageSize.RA2:
					return new XSize(430, 610);
				case PageSize.RA3:
					return new XSize(305, 430);
				case PageSize.RA4:
					return new XSize(215, 305);
				case PageSize.RA5:
					return new XSize(153, 215);
				case PageSize.Royal:
					return new XSize(635, 508);
				case PageSize.Size10x14:
					return new XSize(254, 355);
				case PageSize.Statement:
					return new XSize(391, 216);
				case PageSize.STMT:
					return new XSize(216, 396);
				case PageSize.Tabloid:
					return new XSize(432, 279);
				case PageSize.Undefined:
				default:
					throw new ArgumentException("Invalid Size");



			}
		}

		public static XRect GetAvailablePageSize(PageSize pageSize, PageOrientation orientation)
		{
			//TODO implement for accurate margin
			var size = GetPageSize(pageSize);
			if (orientation == PageOrientation.Landscape)
				size = new XSize(size.Height, size.Width);

			double margin = (size.Width + size.Height) * 0.04;
			return new XRect(margin, margin, size.Width - 2 * margin, size.Height - margin);
		}
	}
}
