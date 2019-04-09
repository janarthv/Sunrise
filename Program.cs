using System;

using Sunrise.Generic;
using Sunrise.Generic.Frames;
using CoordinateType = Sunrise.Generic.CoordinateType;

namespace Sunrise
{
    class Program
    {
        static void Main(string[] args)
        {
            State earthState = new State
            {
                Frame = Frame.EME2000,
                Epoch = new DateTime(year: 2019, month: 4, day: 4, hour: 0, minute: 0, second: 0, millisecond: 0, kind: DateTimeKind.Utc),
                Coordinates = new Coordinates
                {
                    Type = CoordinateType.Keplerian,
                    Keplerian = new KeplerianCoordinates(),
                },
            };
            DateTime from = new DateTime(year: 2019, month: 4, day: 4, hour: 0, minute: 0, second: 0, millisecond: 0, kind: DateTimeKind.Utc);
            DateTime to = new DateTime(year: 2019, month: 4, day: 5, hour: 0, minute: 0, second: 0, millisecond: 0, kind: DateTimeKind.Utc);

            Console.WriteLine("Hello New Worlds!");
        }
    }
}
