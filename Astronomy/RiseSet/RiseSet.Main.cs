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
                Origin = Body.Sun,
                CoordinateFrame = Frame.EME2000,
            };
            CoordinatesNeeded coordinatesNeeded = new CoordinatesNeeded
            {
                Keplerian = false,
                Cartesian = true,
            };
            CartesianCoordinates cartesianCoordinates = new CartesianCoordinates
            {
                //Origin = Body.Sun,
                CoordinateFrame = Frame.EME2000,
            };
            Coordinates coordinates = new Coordinates
            {
                KeplerianCoordinates = keplerianCoordinates,
                CartesianCoordinates = new List<CartesianCoordinates>
                {
                    cartesianCoordinates,
                }
            };
            List<Coordinates> coordinatesSet = new List<Coordinates>
            {
                coordinates,
            };
            StateRetriever stateRetriever = new StateRetriever
            {
                Body = Body.Sun,
                CoordinatesNeeded = coordinatesNeeded,
                State = new State
                {
                    Epoch = date,
                    CoordinatesSet = new List<Coordinates>
                    {
                        coordinates,
                    },
                },
            };
            Earth.GetHelioCentricKeplerianElements(date, ref keplerianCoordinates);
            keplerianCoordinates.ArgPer = 0;
            keplerianCoordinates.Origin = null;
            StateRetriever.GetHelioCentricKeplerianElements(Body.Earth, date, ref keplerianCoordinates);
            //Start testing this one
            StateRetriever.GetHelioCentricState(Body.Earth, date, coordinates);
            //stateRetriever.GetState(Depth.Position);
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

