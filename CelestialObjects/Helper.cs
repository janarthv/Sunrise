using System;
using MathNet.Numerics.LinearAlgebra;
using Sunrise.Generic;

namespace Sunrise.CelestialObjects
{
    public enum Depth
    {
        Position,
        Velocity,
        Acceleration,
    }

    public class CoordinatesNeeded
    {
        public bool Keplerian { get; set; }
        public bool Cartesian { get; set; }
        public bool BodyCentric { get; set; }
        public bool TopoCentric { get; set; }
    }

    public class StateRetriever
    {
        public CoordinatesNeeded CoordinatesNeeded { get; set; }
        public void GetState(Body body, State state, Depth depth)
        {
            state.CheckValidity();
            Coordinates stateCoordinates = state.Coordinates;
            if (body == Body.Sun)
            {
                if (CoordinatesNeeded.Keplerian)
                {
                    throw new NotImplementedException();
                }
                if (CoordinatesNeeded.Cartesian)
                {
                    CartesianCoordinates cartesianCoordinates = stateCoordinates.CartesianCoordinates;
                    if (cartesianCoordinates == null || cartesianCoordinates.Origin == null || cartesianCoordinates.CoordinateFrame == null)
                    {
                        throw new ArgumentNullException();
                    }
                    Body origin = cartesianCoordinates.Origin.Value;
                    if (origin != Body.Sun)
                    {
                        State intState = new State
                        {
                            Epoch = state.Epoch,
                            Coordinates = new Coordinates
                            {
                                CartesianCoordinates = new CartesianCoordinates
                                {
                                    CoordinateFrame = cartesianCoordinates.CoordinateFrame,
                                }
                            }
                        };
                        StateRetriever stateRetriever = new StateRetriever
                        {
                            CoordinatesNeeded = new CoordinatesNeeded
                            {
                                Cartesian = true,
                            }
                        };
                        stateRetriever.GetHelioCentricState(origin, intState, depth);
                        intState.Coordinates.CartesianCoordinates.Negative(); //BETTERME
                        cartesianCoordinates.Position = intState.Coordinates.CartesianCoordinates.Position;
                        if (depth == Depth.Velocity)
                        {
                            cartesianCoordinates.Velocity = intState.Coordinates.CartesianCoordinates.Velocity;
                        }
                    }
                }
                if (stateCoordinates.IsBodyCentricCoordinatesNeeded)
                {
                    BodyCentricCoordinates bodyCentricCoordinates = stateCoordinates.BodyCentricCoordinates;
                    if (bodyCentricCoordinates == null || bodyCentricCoordinates.CentreBody == null || bodyCentricCoordinates.BodyFrame == null)
                    {
                        throw new ArgumentNullException();
                    }
                    Body centreBody = bodyCentricCoordinates.CentreBody.Value;
                    if (centreBody != Body.Sun)
                    {
                        GetBodyCentricState(centreBody, body, state);
                    }
                }
            }
        }

        public void GetHelioCentricState(Body body, State state, Depth depth)
        {
            if (body == Body.Sun)
            {
                throw new InvalidOperationException();
            }
            else if (body == Body.Earth)
            {
                Earth.GetHelioCentricState(state, depth);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void GetBodyCentricState(Body centreBody, Body body, State state)
        {
            if (centreBody == Body.Earth)
            {
                Earth.GetBodyCentricState(body, state);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
