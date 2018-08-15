using System;
using System.Collections.Generic;
using System.Text;
using TFE2017.Core.Models.Abstract;
using Xamarin.Essentials;


namespace TFE2017.Core.Models
{
    class Building:IPlaceEntity
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public double Angle { get; private set; }
    }
}