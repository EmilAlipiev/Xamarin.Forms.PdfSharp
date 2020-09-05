using System;
using System.IO;
using System.Threading;
using Foundation;
using static MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource;

namespace Plugin.Xamarin.Forms.PdfSharp.iOS
{

	internal class IosImageSourceImpl : IImageSource
	{
		internal UIKit.UIImage image;

		private Orientation Orientation { get; }

		private readonly Func<Stream> _streamSource;
		private readonly int _quality;

		public int Width { get; }
		public int Height { get; }
		public string Name { get; }


		public IosImageSourceImpl(string name, Func<Stream> streamSource, int quality)
		{
			Name = name;
			_streamSource = streamSource;
			_quality = quality;
			using (var stream = streamSource.Invoke())
			{
				image = UIKit.UIImage.LoadFromData(NSData.FromStream(stream));
				var size = image?.Size ?? new CoreGraphics.CGSize(0, 0);

				Width = (int)size.Width;
				Height = (int)size.Height;
				Orientation = Orientation.Normal;
			}
		}

		public void SaveAsJpeg(MemoryStream ms, CancellationToken ct)
		{
			var jpg = image.AsJPEG();
			ms.Write(jpg.ToArray(), 0, (int)jpg.Length);
			ms.Seek(0, SeekOrigin.Begin);
		}
	}
}