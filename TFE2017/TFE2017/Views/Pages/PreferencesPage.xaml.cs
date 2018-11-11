﻿using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TFE2017.Core.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreferencesPage : ContentPage
    {
        private string _query;

        public PreferencesPage(string query)
        {
            InitializeComponent();

            _query = query;


            LabelTitre.Text = "Veuillez entrer vos préférences";
            LabelStrairs.Text = "Emprunter les escaliers?";
            ToggleStairs.IsToggled = true;
            LabelLift.Text = "Emprunter les ascenceurs?";
            ToggleLift.IsToggled = true;
            ButtonSuivant.Text = "Suivant";
        }

        public void StairsToggled(object sender, EventArgs e)
        {
            Switch toggle = (Switch)sender;
            if (!(toggle.IsToggled))
                ToggleLift.IsToggled = true;
        }

        public void LiftToggled(object sender, EventArgs e)
        {
            Switch toggle = (Switch)sender;
            if (!(toggle.IsToggled))
                ToggleStairs.IsToggled = true;
        }

        public async void ButtonSuivantCicked(object sender, EventArgs e)
        {
            if (!(ToggleStairs.IsToggled) && !(ToggleLift.IsToggled))
                await DisplayAlert("Attention", "Veuillez accepter au moins une des deux options", "ok");
            else
            {
                await Navigation.PushAsync(new DestinationPage(_query, ToggleStairs.IsToggled, ToggleLift.IsToggled));
            }
        }

    }
}