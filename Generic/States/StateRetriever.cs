using System;
using Sunrise.CelestialObjects;

namespace Sunrise.Generic.States
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
            Coordinates stateCoordinates = State.Coordinates;
            if (Body == CelestialObjects.Body.Sun)
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
                    if (origin != CelestialObjects.Body.Sun)
                    {
                        StateRetriever stateRetriever = new StateRetriever
                        {
                            Body = origin,
                            CoordinatesNeeded = new CoordinatesNeeded
                            {
                                Cartesian = true,
                            },
                        };
                        State intState = new State
                        {
                            Epoch = State.Epoch,
                            Coordinates = new Coordinates
                            {
                                CartesianCoordinates = new CartesianCoordinates
                                {
                                    CoordinateFrame = cartesianCoordinates.CoordinateFrame,
                                }
                            }
                        };
                        stateRetriever.State = intState;
                        stateRetriever.GetHelioCentricState(depth);
                        intState.Coordinates.CartesianCoordinates.Negative(); //BETTERME
                        cartesianCoordinates.Position = intState.Coordinates.CartesianCoordinates.Position;
                        if (depth == Depth.Velocity)
                        {
                            cartesianCoordinates.Velocity = intState.Coordinates.CartesianCoordinates.Velocity;
                        }
                    }
                }
                if (CoordinatesNeeded.BodyCentric)
                {
                    BodyCentricCoordinates bodyCentricCoordinates = stateCoordinates.BodyCentricCoordinates;
                    if (bodyCentricCoordinates == null || bodyCentricCoordinates.CentreBody == null || bodyCentricCoordinates.BodyFrame == null)
                    {
                        throw new ArgumentNullException();
                    }
                    Body centreBody = bodyCentricCoordinates.CentreBody.Value;
                    if (centreBody != CelestialObjects.Body.Sun)
                    {
                        //GetBodyCentricState(body);
                    }
                }
            }
        }
    }
}
