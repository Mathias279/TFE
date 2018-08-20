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
        private int _step;
        private bool _rotating;

        public OnTheWayPage(string buidingId, string departId, string destinationId, bool useStairs, bool useLift)
        {
            try
            {
                InitializeComponent();

                InitVisual();


                Building building = Task.Run(() => DataBaseManager.GetBuilding(buidingId)).Result;

                if (!(building is null) && double.TryParse(building.Angle, out _buildingAngle)) { }
                else
                    _buildingAngle = 0;

                _rotating = false;
                _stepDirection = 0;
                _step = 0;
                _path = Task.Run(() => DataBaseManager.GetPath(buidingId, departId, destinationId, useStairs, useLift)).Result;

                if (_path.Count > 1)
                {
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
            try
            {
                Compass.ReadingChanged -= RotateFleche;
                if (_rotating)
                {
                    Task.Delay(100);
                    _rotating = false;
                }
                else
                {
                    double north = e.Reading.HeadingMagneticNorth;
                    double rotation = (_stepDirection + _buildingAngle - north + 360) % 360;
                    _rotating = true;
                    Fleche.RotateTo(rotation, 250, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
            finally
            {
                Compass.ReadingChanged += RotateFleche;
            }
        }

        private void ButtonPrevClicked(object sender, EventArgs e)
        {

        }

        private void ButtonNextClicked(object sender, EventArgs e)
        {
            try
            {
                Door doorStart = new Door(new PositionEntity(0,0,0));
                Room roomStart = new Room("0", "0");
                Door doorEnd = new Door(new PositionEntity(0, 0, 0));
                Room roomEnd = new Room("0", "0");

                if (_step + 1 < _path.Count - 1)
                {
                    doorStart = (Door)_path[_step];
                    roomStart = (Room)_path[_step + 1];
                    doorEnd = (Door)_path[_step + 2];
                    roomEnd = (Room)_path[_step + 3];

                    _step += 2;

                    LabelNext.Text = roomEnd.Name;
                    _stepDirection = MapManager.GetDirection(doorStart.Position, doorEnd.Position);
                }
                else
                {
                    doorEnd = (Door)_path[_step];
                    roomEnd = (Room)_path[_step + 1];
                    LabelNext.Text = "Terminus : " + roomEnd.Name;
                    _stepDirection = MapManager.GetDirection(doorStart.Position, doorEnd.Position);
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