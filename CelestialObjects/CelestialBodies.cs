using System;
using System.Collections.Generic;

using Sunrise.Generic;
using Sunrise.Generic.Frames;

namespace Sunrise.CelestialObjects
{
    public enum Body
    {
        Sun,
        Earth,
    }

    public class CelestialBody
    {
        public Body Name { get; set; }
        public double MU { get; set; }      // Units in km3/s2
        public IEnumerable<Frame> Frames { get; set; }
    }

    public static partial class CelestialBodies
    {
        public static readonly CelestialBody Sun = new CelestialBody
        {
            Name = Body.Sun,
            MU = Constants.MU.Sun,
        };

        public static readonly CelestialBody Earth = new CelestialBody
        {
            /*
             Reference: Explanatory Supplement to the Astronomical Almanac. 1992. K. P. Seidelmann, Ed., p.316 (Table 5.8.1), University Science Books, Mill Valley, California. 
             Mean ecliptic and equinox of J2000 at the J2000 epoch (2451545.0 JD)
             [A(AU) e i(deg) omega(deg) long. of peri(deg) mean long.(deg)] 1.00000011	0.01671022 	0.00005	-11.26064	102.94719	100.46435
             */
            Name = Body.Earth,
            MU = Constants.MU.Earth,
            Frames = new List<Frame>
            {
                Frame.ECEF,
            },
            
        };
    }
}
