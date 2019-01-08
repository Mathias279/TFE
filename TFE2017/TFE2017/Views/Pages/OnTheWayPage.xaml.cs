using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFE2017.Core.Managers;
using TFE2017.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using Xamarin.Essentials;
using TFE2017.Core.Models.Abstract;
using System.Diagnostics;

namespace TFE2017.Core.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnTheWayPage : ContentPage
    {
        private ZXingScannerView _scanView;
        private List<IPlaceEntity> _path;
        private double _buildingAngle;
        private double _stepDirection;
        private int _currentStep;
        private int _totalSteps;
        private bool _rotating;
        private double _rotation;

        public OnTheWayPage(string buidingId, string departId, string destinationId, bool useStairs, bool useLift)
        {
            try
            {
                InitializeComponent();

                InitVisual();


                Building building = Task.Run(() => DataBaseManager.GetBuilding(buidingId)).Result;

                double.TryParse(building.Angle, out _buildingAngle);

                _rotating = false;
                _rotation = 0;
                _stepDirection = 0;
                _currentStep = 0;
                _totalSteps = 0;
                _path = DataBaseManager.GetPath(buidingId, departId, destinationId, useStairs, useLift);

                if (_path.Count > 1)
                {
                    _totalSteps = (_path.Count - 1) / 2;
                    ButtonNext.IsEnabled = true;
                    ButtonNextClicked(null, null);
                }
                else
                {
                    DisplayAlert("erreur", "itineraire vide", "ok");
                    ButtonNext.IsEnabled = false;
                }

                Compass.Start(SensorSpeed.Ui);
                Compass.ReadingChanged += RotateFleche;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
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
            Compass.ReadingChanged -= RotateFleche;
            try
            {
                double north = e.Reading.HeadingMagneticNorth;
                double oldRotation = _rotation;
                _rotation = Math.Round((_stepDirection + _buildingAngle - north));                 
                Fleche.RotateTo(_rotation, 250, null);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
            Compass.ReadingChanged += RotateFleche;
        }

        private void ButtonPrevClicked(object sender, EventArgs e)
        {

        }

        private void ButtonNextClicked(object sender, EventArgs e)
        {
            try
            {

                //ex : pth .count = 12
                // 6 doors + 6 rooms

                Door doorStepStart = new Door(new PositionEntity(0, 0, 0));
                Room roomStepStart = new Room("0", "0");
                Door doorStepEnd = new Door(new PositionEntity(0, 0, 0));
                Room roomStepEnd = new Room("0", "0");

                if (_currentStep < _totalSteps)// ex: 0 - 5 < 6 (=12/2)
                {
                    doorStepStart = (Door)_path[_currentStep * 2];
                    roomStepStart = (Room)_path[(_currentStep * 2) + 1];
                    doorStepEnd = (Door)_path[(_currentStep + 1) * 2];
                    roomStepEnd = (Room)_path[((_currentStep + 1) * 2) + 1];
                    LabelTitre.Text = $"Votre position est: {roomStepStart.Name}.\nIl reste {_totalSteps - _currentStep} étapes.\nVeuillez suivre la flèche.";

                    _currentStep++;

                    LabelNext.Text = roomStepEnd.Name;
                    _stepDirection = MapManager.GetDirection(doorStepStart.Position, doorStepEnd.Position);
                }
                if (_currentStep == _totalSteps) //ex : current = total => last
                {
                    doorStepEnd = (Door)_path[_currentStep * 2];
                    roomStepEnd = (Room)_path[(_currentStep * 2) + 1];
                    LabelTitre.Text = $"Votre position est: {roomStepStart.Name}.\nDernière étape.\nVeuillez suivre la flèche.";
                    LabelNext.Text = "Terminus : " + roomStepEnd.Name;
                    _stepDirection = MapManager.GetDirection(doorStepStart.Position, doorStepEnd.Position);
                    ButtonNext.IsEnabled = false;
                }
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