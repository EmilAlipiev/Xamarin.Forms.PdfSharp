using System;
using System.IO;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;

namespace Plugin.Xamarin.Forms.PdfSharp.UWP
{
	public class UwpImageSource : ImageSource
	{
		protected override IImageSource FromBinaryImpl(string name, Func<byte[]> imageSource, int? quality = 75)
		{
			return new UwpImageSourceImpl(name,
				() => {
					return new MemoryStream(imageSource.Invoke()).AsRandomAccessStream();
				}, (int)quality);
		}

		protected override IImageSource FromFileImpl(string path, int? quality = 75)
		{
			return new UwpImageSourceImpl(Path.GetFileName(path),
				() => {
					var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
					return stream.AsRandomAccessStream();
				}, (int)quality);
		}

		protected override IImageSource FromStreamImpl(string name, Func<Stream> imageStream, int? quality = 75)
		{
			return new UwpImageSourceImpl(name,
				() => {
					return imageStream.Invoke().AsRandomAccessStream();
				}, (int)quality);
		}

	}
}
