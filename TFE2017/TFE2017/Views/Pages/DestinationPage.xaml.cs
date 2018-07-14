using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core.Managers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TFE2017.Core.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DestinationPage : ContentPage
    {
        private string _query;
        public DestinationPage(string query)
        {            
            try
            {
                InitializeComponent();

                _query = query;

                var dbMan = new DataBaseManager();
                
                ListDestinations.ItemsSource = dbMan.GetAllNodesNames();

                ListDestinations.ItemTapped += (sender, e) =>
                {
                    DisplayAlert("", "source = " + _query + "  desination = " + sender.ToString(), "ok");
                    Navigation.PushAsync(new OnTheWayPage());
                };
                    //recherche par nom
                    //arborescence
                }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }
    }
}