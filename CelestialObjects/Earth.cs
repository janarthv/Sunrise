using System;
using Sunrise.Generic;

namespace Sunrise.CelestialObjects
{
    public static class Earth
    {
        public static void GetHelioCentricState(State state)
        {
            throw new NotImplementedException();
        }                
    }
}

/*
 *             State = new State
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
 */
