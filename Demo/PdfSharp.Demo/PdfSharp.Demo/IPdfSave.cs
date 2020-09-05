using PdfSharpCore.Pdf;

namespace PdfSharp.Demo
{
	public interface IPdfSave
	{
		void Save(PdfDocument doc, string fileName);
	}
}
