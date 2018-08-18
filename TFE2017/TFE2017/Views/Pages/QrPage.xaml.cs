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
        private Uri _uriQR;
        //private string _idBuilding;
        //private string _idQrCode;
        internal string _textQR;
        private ScannerPage _scanner;

        public QrPage()
        {
            try
            {
                InitializeComponent();
                InitVisual();

                _appUrl = new Uri("http://onelink.to/intramuros?builingId=1&entryId=1");
                //_idBuilding = string.Empty;
                //_idQrCode = string.Empty;
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

            _appUrl = new Uri("http://onelink.to/intramuros?builingId=1&entryId=1");
            //_idBuilding = string.Empty;
            //_idQrCode = string.Empty;
            _textQR = string.Empty;
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
            Uri uriFromQR = null;
            if (!string.IsNullOrWhiteSpace(_textQR) && Uri.TryCreate(_textQR, UriKind.RelativeOrAbsolute, out uriFromQR))
            {
                //await DisplayAlert("qr code", "le code est: " + _textQR, "ok");
                if (!(uriFromQR is null))
                    _uriQR = uriFromQR;
                ButtonSuivant.IsEnabled = true;
            }
            else
            {
                await DisplayAlert("erreur", "qr vide ou incorrect", "cancel");
            }
        }

        private void InitVisual()
        {
            LabelCode.Text = "Veuillez scanner le QR code à votre disposition";
            ButtonSuivant.Text = "Suivant";
            ButtonReScan.Text = "Scanner";
        }

        public async void ButtonSuivantClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PreferencesPage(_uriQR.ToString()));
            //await Navigation.PushAsync(new DestinationPage(_uriQR.Query));
        }

        public async void ButtonReScanClicked(object sender, EventArgs e)
        {
            ScanAsync();
        }
    }
}