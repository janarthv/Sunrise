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
        public CelestialBody Observee { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }

        public void Show()
        {
            Astronomy.Settings.SetEarthBased();
            Console.WriteLine(Observer.Name + ": Rise and set times for celestial body "+ Observee.Name + "\n");
            DateTime date = TimeFrom;
            while (date <= TimeTo)
            {
                //Get az, el of body w.r.t observer
                State bodyState = new State
                {
                    Epoch = date,
                    Coordinates = new Coordinates()
                    {
                        Type = Generic.Type.Topocentric,
                        TopoCentricCoordinates = new TopoCentricCoordinates
                        {
                            Location = Observer.GeocentricCoordinates,
                            LocalFrame = Frame.ENU,
                        },
                    }
                };
                Observee.GetState(bodyState);
            }
        }
    }
}
