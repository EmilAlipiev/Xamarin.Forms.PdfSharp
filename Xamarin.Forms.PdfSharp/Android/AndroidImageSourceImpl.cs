using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Android.Graphics;

using static Android.Graphics.Bitmap;
using static Android.Graphics.BitmapFactory;
using static MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource;

namespace Plugin.Xamarin.Forms.PdfSharp.Droid
{
    internal class AndroidImageSourceImpl : IImageSource
    {
        internal Bitmap Bitmap { get; set; }
        internal Stream Stream { get; set; }
        private Orientation Orientation { get; }

        private readonly Func<Stream> _streamSource;
        private readonly int _quality;

        public int Width { get; }
        public int Height { get; }
        public string Name { get; }

        public AndroidImageSourceImpl(string name, Func<Stream> streamSource, int quality)
        {
            Name = name;
            _streamSource = streamSource;
            _quality = quality;
            using (var stream = streamSource.Invoke())
            {
                Orientation = Orientation.Normal;
                stream.Seek(0, SeekOrigin.Begin);
                var options = new Options { InJustDecodeBounds = true };
#pragma warning disable CS0642 // Possible mistaken empty statement
                using (DecodeStream(stream, null, options))
                    ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                Width = Orientation == Orientation.Normal || Orientation == Orientation.Rotate180 ? options.OutWidth : options.OutHeight;
                Height = Orientation == Orientation.Normal || Orientation == Orientation.Rotate180 ? options.OutHeight : options.OutWidth;
            }
        }

        public void SaveAsJpeg(MemoryStream ms, CancellationToken ct)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            ct.Register(() =>
            {
                tcs.TrySetCanceled();
            });
            var task = Task.Run(() =>
            {
                Matrix mx = new Matrix();
                ct.ThrowIfCancellationRequested();
                //using (var bitmap = this.Bitmap; DecodeStream(_streamSource.Invoke()))
                //{
                switch (Orientation)
                {
                    case Orientation.Rotate90:
                        mx.PostRotate(90);
                        break;
                    case Orientation.Rotate180:
                        mx.PostRotate(180);
                        break;
                    case Orientation.Rotate270:
                        mx.PostRotate(270);
                        break;
                    default:
                        ct.ThrowIfCancellationRequested();
                        if (Bitmap != null)
                            Bitmap.Compress(CompressFormat.Jpeg, _quality, ms);
                        ct.ThrowIfCancellationRequested();
                        return;
                }
                ct.ThrowIfCancellationRequested();
                using (var flip = Android.Graphics.Bitmap.CreateBitmap(Bitmap, 0, 0, Bitmap.Width, Bitmap.Height, mx, true))
                {
                    flip.Compress(CompressFormat.Jpeg, _quality, ms);
                }
                ct.ThrowIfCancellationRequested();
                //}
            });
            Task.WaitAny(task, tcs.Task);
            tcs.TrySetCanceled();
            ct.ThrowIfCancellationRequested();
            if (task.IsFaulted)
                throw task.Exception;
        }
    }
}