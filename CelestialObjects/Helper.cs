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

    public static class Helper
    {
        public static void GetState(Body body, State state, Depth depth)
        {
            state.CheckCoordinatesValidity();
            Coordinates stateCoordinates = state.Coordinates;
            if (body == Body.Sun)
            {
                if (stateCoordinates.IsKeplerianCoordinatesNeeded)
                {
                    throw new NotImplementedException();
                }
                if (stateCoordinates.IsCartesianCoordinatesNeeded)
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
                                IsCartesianCoordinatesNeeded = true,
                                CartesianCoordinates = new CartesianCoordinates
                                {
                                    CoordinateFrame = cartesianCoordinates.CoordinateFrame,
                                }
                            }
                        };
                        GetHelioCentricState(origin, intState, depth);
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

        private static void GetHelioCentricState(Body body, State state, Depth depth)
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

        private static void GetBodyCentricState(Body centreBody, Body body, State state)
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
