using System;
using System.Collections.Generic;
using System.Text;

using Sunrise.Generic;
using Sunrise.Generic.Frames;
using Sunrise.Math;
using Type = Sunrise.Generic.Type;

namespace Sunrise.CelestialObjects
{
    public class CelestialObject
    {
        public double MU { get; set; }      // Units in km3/s2
        public State State { get; set; }
    }

    public enum Origin
    {
        Sun,
        Earth,
    }

    public static class CelestialObjects
    {
        public static readonly CelestialObject Sun = new CelestialObject
        {
            MU = Constants.MU.Sun,
            State = new State
            {
                Epoch = new DateTime(),
                Frame = Frame.EME2000,
                Coordinates = new Coordinates(),
                Origin = Origin.Sun,
            },
        };

        public static readonly CelestialObject Earth = new CelestialObject
        {
            /*
             Reference: Explanatory Supplement to the Astronomical Almanac. 1992. K. P. Seidelmann, Ed., p.316 (Table 5.8.1), University Science Books, Mill Valley, California. 
             Mean ecliptic and equinox of J2000 at the J2000 epoch (2451545.0 JD)
             [A(AU) e i(deg) omega(deg) long. of peri(deg) mean long.(deg)] 1.00000011	0.01671022 	0.00005	-11.26064	102.94719	100.46435
             */
            MU = Constants.MU.Earth,
            State = new State
            {
                Epoch = new DateTime(),
                Frame = Frame.EME2000,
                Coordinates = new Coordinates
                {
                    Type = Type.Keplerian,
                    Keplerian = new Keplerian
                    {
                        SMA = 1.00000011 * Constants.AU,
                        Ecc = 0.01671022,
                        Inc = BasicMath.DegreeToRadian(0.00005),
                        RAAN = BasicMath.DegreeToRadian(-11.26064),
                        ArgPer = BasicMath.DegreeToRadian(102.94719),
                        TA = BasicMath.DegreeToRadian(100.46435),
                    },
                },
                Origin = Origin.Sun,
            },
        };
    }
}
