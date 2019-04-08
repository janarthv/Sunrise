using System;

using Sunrise.Generic.Frames;
using Sunrise.CelestialObjects;

namespace Sunrise.Generic
{
    public class State
    {
        public Frame Frame { get; set; }
        public DateTime Epoch { get; set; }
        public Coordinates Coordinates { get; set; }
        public Origin Origin { get; set; }
    }
}
