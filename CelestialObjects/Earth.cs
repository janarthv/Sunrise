using System;
using MathNet.Numerics.LinearAlgebra;
using Sunrise.Generic;
using Sunrise.Generic.Frames;
using Sunrise.Math;

namespace Sunrise.CelestialObjects
{
    public class Earth
    {
        public CoordinatesNeeded CoordinatesNeeded { get; set; }
        public void GetHelioCentricState(State state,Depth depth)
        {
            Coordinates coordinates = state.Coordinates;
            if (CoordinatesNeeded.Keplerian)
            {
                coordinates.KeplerianCoordinates = new KeplerianCoordinates
                {
                    Origin = Body.Sun,
                };
                //FIXME Implement logic using coordinate frame from input
            }
            if (CoordinatesNeeded.Cartesian)
            {
                state.Coordinates.CartesianCoordinates = new CartesianCoordinates
                {
                    Origin = Body.Sun,
                    Position = Vector<double>.Build.Random(3),
                };
                //FIXME Implement logic using coordinate frame from input
            }
        }

        public void GetBodyCentricState(Body body, State state)
        {
            BodyCentricCoordinates bodyCentricCoordinates = state.Coordinates.BodyCentricCoordinates;
        }

        private void GetHelioCentricKeplerianElements(DateTime epoch)
        {
            State state = new State
            {
                Epoch = new DateTime(2000, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc),
                Coordinates = new Coordinates
                {
                    KeplerianCoordinates = new KeplerianCoordinates
                    {
                        SMA = 1.00000011 * Constants.AU,
                        Ecc = 0.01671022,
                        Inc = BasicMath.DegreeToRadian(0.00005),
                        RAAN = BasicMath.DegreeToRadian(-11.26064),
                        ArgPer = BasicMath.DegreeToRadian(102.94719),
                        TA = BasicMath.DegreeToRadian(100.46435),
                        CoordinateFrame = Frame.EME2000,
                        Origin = Body.Sun,
                    },
                },
            };
            return state.Coordinates.KeplerianCoordinates;
        }
    }
}

