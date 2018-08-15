﻿using Acr.UserDialogs;
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
        private string _destinationName;
        private ListView _list;
        private List<Room> _rooms;

        public DestinationPage(string query)
        {
            try
            {
                InitializeComponent();

                _query = query;

                DecodeQuery();
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            if (_list is null)
            {
                 await InitList();
            }

        }

        private void DecodeQuery()
        {
            string[] fields = _query.Split('?')[1].Split('&');


            if (fields.Count() == 3 &&
                fields[0] == Constants.UrlField0 && fields[1].Split('=')[0] == Constants.UrlField1Key && fields[2].Split('=')[0] == Constants.UrlField2Key)
            {
                _buildingId = fields[1].Split('=')[1];
                _entryId = fields[2].Split('=')[1];
            }
        }

        private async Task<bool> InitList()
        {
            try
            {
                //var dbMan = new DataBaseManager();

                _list = new ListView();

                _rooms = await DataBaseManager.GetAllRooms(_buildingId);

                if (_rooms.Any())
                {                
                    _list.ItemsSource = _rooms.Select(room => room.Name);
                    _list.ItemSelected += DestinationSelected;
                }
                Device.BeginInvokeOnMainThread(() => Container.Children.Add(_list));
                return true;
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
            _destinationName = _rooms.FirstOrDefault(room => room.Name == _list.SelectedItem.ToString()).Id;
            //Device.BeginInvokeOnMainThread(() => DisplayAlert(_query, "building = " + _buildingId + "\n depart = " + _entryId + "\n desination = " + _destinationName, "ok"));
            ButtonSuivant.IsEnabled = true;
        }

        public async void ButtonSuivantCicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OnTheWayPage(_buildingId, _entryId, _destinationName));
        }
    }
}