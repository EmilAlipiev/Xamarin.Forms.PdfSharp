using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PdfSharpCore.Pdf;

[assembly: Xamarin.Forms.Dependency(typeof(PdfSharp.Demo.Droid.PdfSave))]
namespace PdfSharp.Demo.Droid
{
    public class PdfSave : IPdfSave
    {

        private Context CurrentContext =>
                   Xamarin.Essentials.Platform.CurrentActivity ?? throw new NullReferenceException("Current Activity is null, ensure that the MainActivity.cs file is configuring Xamarin.Essentials in your source code so the In App Billing can use it.");


        public void Save(PdfDocument doc, string fileName)
        {
            //  String destPath = mContext.getExternalFilesDir(null).getAbsolutePath();

            string fIlePath = System.IO.Path.Combine(GetFullPath());

            using (Java.IO.File myDir = new Java.IO.File(fIlePath))
            {
                myDir.Mkdir();

                Java.IO.File file = new Java.IO.File(myDir, fileName);
                if (file.Exists())
                {
                    file.Delete();
                }


                doc.Save(file.AbsolutePath);
                doc.Close();
                //using (var outs = new FileOutputStream(file))
                //{
                //    await outs.WriteAsync(stream.ToArray());
                //    outs.Flush();
                //    outs.Close();
                //}



                if (file.Exists())
                {
                    Android.Net.Uri path = AndroidX.Core.Content.FileProvider.GetUriForFile(CurrentContext, "com.companyname.pdfsharp.demo.provider", file);
                    //Android.Net.Uri path = Android.Net.Uri.FromFile(file);

                    string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(path.ToString());

                    string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);

                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetFlags(ActivityFlags.GrantReadUriPermission);

                    intent.SetDataAndType(path, mimeType);

                    CurrentContext.StartActivity(Intent.CreateChooser(intent, "Choose App"));
                }
            }


            //global::Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
            //    title: "Success",
            //    message: $"Your PDF generated and saved @ {path}",
            //    cancel: "OK");

        }

        public Intent DisplayPDF(Java.IO.File file)
        {
            Intent intent = new Intent(Intent.ActionView);
            //var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //var filename = Path.Combine(path, "PDF_Temp.pdf");
            Android.Net.Uri filepath = Android.Net.Uri.FromFile(file);
            intent.SetDataAndType(filepath, "application/pdf");
            intent.SetFlags(ActivityFlags.ClearTop);
            return intent;

        }

        private string GetFullPath()
        {
            string root = null;

            if (Android.OS.Environment.IsExternalStorageEmulated)
            {
                root = CurrentContext.GetExternalFilesDir(null).AbsolutePath;
            }
            else
            {
                root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            }

            string path = root + "/Test";
            return path;
        }
    }
}

