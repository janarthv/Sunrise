using System;
using System.Collections.Generic;
using System.Text;

using Sunrise;
using Sunrise.Generic;
using Sunrise.Generic.Frames;
using Sunrise.CelestialObjects;

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
            Astronomy.Settings.SetEarthBased();
            Console.WriteLine(Observer.Name + ": Rise and set times for celestial body "+ Observee.ToString() + "\n");
            DateTime date = TimeFrom;
            while (date <= TimeTo)
            {
                State state = new State
                {
                    Epoch = date,
                    Coordinates = new Coordinates
                    {
                        CartesianCoordinates = new CartesianCoordinates
                        {
                            Origin = Body.Earth,
                            CoordinateFrame = Frame.EME2000,
                        },
                    },
                };
                StateRetriever stateRetriever = new StateRetriever
                {
                    CoordinatesNeeded = new CoordinatesNeeded
                    {
                        Cartesian = true,
                    },
                };
                stateRetriever.GetState(Body.Sun, state, Depth.Position);
                //stateRetriever.GetHelioCentricState(Body.Earth, state, Depth.Position);
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
            }
        }
    }
}
