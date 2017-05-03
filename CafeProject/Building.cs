using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace CafeProject
{
    //class Time
    //{
    //    public int Hour { get; set; }
    //    public int Minute { get; set; }
    //    public Time(DateTime time)
    //    {
    //        this.Hour = time.Hour;
    //        this.Minute = time.Minute;

    //    }
    //    public Time(String time)
    //    {
    //        // in HH:mm format
    //        string[] t = time.Split(':');
    //        this.Hour = int.Parse(t[0]);
    //        this.Minute = int.Parse(t[1]);
    //    }
    //    public Time(int hour, int minute)
    //    {
    //        this.Hour = hour;
    //        this.Minute = minute;
    //    }

    //    public static bool operator ==(Time left, Time right)
    //    {
    //        return left.Hour == right.Hour && left.Minute == right.Minute;
    //    }
    //    public static bool operator !=(Time left, Time right)
    //    {
    //        return !(left == right);
    //    }
    //    public static bool operator >(Time left, Time right)
    //    {
    //        if ((left.Hour > right.Hour) || (left.Hour == right.Hour && left.Minute > right.Minute))
    //        {
    //            return true;
    //        }
    //        return false;
    //    }
    //    public static bool operator <(Time left, Time right)
    //    {
    //        if ((left.Hour < right.Hour) || (left.Hour == right.Hour && left.Minute < right.Minute))
    //        {
    //            return true;
    //        }
    //        return false;
    //    }
    //    public static bool operator >=(Time left, Time right)
    //    {
    //        return left > right || left == right;
    //    }
    //    public static bool operator <=(Time left, Time right)
    //    {
    //        return left < right || left == right;
    //    }
    //    public override string ToString()
    //    {
    //        //in HH:mm format
    //        return this.Hour + ":" + this.Minute;
    //    }
    //}

    struct OpenTimes
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingingTime { get; set; }

        public OpenTimes(DayOfWeek day, String openingtime, String closingTime)
        {
            string[] openingT = openingtime.Split(':');
            string[] closingT = openingtime.Split(':');

            this.OpeningTime = new TimeSpan(int.Parse(openingT[0]), int.Parse(openingT[1]), 0);
            this.ClosingingTime = new TimeSpan(int.Parse(closingT[0]), int.Parse(closingT[1]), 0);
            this.Day = day;
        }

        public override string ToString()
        {
            return this.Day + " " + OpeningTime + " - " + ClosingingTime;
        }
    }

    public abstract class Building
    {

        //properties
        public virtual string Type { get; protected set; }
        public abstract Address BulidingAddress { get; protected set; }
        public abstract GeoCoordinate Coordinates { get; protected set; }
        public virtual string Name { get; set; }


        //constructor
        public Building(Address buildingAddress, GeoCoordinate cordinates, String type = "", string name = "", )
        {
            this.Name = name;
            this.BulidingAddress = buildingAddress;
            this.Coordinates = cordinates;
            this.Type = type;
        }

        //Methods
        public virtual List<Building> Nearby()
        {
            List<Building> nearbyBuildings = new List<Building>();
            foreach (Building b in MyMap.AllBuildings)
            {
                if (this.Coordinates.GetDistanceTo(b.Coordinates) <= 1000)
                    nearbyBuildings.Add(b);
            }

            return nearbyBuildings;
        }

        public virtual double Directions(Building building)
        {
            return this.Coordinates.GetDistanceTo(building.Coordinates);
        }

    }
}