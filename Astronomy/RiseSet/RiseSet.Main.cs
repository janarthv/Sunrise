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
            StateRetriever stateRetriever = new StateRetriever
            {
                Body = Body.Sun,
                CoordinatesNeeded = new CoordinatesNeeded
                {
                    Keplerian = false,
                    Cartesian = true,
                },
                State = new State
                {
                    Epoch = date,
                    CoordinatesSet = new Dictionary<Body, Coordinates>
                    {
                        {
                            Body.Sun,
                            new Coordinates
                            {
                                Body = Body.Earth,
                                KeplerianCoordinates = new KeplerianCoordinates
                                {
                                    CoordinateFrame = Frame.EME2000,
                                },
                                CartesianCoordinates = new List<CartesianCoordinates>
                                {
                                    new CartesianCoordinates
                                    {
                                        CoordinateFrame = Frame.EME2000,
                                    },
                                    new CartesianCoordinates
                                    {
                                        CoordinateFrame = Frame.ECEF,
                                    },
                                },
                            }
                        },
                    },
                },
            };
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

