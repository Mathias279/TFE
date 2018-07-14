using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TFE2017.Core.Views.Customs;
using TFE2017.Core.Views.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace TFE2017.Core.Views.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QrPage : ContentPage
	{
        private Uri _appUrl;
        private string _idBuilding;
        private string _idQrCode;
        internal string _textQR;
        private ScannerPage _scanner;

        public QrPage()
		{
            try
            {
                InitializeComponent();
                InitVisual();

                _appUrl = new Uri("https://play.google.com/store/apps/details?id=com.Slack&builingId=1&entryId=1");
                _idBuilding = "";
                _idQrCode = "";
                _textQR = "";

                Scan();
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }
        
        public async Task Scan()
        {
            if (!(await App.CheckPermission(Permission.Camera)))
            {
                Device.BeginInvokeOnMainThread(async () => await DisplayAlert("permission", "veuillez autoriser l'utilisations de l'appareil photo", "ok"));
                Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync(true));
            }
            else
            {
                _scanner = new ScannerPage(true);// App.CheckPermission(Permission.FLASHLIGHT).Result));
                _scanner.PropertyChanged += ScanDone;

                Device.BeginInvokeOnMainThread(() => Navigation.PushAsync(_scanner, true));


                //ZXingScannerView scanView = new ZXingScannerView()
                //{
                //    IsScanning = true,
                //    IsAnalyzing = true,
                //    //Options = new ZXing.Mobile.MobileBarcodeScanningOptions() { DelayBetweenContinuousScans = 1000},
                //};

                //scanView.OnScanResult += ((result) =>
                //{
                //    Device.BeginInvokeOnMainThread(() =>
                //    //Device.BeginInvokeOnMainThread(async () =>
                //    {
                //        //await Navigation.PopAsync();

                //        _textQR = result.Text;

                //        CheckScanResult();
                //    });
                //});
            }
        }

        private async void ScanDone(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var qrSender = (ScannerPage)sender;
            if (qrSender.IsDone)
            {
                qrSender.PropertyChanged -= ScanDone;
                _textQR = _scanner.Result;
                //await Navigation.PopAsync();
                CheckScanResult();
            }
         }

        private void CheckScanResult()
        {
            if (!string.IsNullOrWhiteSpace(_textQR))
            {
                Uri uriQR = new Uri(_textQR);
                bool isQRValid = true;

                isQRValid &= _appUrl.Scheme == uriQR.Scheme;
                isQRValid &= _appUrl.AbsoluteUri == uriQR.AbsoluteUri;
                isQRValid &= _appUrl.LocalPath == uriQR.LocalPath;
                
                if (isQRValid)                
                    Navigation.PushAsync(new DestinationPage(_appUrl.Query));
                else
                {
                    DisplayAlert("erreur", "qr non valide", "cancel");
                    DisplayAlert("erreur", _appUrl.ToString() + "\n " + uriQR.ToString(), "cancel");
                }
            }
            else
            {
                DisplayAlert("erreur", "scan non valide", "cancel");
            }
        }

        private void InitVisual()
        {
            ButtonSuivant.Text = "Suivant";
        }

        public async void ButtonSuivantCicked(object sender, EventArgs e)
        {
            await DisplayAlert("qr code", "le code est: " + _textQR, "ok", null);
        }
    }
}