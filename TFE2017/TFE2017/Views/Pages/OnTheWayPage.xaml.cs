using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core.Managers;
using TFE2017.Core.Models;
using TFE2017.Core.Views.Customs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Permissions.Abstractions;
using ZXing.Net.Mobile.Forms;
using TFE2017.Core.Services;
using Xamarin.Essentials;
using TFE2017.Core.Models.Abstract;

namespace TFE2017.Core.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnTheWayPage : ContentPage
    {
        private ZXingScannerView _scanView;
        private List<IPlaceEntity> _path;
        private double _buildingAngle;
        private double _stepDirection;
        private int _step;

        public OnTheWayPage(string buidingId, string departId, string destinationId, bool useStairs, bool useLift)
        {
            InitializeComponent();

            InitVisual();

            Compass.Start(SensorSpeed.Ui);
            Compass.ReadingChanged += RotateFleche;

            Building building = Task.Run(() => DataBaseManager.GetBuilding(buidingId)).Result;

            if (double.TryParse(building.Angle, out _buildingAngle)) { }
            else
                _buildingAngle = 0;

            _stepDirection = 0;
            _step = 0;
            _path = Task.Run(() => DataBaseManager.GetPath(buidingId, departId, destinationId, useStairs, useLift)).Result;

            if (_path.Any())
            {
                ButtonNext.IsEnabled = true;
                ButtonNextClicked(null, null);
            }
            else
            {
                DisplayAlert("erreur", "itineraire vide", "ok");
                ButtonNext.IsEnabled = false;
            }

        }
        
        public void OnDisappearing()
        {
            base.OnDisappearing();
            Compass.ReadingChanged -= RotateFleche;
            Compass.Stop();
        }

        private void InitVisual()
        {
            LabelTitre.Text = "Suivez la flèche";
            ButtonNext.Text = "Etape suivante";
            //LabelNext.Text = "prochaine étape";
        }

        public void RotateFleche(CompassChangedEventArgs e)
        {
            double north = e.Reading.HeadingMagneticNorth;
            double rotation = north + _stepDirection;
            Fleche.RotateTo(rotation,0,null);
        }

        private void ButtonPrevClicked(object sender, EventArgs e)
        {

        }

        private void ButtonNextClicked(object sender, EventArgs e)
        {
            Door doorStart = (Door)_path[_step];
            Room roomStart = (Room)_path[_step+1];
            Door doorEnd = (Door)_path[_step+2];
            Room roomEnd = (Room)_path[_step+3];

            _step += 2;

            if (_step+1 < _path.Count-1)
            {
                LabelNext.Text = roomEnd.Name;
                _stepDirection = MapManager.GetDirection(doorStart.Position, doorEnd.Position);
            }
            else
            {
                LabelNext.Text = "Terminus : " + roomEnd.Name;
                _stepDirection = MapManager.GetDirection(doorStart.Position, doorEnd.Position);
                ButtonNext.IsEnabled = false;
            }


        }
    }
}