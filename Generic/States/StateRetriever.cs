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

    public static partial class StateRetriever
    {
        private static Dictionary<Body, DefaultState> bodyDefaultStates = new Dictionary<Body, DefaultState>();
         
        public static void GetState(State state)
        {
            if (state == null)
            {
                throw new ArgumentNullException();
            }
            state.CheckValidity();

            Body body = state.Body.Value;
            DefaultState defaultState = CelestialBodies.GetDefaultState(body);
            bodyDefaultStates.Add(body, defaultState);

            foreach (Coordinates coordinates in state.CoordinatesSet)
            {
                if (coordinates.CoordinatesNeeded == null || coordinates.Body == null)
                {
                    throw new ArgumentNullException();
                }

                Body origin = coordinates.Body.Value;
                if (origin == body)
                {
                    throw new InvalidOperationException();
                }

                DefaultState originDefaultState;
                if (defaultState.Coordinates.Body != origin)
                {
                    originDefaultState = CelestialBodies.GetDefaultState(origin);
                    bodyDefaultStates.Add(origin, originDefaultState);
                }

                CoordinatesNeeded coordinatesNeeded = coordinates.CoordinatesNeeded;

                if (coordinatesNeeded.Keplerian)
                {

                }
                if (coordinatesNeeded.Cartesian)
                {
                    foreach (CartesianCoordinates cartesianCoordinates in coordinates.CartesianCoordinates)
                    {
                        if (cartesianCoordinates == null || cartesianCoordinates.CoordinateFrame == null)
                        {
                            throw new ArgumentNullException();
                        }
                        cartesianCoordinates.Origin = origin;
                        if (origin != body)
                        {
                            CartesianCoordinates dummyCartesianCoordinates = new CartesianCoordinates
                            {
                                Depth = cartesianCoordinates.Depth,
                                CoordinateFrame = cartesianCoordinates.CoordinateFrame,
                            };
                            Coordinates dummyCoordinates = new Coordinates
                            {
                                Body = origin,
                                CoordinatesNeeded = new CoordinatesNeeded
                                {
                                    Cartesian = true,
                                },
                                KeplerianCoordinates = coordinates.KeplerianCoordinates,
                                CartesianCoordinates = new List<CartesianCoordinates>
                                        {
                                            dummyCartesianCoordinates,
                                        }
                            };
                            GetHelioCentricState(State.Epoch, dummyCoordinates);
                            dummyCartesianCoordinates.Negative(); //BETTERME
                            cartesianCoordinates.Position = dummyCartesianCoordinates.Position;
                            if (cartesianCoordinates.Depth >= CartesianDepth.Velocity)
                            {
                                cartesianCoordinates.Velocity = dummyCartesianCoordinates.Velocity;
                            }
                        }
                        else
                        {
                            cartesianCoordinates.Position = Vector<double>.Build.Dense(3);
                            if (cartesianCoordinates.Depth >= CartesianDepth.Velocity)
                            {
                                cartesianCoordinates.Velocity = Vector<double>.Build.Dense(3);
                            }
                        }
                    }
                }
            }                
        }
    }
}
