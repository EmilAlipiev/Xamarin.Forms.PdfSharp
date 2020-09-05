using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Plugin.Xamarin.Forms.PdfSharp;

using Xamarin.Essentials;
using Xamarin.Forms;
  
namespace PdfSharp.Demo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            picker.ItemsSource = new List<string>() { "Item 1", "Item 2", "Item 3" };
            picker.SelectedIndex = 0;
        }

        private async void GeneratePDF(object sender, EventArgs e)
        {


            PermissionStatus status = await GetPermissions(new Permissions.StorageWrite());

            if (status == PermissionStatus.Granted)
            {
                var pdf = PDFManager.GeneratePDFFromView(mainGrid);
                DependencyService.Get<IPdfSave>().Save(pdf, "SinglePage.pdf");
            }
            else if (status != PermissionStatus.Unknown)
            {
                await DisplayAlert("Storage Access Denied", "Try Again", "OK");
            }

        }

        public static async Task<PermissionStatus> GetPermissions<T>(T permission) where T : Permissions.BasePermission
        {

            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }
    }
}
