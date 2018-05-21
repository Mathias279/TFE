using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace TFE2017.Core
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QrCodePage : ContentPage
	{
		public QrCodePage()
		{
			InitializeComponent ();


        }
        public async void ButtonScanClicked(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    LabelCode.Text = result.Text;
                });
            };

        }

    }
}