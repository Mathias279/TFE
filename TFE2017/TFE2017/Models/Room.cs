using System;
using System.Collections.Generic;
using System.Text;
using TFE2017.Core.Models.Abstract;

namespace TFE2017.Core.Models
{
    class Room : IPlaceEntity
    {
        public Room(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }


    }
}