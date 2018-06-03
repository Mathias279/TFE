using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core;
using Xamarin.Forms;

namespace TFE2017
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();



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
        public async void ButtonScanPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QrCodePage());
        }

        public async void ButtonPositionPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PositionPage());
        }
        
    

    }
}
