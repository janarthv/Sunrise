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
        public State State { get; set; }
        public CoordinatesNeeded CoordinatesNeeded { get; set; }

        public void GetState()
        {
            if (State == null || CoordinatesNeeded == null)
            {
                throw new ArgumentNullException();
            }
            State.CheckValidity();
            Body body = State.Body.Value;
            foreach (Coordinates coordinates in State.CoordinatesSet)
            {
                Body origin = coordinates.Body.Value;
                CoordinatesNeeded localCoordinatesNeeded = coordinates.CoordinatesNeeded;
                if (CoordinatesNeeded.Keplerian && (localCoordinatesNeeded == null || localCoordinatesNeeded.Keplerian))
                {
                    
                }
                if (CoordinatesNeeded.Cartesian && (localCoordinatesNeeded == null || localCoordinatesNeeded.Cartesian))
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
