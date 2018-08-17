using Acr.UserDialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core.Managers;
using TFE2017.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TFE2017.Core.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DestinationPage : ContentPage
    {
        private string _query;
        private string _buildingId;
        private string _entryId;
        private string _stairs;
        private string _destinationId;

        private bool _useStairs;
        private bool _useLift;

        private ListView _list;
        private List<Room> _rooms;

        public DestinationPage(string query, bool useStairs, bool useLift)
        {
            try
            {
                InitializeComponent();

                _query = query;
                _useStairs = useStairs;
                _useLift = useLift;

                DecodeQuery();

                if (_list is null)
                {
                    Task.Run(async () =>
                    {
                        if (await InitList())
                            Device.BeginInvokeOnMainThread(() => Container.Children.Add(_list));
                        else
                            await DisplayAlert("Erreur", "Une erreur est survenue lors de l'appel a la base de données", "ok");

                    });
                }

            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }
        

        private void DecodeQuery()
        {
            string[] fields = _query.Split('?')[1].Split('&');


            if (fields.Count() == 3 &&
                fields[0] == Constants.UrlField0 && 
                fields[1].Split('=')[0] == Constants.UrlField1Key && 
                fields[2].Split('=')[0] == Constants.UrlField2Key)
            {
                _buildingId = fields[1].Split('=')[1];
                _entryId = fields[2].Split('=')[1];
            }
            else
                DisplayAlert("Erreur", "erreur dans les parametres du qr code", "ok");
        }

        private async Task<bool> InitList()
        {
            try
            {
                _list = new ListView();
                _rooms = await DataBaseManager.GetAllRooms(_buildingId);

                if (_rooms.Any())
                {                
                    _list.ItemsSource = _rooms.Select(room => room.Name);
                    _list.ItemSelected += DestinationSelected;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                return false;
            }
        }

        private void DestinationSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _list.IsEnabled = false;
            _destinationId = _rooms.FirstOrDefault(room => room.Name == _list.SelectedItem.ToString()).Id;
            ButtonSuivant.IsEnabled = true;
        }

        public async void ButtonSuivantCicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OnTheWayPage(_buildingId, _entryId, _destinationId, _useStairs, _useLift));
        }
    }
}