using System;
using System.Collections.Generic;
using System.Text;

using Sunrise;
using Sunrise.Generic;
using Sunrise.Generic.Frames;
using Sunrise.CelestialObjects;
using Sunrise.Generic.States;

namespace Sunrise.Astronomy.RiseSet
{
    public class Observer
    {
        public string Name { get; set; }
        public BodyCentricCoordinates GeocentricCoordinates { get; set; }
    }

    public class RiseSet
    {
        public Observer Observer { get; set; }
        public Body Observee { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }

        public void Show()
        {
            Settings.SetEarthBased();
            Console.WriteLine(Observer.Name + ": Rise and set times for celestial body " + Observee.ToString() + "\n");
            DateTime date = TimeFrom;
            //while (date <= TimeTo)
            //{
            KeplerianCoordinates keplerianCoordinates = new KeplerianCoordinates
            {
                Origin = Body.Earth,
                CoordinateFrame = Frame.EME2000,
            };
            CoordinatesNeeded coordinatesNeeded = new CoordinatesNeeded
            {
                Keplerian = true,
                Cartesian = true,
            };
            CartesianCoordinates cartesianCoordinates = new CartesianCoordinates
            {
                Origin = Body.Earth,
                CoordinateFrame = Frame.EME2000,
            };
            CartesianCoordinates moonCartesianCoordinates = new CartesianCoordinates
            {
                Origin = Body.Moon,
                CoordinateFrame = Frame.EME2000,
            };
            KeplerianCoordinates moonKeplerianCoordinates = new KeplerianCoordinates
            {
                Origin = Body.Moon,
                CoordinateFrame = Frame.EME2000,
            };
            Coordinates earthCoordinates = new Coordinates
            {
                Body = Body.Earth,
                CoordinatesNeeded = coordinatesNeeded,
                KeplerianCoordinates = null,
                CartesianCoordinates = new List<CartesianCoordinates>
                {
                    cartesianCoordinates,
                }
            };
            Coordinates moonCoordinates = new Coordinates
            {
                Body = Body.Moon,
                CoordinatesNeeded = coordinatesNeeded,
                KeplerianCoordinates = moonKeplerianCoordinates,
                CartesianCoordinates = new List<CartesianCoordinates>
                {
                    moonCartesianCoordinates,
                }
            };
            List<Coordinates> coordinatesSet = new List<Coordinates>
            {
                earthCoordinates,
                moonCoordinates,
            };
            StateRetriever stateRetriever = new StateRetriever
            {
                CoordinatesNeeded = coordinatesNeeded,
                State = new State
                {
                    Body = Body.Sun,
                    Epoch = date,
                    CoordinatesSet = coordinatesSet,
                },
            };
            //Earth.GetHelioCentricKeplerianElements(date, keplerianCoordinates);
            //keplerianCoordinates.ArgPer = 0;
            //keplerianCoordinates.Origin = null;
            //StateRetriever.GetHelioCentricKeplerianElements(Body.Earth, date, keplerianCoordinates);
            //Start testing this one
            //coordinates.KeplerianCoordinates = null;
            StateRetriever.GetHelioCentricState(date, moonCoordinates);
            stateRetriever.GetState();
            //stateRetriever.GetBodyCentricState();
            //stateRetriever.GetHelioCentricState(Depth.Position);
            //State bodyState = new State
            //{
            //    Epoch = date,
            //    Coordinates = new Coordinates()
            //    {
            //        //Type = Generic.CoordinateType.TopoCentric,
            //        TopoCentricCoordinates = new TopoCentricCoordinates
            //        {
            //            Location = Observer.GeocentricCoordinates,
            //            LocalFrame = Frame.ENU,
            //        },
            //    }
            //};
            //Observee.GetState(bodyState);
            //}
        }
    }
}

