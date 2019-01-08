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
        public string Angle { get; private set; }

        public Building()
        {
            Id = "";
            Name = "";
            Angle = "";
        }

        public Building(string id, string name, string angle)
        {
            Id = id;
            Name = name;
            Angle = angle;
        }
    }
}