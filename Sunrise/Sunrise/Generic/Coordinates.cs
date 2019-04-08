using System;
using System.Collections.Generic;
using System.Text;

namespace Sunrise.Generic
{
    public enum Type
    {
        Keplerian,
        Cartesian,
        Geocentric,
    }

    public class Keplerian
    {
        public double SMA { get; set; }
        public double Ecc { get; set; }
        public double Inc { get; set; }
        public double RAAN { get; set; }
        public double ArgPer { get; set; }
        public double TA { get; set; }

        public Keplerian()
        {
            SMA = 0D;
            Ecc = 0D;
            Inc = 0D;
            RAAN = 0D;
            ArgPer = 0D;
            TA = 0D;
        }

        public Keplerian(double sma, double ecc, double inc, double raan, double argPer, double ta)
        {
            SMA = sma;
            Ecc = ecc;
            Inc = inc;
            RAAN = raan;
            ArgPer = argPer;
            TA = ta;
        }
    }

    public class Position
    {
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        public Position()
        {
            X = 0D;
            Y = 0D;
            Z = 0D;
        }

        public Position(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class Velocity
    {
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        public Velocity()
        {
            X = 0D;
            Y = 0D;
            Z = 0D;
        }

        public Velocity(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class LatLon
    {
        double Lat { get; set; }
        double Lon { get; set; }

        public LatLon()
        {
            Lat = 0D;
            Lon = 0D;
        }

        public LatLon(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }
    }

    public class Cartesian
    {
        public Position Position { get; set; }
        public Velocity Velocity { get; set; }
        public Cartesian()
        {
            Position = new Position();
            Velocity = new Velocity();
        }
    }

    public class Geocentric
    {
        public LatLon LatLon { get; set; }
        public double Altitude { get; set; }
    }

    public class Coordinates
    {
        public Type Type { get; set; }
        public Keplerian Keplerian { get; set; }
        public Cartesian Cartesian { get; set; }
        public Geocentric Geocentric { get; set; }

        public Coordinates()
        {
            Type = Type.Cartesian;
            Cartesian = new Cartesian();
        }

        public Coordinates Convert(Type from, Type to)
        {
            if (from == Type.Keplerian && to == Type.Cartesian)
            {
                KeplerianToCartesian(this);
            }
            else if(from == Type.Cartesian && to == Type.Keplerian)
            {
                CartesianToKeplerian(this);
            }
            else if (from == Type.Cartesian && to == Type.Geocentric)
            {
                CartesianToGeocentric(this);
            }
            else if (from == Type.Geocentric && to == Type.Cartesian)
            {
                GeocentricToCartesian(this);
            }
            return this;
        }

        private void GeocentricToCartesian(Coordinates coordinates)
        {
            throw new NotImplementedException();
        }

        private void CartesianToGeocentric(Coordinates coordinates)
        {
            throw new NotImplementedException();
        }

        private void CartesianToKeplerian(Coordinates coordinates)
        {
            coordinates.Keplerian = new Keplerian(1, 1, 1, 1, 1, 1);
        }

        private void KeplerianToCartesian(Coordinates coordinates)
        {
            coordinates.Cartesian = new Cartesian
            {
                Position = new Position(2, 2, 2),
                Velocity = new Velocity(3, 3, 3),
            };
        }
    }

}
