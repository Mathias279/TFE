using Acr.UserDialogs;
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

                _appUrl = new Uri("https://play.google.com/store/apps/details?id=com.Slack&buildingId=1&entryId=1");
                _idBuilding = string.Empty;
                _idQrCode = string.Empty;
                _textQR = string.Empty;

                //ScanAsync();
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }

        protected override void OnAppearing()
        {
            //Scan();
            base.OnAppearing();
        }

        public async Task ScanAsync()
        {
            if (!(await App.CheckPermission(Permission.Camera)))
            {
                await DisplayAlert("permission", "veuillez autoriser l'utilisations de l'appareil photo", "ok");
                //await Navigation.PopAsync(true);
            }
            else
            {
                _scanner = new ScannerPage(true);// App.CheckPermission(Permission.FLASHLIGHT).Result));
                _scanner.PropertyChanged += ScanDoneAsync;

                Device.BeginInvokeOnMainThread(() => Navigation.PushAsync(_scanner, true));
                //Device.BeginInvokeOnMainThread(() => ButtonReScan.IsVisible = true);
                ButtonReScan.IsVisible = true;
            }
        }

        private async void ScanDoneAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var qrSender = (ScannerPage)sender;
            if (qrSender.IsDone)
            {
                qrSender.PropertyChanged -= ScanDoneAsync;
                _textQR = _scanner.Result;
                //await Navigation.PopAsync();
                await CheckScanResultasync();
            }
        }

        private async Task CheckScanResultasync()
        {
            Uri uriQR = null;
            if (!string.IsNullOrWhiteSpace(_textQR) && Uri.TryCreate(_textQR, UriKind.RelativeOrAbsolute, out uriQR))
            {
                //    bool isQRValid = true;

                //    isQRValid &= _appUrl.Scheme == uriQR.Scheme;
                //    isQRValid &= _appUrl.AbsoluteUri == uriQR.AbsoluteUri;
                //    isQRValid &= _appUrl.LocalPath == uriQR.LocalPath;

                //    if (isQRValid)
                //    {
                await DisplayAlert("qr code", "le code est: " + _textQR, "ok");
                ButtonSuivant.IsEnabled = true;
                //}
                //else
                //{
                //    await DisplayAlert("erreur", "qr non valide", "cancel");
                //    await DisplayAlert("erreur", _appUrl.ToString() + "\n " + uriQR.ToString(), "cancel");
                //}
            }
            else
            {
                await DisplayAlert("erreur", "qr vide ou incorrect", "cancel");
            }
        }

        private void InitVisual()
        {
            ButtonSuivant.Text = "Suivant";
            ButtonReScan.Text = "Scan";
        }

        public async void ButtonSuivantClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();
            await Navigation.PushAsync(new DestinationPage(_appUrl.Query));
            UserDialogs.Instance.HideLoading();
        }

        public async void ButtonReScanClicked(object sender, EventArgs e)
        {
            ScanAsync();
        }
    }
}