using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Sunrise.CelestialObjects;

namespace Sunrise.Generic.States
{
    public enum Depth
    {
        Position,
        Velocity,
        Acceleration,
    }

    public partial class StateRetriever
    {
        public Body? Body { get; set; }
        public State State { get; set; }
        public CoordinatesNeeded CoordinatesNeeded { get; set; }

        public void GetState(Depth depth)
        {
            if (Body == null || CoordinatesNeeded == null || State == null)
            {
                throw new ArgumentNullException();
            }
            State.CheckValidity();
            if (Body == CelestialObjects.Body.Sun)
            {
                if (CoordinatesNeeded.Keplerian)
                {
                    throw new NotImplementedException();
                }
                if (CoordinatesNeeded.Cartesian)
                {
                    foreach (Coordinates stateCoordinates in State.CoordinatesSet)
                    {
                        if (stateCoordinates.CoordinatesNeeded == null || stateCoordinates.CoordinatesNeeded.Cartesian)
                        {
                            foreach (CartesianCoordinates cartesianCoordinates in stateCoordinates.CartesianCoordinates)
                            {
                                if (cartesianCoordinates == null || cartesianCoordinates.Origin == null || cartesianCoordinates.CoordinateFrame == null)
                                {
                                    throw new ArgumentNullException();
                                }
                                Body origin = cartesianCoordinates.Origin.Value;
                                if (origin != CelestialObjects.Body.Sun)
                                {
                                    CartesianCoordinates dummyCartesianCoordinates = new CartesianCoordinates
                                    {
                                        Depth = cartesianCoordinates.Depth,
                                        CoordinateFrame = cartesianCoordinates.CoordinateFrame,
                                    };
                                    Coordinates dummyCoordinates = new Coordinates
                                    {
                                        CoordinatesNeeded = new CoordinatesNeeded
                                        {
                                            Cartesian = true,                                            
                                        },
                                        KeplerianCoordinates = stateCoordinates.KeplerianCoordinates,
                                        CartesianCoordinates = new List<CartesianCoordinates>
                                        {
                                            dummyCartesianCoordinates,
                                        }
                                    };
                                    GetHelioCentricState(origin, State.Epoch, dummyCoordinates);
                                    dummyCartesianCoordinates.Negative(); //BETTERME
                                    cartesianCoordinates.Position = dummyCartesianCoordinates.Position;
                                    cartesianCoordinates.Velocity = dummyCartesianCoordinates.Velocity;
                                }
                                else
                                {
                                    cartesianCoordinates.Position = Vector<double>.Build.Dense(3);
                                    if (cartesianCoordinates.Depth == CartesianDepth.Velocity)
                                    {
                                        cartesianCoordinates.Velocity = Vector<double>.Build.Dense(3);
                                    }
                                }
                            }
                        }
                    }                
                }
                if (CoordinatesNeeded.BodyCentric)
                {

                }
            }
        }
    }
}
