using System;
using System.Collections.Generic;
using System.Text;

using Sunrise.Generic;
using Sunrise.CelestialObjects;

namespace Sunrise.Astronomy.Sunrise
{
    public class Observer
    {
        public Geocentric GeocentricCoordinates { get; set; }
    }

    public class Observee
    {
        public CelestialObject CelestialObject { get; set; }
    }

    public class Sunrise
    {
        public Observer Observer { get; set; }
        public Observer Observee { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }

        public void Show()
        {
            
        }
    }
}
