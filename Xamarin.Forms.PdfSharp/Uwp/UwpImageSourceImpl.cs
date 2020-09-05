using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using static MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource;

namespace Plugin.Xamarin.Forms.PdfSharp.UWP
{

	internal class UwpImageSourceImpl : IImageSource
	{
		private readonly int _quality;
		private readonly Func<IRandomAccessStream> _getRas;

		public int Width { get; }

		public int Height { get; }

		public string Name { get; }

		public UwpImageSourceImpl(string name, Func<IRandomAccessStream> getRas, int quality)
		{
			Name = name;
			_getRas = getRas;
			_quality = quality;
			using (var ras = getRas.Invoke())
			{
				var decoder = GetDecoder(ras);
				Width = (int)decoder.OrientedPixelWidth;
				Height = (int)decoder.OrientedPixelHeight;
			}
		}

		private BitmapDecoder GetDecoder(IRandomAccessStream ras)
		{
			return Task.Run(async () => { return await BitmapDecoder.CreateAsync(ras); }).Result;
		}

		public void SaveAsJpeg(MemoryStream ms, CancellationToken ct)
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			ct.Register(() => {
				tcs.TrySetCanceled();
			});
			using (var ras = _getRas.Invoke())
			{
				var decoder = GetDecoder(ras);
				var task = Task.Run(async () => {
					ct.ThrowIfCancellationRequested();
					var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, ms.AsRandomAccessStream(),
						new BitmapPropertySet() {
							 { "ImageQuality", new BitmapTypedValue(Convert.ToSingle(_quality) / 100, PropertyType.Single) }
						});
					ct.ThrowIfCancellationRequested();
					encoder.SetPixelData(decoder.BitmapPixelFormat, decoder.BitmapAlphaMode, decoder.OrientedPixelWidth, decoder.OrientedPixelHeight, decoder.DpiX, decoder.DpiY, (await decoder.GetPixelDataAsync()).DetachPixelData());
					ct.ThrowIfCancellationRequested();
					await encoder.FlushAsync();
				});
				Task.WaitAny(task, tcs.Task);
				tcs.TrySetCanceled();
				ct.ThrowIfCancellationRequested();
				if (task.IsFaulted)
					throw task.Exception;
			}
		}
	}
}
