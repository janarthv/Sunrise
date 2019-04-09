using MathNet.Numerics.LinearAlgebra;
using Sunrise.Generic;

namespace Sunrise.CelestialObjects
{
    public static class Earth
    {
        public static void GetHelioCentricState(State state,Depth depth)
        {
            Coordinates coordinates = state.Coordinates;
            if (coordinates.IsKeplerianCoordinatesNeeded )
            {
                coordinates.KeplerianCoordinates = new KeplerianCoordinates
                {
                    Origin = Body.Sun,
                };
                //FIXME Implement logic using coordinate frame from input
            }
            if (state.Coordinates.IsCartesianCoordinatesNeeded)
            {
                state.Coordinates.CartesianCoordinates = new CartesianCoordinates
                {
                    Origin = Body.Sun,
                    Position = Vector<double>.Build.Random(3),
                };
                //FIXME Implement logic using coordinate frame from input
            }
        }

        internal static void GetBodyCentricState(Body body, State state)
        {
            BodyCentricCoordinates bodyCentricCoordinates = state.Coordinates.BodyCentricCoordinates;
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
