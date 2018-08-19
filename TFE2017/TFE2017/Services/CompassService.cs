using Xamarin.Essentials;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Services
{
    public class CompassService
    {

        public SensorSpeed Speed { get; set; }
        //public double Heading { get; set; }

        public CompassService()
        {
            Speed = SensorSpeed.Ui;
            // Register for reading changes, be sure to unsubscribe when finished
            Compass.ReadingChanged += Compass_ReadingChanged;
        }

        public void Compass_ReadingChanged(CompassChangedEventArgs e)
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
