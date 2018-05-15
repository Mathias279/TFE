using System;
using TFE2017.Core;
using Xamarin.Forms;

namespace TFE2017
{
    public partial class MainPage : ContentPage
    {
        string translatedNumber;

        public MainPage()
		{
            InitializeComponent();

            if (App.PhoneNumbers.Count == 0 )
            {
                callHistoryButton.IsEnabled = false;
            }
        }

        void OnTranslate(object sender, EventArgs e)
        {
            translatedNumber = PhonewordTranslator.ToNumber(phoneNumberText.Text);
            if (!string.IsNullOrWhiteSpace(translatedNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = "Call " + translatedNumber;
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Call";
            }
        }

        async void OnCall(object sender, EventArgs e)
        {
            if (await this.DisplayAlert(
                    "Dial a Number",
                    "Would you like to call " + translatedNumber + "?",
                    "Yes",
                    "No"))
            {
                var dialer = DependencyService.Get<IDialer>();
                if (dialer != null)
                {
                    App.PhoneNumbers.Add(translatedNumber);
                    callHistoryButton.IsEnabled = true;
                    dialer.Dial(translatedNumber);
                }
            }
        }
        async void OnCallHistory(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CallHistoryPage());
            InitializeComponent();
        }        
    }
}
