using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Managers
{
    class CLLocationManager
    { 
        lm = new CLLocationManager(); //changed the class name
... (Acurray)
... (Other Specs)

lm.LocationsUpdated += delegate(object sender, CLLocationsUpdatedEventArgs e) {
    foreach(CLLocation l in e.Locations) {
        Console.WriteLine(l.Coordinate.Latitude.ToString() + ", " +l.Coordinate.Longitude.ToString());
    }
};

lm.StartUpdatingLocation();
}
