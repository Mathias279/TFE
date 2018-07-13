using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core;
using TFE2017.Core.Pages;
using Xamarin.Forms;

namespace TFE2017.Core.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();
                InitVisual();

                //async Task TryGetLocationAsync()
                //{
                //    if ((int)Build.VERSION.SdkInt < 23)
                //    {
                //        await GetLocationAsync();
                //        return;
                //    }

                //    await GetLocationPermissionAsync();
                //}        
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }

        private void InitVisual()
        {
            LabelTitre.Text = "Welcome to my app!";
            ButtonScanPage.Text = "ScanPage";
            ButtonPositionPage.Text = "PositionPage";
            ButtonDBPage.Text = "DBPage";
            ButtonSuivant.Text = "Suivant";
        }

        //public void IsDebugModeToggled(object sender, EventArgs e)
        //{
        //    if (IsDebugMode.is)
        //}


        public void IsDebugModeToggled(object sender, EventArgs e)
        {
            ContainerDebug.IsVisible = IsDebugMode.IsToggled;
            ButtonSuivant.IsVisible = !IsDebugMode.IsToggled;
        }

        public async void ButtonScanPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QrCodePage());
        }

        public async void ButtonPositionPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PositionPage());
        }

        public async void BoutonDBPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DBPage2());
        }

        public async void ButtonSuivantCicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QrCodePage());
        }
    }
}