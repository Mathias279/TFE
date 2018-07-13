using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TFE2017.Core.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace TFE2017.Core
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QrPage : ContentPage
	{
        private Uri _appUrl;
        private string _idBuilding;
        private string _idQrCode;
        internal string _textQR;

        public QrPage()
		{
            try
            {
                InitializeComponent();
                InitVisual();
                Init();

                Scan();
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }

        private void Init()
        {
            _appUrl = new Uri("https://play.google.com/store/apps/details?id=com.Slack&builingId=1&entryId=1");
            _idBuilding = "";
            _idQrCode = "";
            _textQR = "";
        }

        public void Scan()
        {
            if (!App.CheckPermission(Permission.Camera).Result)
            {
                DisplayAlert("permission", "veuillez autoriser l'utilisations de l'appareil photo", "ok");
            }
            else
            {
                ZXingScannerView scanView = new ZXingScannerView()
                {
                    IsScanning = true,
                    IsAnalyzing = true,
                    //Options = new ZXing.Mobile.MobileBarcodeScanningOptions() { DelayBetweenContinuousScans = 1000},
                };


                scanView.OnScanResult += ((result) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    //Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await Navigation.PopAsync();

                        _textQR = result.Text;

                        CheckScanResult();
                    });
                });
            }
        }

        private async void CheckScanResult()
        {
            if (!string.IsNullOrWhiteSpace(_textQR))
            {
                Uri uriQR = new Uri(_textQR);
                bool isQRValid = true;

                isQRValid &= _appUrl.Scheme == uriQR.Scheme;
                isQRValid &= _appUrl.AbsoluteUri == uriQR.AbsoluteUri;
                isQRValid &= _appUrl.LocalPath == uriQR.LocalPath;
                
                if (isQRValid)                
                    await Navigation.PushAsync(new DestinationPage(_appUrl.Query));
                else
                {
                    await DisplayAlert("erreur", "qr non valide", "cancel");
                    await DisplayAlert("erreur", _appUrl.ToString() + "\n " + uriQR.ToString(), "cancel");
                }
            }
            else
            {
                await DisplayAlert("erreur", "scan non valide", "cancel");
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