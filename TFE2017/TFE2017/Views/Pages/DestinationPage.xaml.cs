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
        private List<Room> _roomsFull;
        private List<Room> _roomsShown;

        public DestinationPage(string query, bool useStairs, bool useLift)
        {
            try
            {
                InitializeComponent();

                LabelDestination.Text = "Selectionnez votre destination";
                ButtonSuivant.Text = "Commencer la navigation";
                EntryRecherche.IsSpellCheckEnabled = false;
                EntryRecherche.IsTextPredictionEnabled = false;
                EntryRecherche.Placeholder = "Filtrer les resultats";
                EntryRecherche.TextChanged += FilterList;

                _query = query;
                _useStairs = useStairs;
                _useLift = useLift;

                if (IsQRValid())
                {
                    if (_list is null)
                    {
                        Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading());
                        Task.Run(async () =>
                        {
                            if (await InitList())
                                Device.BeginInvokeOnMainThread(() => Container.Children.Add(_list));
                            else
                                Device.BeginInvokeOnMainThread(() => DisplayAlert("Erreur", "Une erreur est survenue lors de l'appel a la base de données", "ok"));
                            Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.HideLoading());
                        });
                    }
                }
                else
                {
                    DisplayAlert("Erreur", "QR code invalide !", "ok");
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }

        private async void FilterList(object sender, TextChangedEventArgs e)
        {
            var entry = (Xamarin.Forms.Entry)sender;
            List<Room> tempRooms;

            if (_roomsFull.Any())
            {
                if (!(string.IsNullOrWhiteSpace(entry.Text)))
                {

                    tempRooms = _roomsFull.Where(room => room.Name.ToLower().Contains(entry.Text.ToLower())).ToList();

                }
                else
                    tempRooms = _roomsFull;

                if (tempRooms != _roomsShown)
                {
                    _roomsShown = tempRooms;
                    ListView filteredList = new ListView();
                    filteredList.ItemsSource = tempRooms.Select(room => room.Name);
                    filteredList.ItemSelected += DestinationSelected;

                    Container.Children.Clear();
                    Container.Children.Add(filteredList);

                }
            }
        }

        private bool IsQRValid()
        {
            string baseUrlOk = "http://onelink.to/intramuros";

            string[] halfUrl = _query.Split('?');

            if (halfUrl.Count() != 2)
                return false;

            string baseUrlQuery = halfUrl[0];
            string[] fields = halfUrl[1].Split('&');


            if (baseUrlQuery == baseUrlOk &&
                fields.Count() == 2 &&
                //fields[0] == Constants.UrlField0 && 
                fields[0].Split('=')[0] == Constants.UrlField1Key &&
                fields[1].Split('=')[0] == Constants.UrlField2Key)
            {
                _buildingId = fields[0].Split('=')[1];
                _entryId = fields[1].Split('=')[1];
                return true;
            }
            else
            {
                //DisplayAlert("Erreur", "erreur dans les parametres du qr code", "ok");
                return false;
            }
        }

        private async Task<bool> InitList()
        {
            try
            {
                _list = new ListView();
                _roomsFull = DataBaseManager.GetAllRooms(_buildingId);

                if (_roomsFull.Any())
                {
                    _roomsShown = _roomsFull;
                    _list.ItemsSource = _roomsFull.Select(room => room.Name);
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

            ListView listShown = (ListView)sender;
            string selectedName = listShown.SelectedItem.ToString();
            Room selectedRoom = _roomsShown.FirstOrDefault(room => room.Name == selectedName);
            if(!(selectedRoom is null)) {
                _destinationId = selectedRoom.Id;
                ButtonSuivant.IsEnabled = true;
            }
        }

        public async void ButtonSuivantCicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OnTheWayPage(_buildingId, _entryId, _destinationId, _useStairs, _useLift));
        }
    }
}