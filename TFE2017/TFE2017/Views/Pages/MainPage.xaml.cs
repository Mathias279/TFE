using Acr.UserDialogs;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core;
using TFE2017.Core.Views.Pages;
using Xamarin.Forms;

namespace TFE2017.Core.Views.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();
                InitVisual();

#if DEBUG
                IsDebugMode.IsVisible = true;
#endif
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
            ButtonDestinationPage.Text = "DestinationPage";
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
            await Navigation.PushAsync(new QrPage());
        }

        public async void ButtonPositionPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PositionPage());
        }

        public async void BoutonDestinationPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DestinationPage("https://play.google.com/store/apps/details?id=com.Slack&buildingId=1&entryId=1"));
        }

        public async void ButtonSuivantCicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QrPage());
        }
    }
}