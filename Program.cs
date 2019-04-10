using System;

using Sunrise.Generic;
using Sunrise.Generic.Frames;
using Sunrise.Astronomy.RiseSet;
using Sunrise.Generic.States;

namespace Sunrise
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime from = new DateTime(year: 2019, month: 4, day: 4, hour: 0, minute: 0, second: 0, millisecond: 0, kind: DateTimeKind.Utc);
            DateTime to = new DateTime(year: 2019, month: 4, day: 5, hour: 0, minute: 0, second: 0, millisecond: 0, kind: DateTimeKind.Utc);

            Console.WriteLine("Hello New Worlds!");
            RiseSet riseSet = new RiseSet
            {
                TimeFrom = from,
                TimeTo = to,
                Observee = CelestialObjects.Body.Sun,
                Observer = new Observer
                {
                    Name = "Vishnu",
                    GeocentricCoordinates = new BodyCentricCoordinates(),
                },
            };
            riseSet.Show();
        }
    }
}
