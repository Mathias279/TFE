using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;


namespace TFE2017.Core.Views.Customs
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScannerPage : ContentPage
	{
        private bool _isScanning;
        private ZXingScannerView _scanView;
        private ZXingDefaultOverlay _scanOverlay;
        private MobileBarcodeScanningOptions _scanOptions;
            
        public string Result { get; internal set; }
        public bool IsDone { get; internal set; }

        public ScannerPage(bool isScanning)
        {
            InitializeComponent();

            Result = null;
            IsDone = false;

            _isScanning = isScanning;//WithTorch;

            _scanOptions = MakeOptions();
            _scanView = MakeView();
            _scanOverlay = MakeOverlay();
            if (false)
                _scanOverlay.FlashButtonClicked += (sender, e) => FlashButtonClickedCustom();

            MainGrid.Children.Add(_scanView);
            //MainGrid.Children.Add(_scanOverlay);
        }

        private MobileBarcodeScanningOptions MakeOptions()
        {
            MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions
            {
                InitialDelayBeforeAnalyzingFrames = 100,
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                TryHarder = true,
                DisableAutofocus = false,
                
                //PossibleFormats = new List<ZXing.BarcodeFormat>
                //{
                //ZXing.BarcodeFormat.Ean8, ZXing.BarcodeFormat.Ean13
                //}
            };
            return options;
        }

        private ZXingScannerView MakeView()
        {
            ZXingScannerView view = new ZXingScannerView()
            {
                IsScanning = _isScanning,
                IsAnalyzing = true
            };
            view.OnScanResult += async (result) => await ScannedAsync(result);
            return view;
        }

        private ZXingDefaultOverlay MakeOverlay()
        {
            ZXingDefaultOverlay overlay = new ZXingDefaultOverlay()
            {
                TopText = string.Empty,
                BottomText = string.Empty,
                IsClippedToBounds = true,
                Opacity = 0,
            };
            if (false)
                overlay.ShowFlashButton = true;
            return overlay;
        }

        private async Task ScannedAsync(ZXing.Result result)
        {
            if (!(string.IsNullOrWhiteSpace(result.ToString())))
            {
                Result = result.ToString();
                IsDone = true;
                _scanView.IsScanning = false;

                Device.BeginInvokeOnMainThread(() => Navigation.PopAsync());
            }
        }

        private void FlashButtonClickedCustom()
        {
            _scanView.IsTorchOn = !_scanView.IsTorchOn;
        }
	}
}