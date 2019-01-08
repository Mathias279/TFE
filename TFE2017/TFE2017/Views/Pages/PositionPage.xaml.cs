using Plugin.Compass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace TFE2017.Core.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PositionPage : ContentPage
    {
        private List<string> _listDirections;
        private int _degres;
        PositionEntity _pos1;
        int _indexDirection;
        int _xamDegre;



        CompassTest comp;


        public PositionPage()
        {
            try
            {
                InitializeComponent();
                _listDirections = new List<string>() { "N", "NE", "E", "SE", "S", "SO", "O", "NO" };
                _degres = 0;

                CrossCompass.Current.CompassChanged += (s, e) =>
                {
                    _degres = (int)(e.Heading);// + 1;// so always > 0

                    LabelHeading.Text = _degres.ToString() + "°";
                    _indexDirection = _degres / 45;// (_degres + 22) / 45;
                    LabelDirection.Text = _listDirections[_indexDirection];
                };

                CrossCompass.Current.Start();




                comp = new CompassTest();

                comp.ToggleCompass();

                Compass.ReadingChanged += (e) => ReadingChanged(e);
                
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }
        }
        
        public void ReadingChanged(CompassChangedEventArgs e)
            {
            var data = e.Reading;
            //Console.WriteLine($"Reading: {data.HeadingMagneticNorth} degrees");
            //Heading = data.HeadingMagneticNorth;
            // Process Heading Magnetic North
            LabelDirectionXam.Text = data.HeadingMagneticNorth.ToString();
            _xamDegre = (int)Math.Round(data.HeadingMagneticNorth);
        }
        public void ComparerClicked(object sender, EventArgs e)
        {
            int un = _degres;
            int deux = _xamDegre;

            DisplayAlert("pareil?", $"cross = {un} xam = {deux} diff = {Math.Abs(un - deux)}","ok");
        }
        public void SpeedClicked(object sender, EventArgs e)
        {
            switch (comp.Speed)
            {
                case SensorSpeed.Fastest:
                    comp.Speed = SensorSpeed.Game;
                    break;
                case SensorSpeed.Game:
                    comp.Speed = SensorSpeed.Ui;
                    break;
                case SensorSpeed.Ui:
                    comp.Speed = SensorSpeed.Normal;
                    break;
                case SensorSpeed.Normal:
                    comp.Speed = SensorSpeed.Fastest;
                    break;
                default:
                    break;
            }
        }
    }
    public class CompassTest
    {

        public SensorSpeed Speed { get; set; }
        //public double Heading { get; set; }

        public CompassTest()
        {
            Speed = SensorSpeed.Ui;
            // Register for reading changes, be sure to unsubscribe when finished
            Compass.ReadingChanged += Compass_ReadingChanged;
        }

        void Compass_ReadingChanged(CompassChangedEventArgs e)
        {
            var data = e.Reading;
            //Console.WriteLine($"Reading: {data.HeadingMagneticNorth} degrees");
            
            // Process Heading Magnetic North
        }

        public string GetAzimut(CompassChangedEventArgs e)
        {
            return e.Reading.ToString();
        }

        public void ToggleCompass()
        {
            try
            {
                if (Compass.IsMonitoring)
                    Compass.Stop();
                else
                    Compass.Start(Speed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Some other exception has occured
            }
        }
    }
}