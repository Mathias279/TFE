using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models.Abstract
{
    abstract class MapEntity
    {
        public abstract string Id { get; set; }
        public abstract string Name { get; set; }
    }
}