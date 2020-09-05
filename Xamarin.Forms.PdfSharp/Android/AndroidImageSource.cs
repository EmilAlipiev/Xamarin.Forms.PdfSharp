using System;
using System.IO;
using System.Linq;
using Android.Graphics.Drawables;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using static Android.Graphics.Bitmap;
namespace Plugin.Xamarin.Forms.PdfSharp.Droid
{
	public class AndroidImageSource : ImageSource
	{
		protected override IImageSource FromBinaryImpl(string name, Func<byte[]> imageSource, int? quality = 75)
		{
			return new AndroidImageSourceImpl(name, () => { return new MemoryStream(imageSource.Invoke()); }, (int)quality);
		}

		protected override IImageSource FromFileImpl(string path, int? quality = 75)
		{
			if (path.Contains("."))
			{
				string[] tokens = path.Split('.');
				tokens = tokens.Take(tokens.Length - 1).ToArray();
				path = String.Join(".", tokens);
			}

			var res = global::Xamarin.Forms.Forms.Context.Resources;
			var resId = res.GetIdentifier(path, "drawable", res.GetResourcePackageName(Android.Resource.Id.Home));
			Stream stream = new MemoryStream();
			BitmapDrawable drawable = null;
			if (resId > 0)
			{
				drawable = res.GetDrawable(resId) as BitmapDrawable;
				if (drawable != null)
				{
					drawable.Bitmap.Compress(CompressFormat.Jpeg, quality ?? 75, stream);
				}
			}

			return new AndroidImageSourceImpl(path, () => stream, quality ?? 75) { Bitmap = drawable?.Bitmap };
		}

		protected override IImageSource FromStreamImpl(string name, Func<Stream> imageStream, int? quality = 75)
		{
			return new AndroidImageSourceImpl(name, imageStream, (int)quality);
		}
	}
}