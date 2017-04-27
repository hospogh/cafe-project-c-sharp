﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace CafeProject
{
    class Time
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public Time(DateTime time)
        {
            this.Hour = time.Hour;
            this.Minute = time.Minute;
        }
        public Time(String time)
        {
            // in HH:mm format
            string[] t = time.Split(':');
            this.Hour = int.Parse(t[0]);
            this.Minute = int.Parse(t[1]);
        }
        public Time(int hour, int minute)
        {
            this.Hour = hour;
            this.Minute = minute;
        }

        public static bool operator ==(Time left, Time right)
        {
            return left.Hour == right.Hour && left.Minute == right.Minute;
        }
        public static bool operator !=(Time left, Time right)
        {
            return !(left == right);
        }
        public static bool operator >(Time left, Time right)
        {
            if ((left.Hour > right.Hour) || (left.Hour == right.Hour && left.Minute > right.Minute))
            {
                return true;
            }
            return false;
        }
        public static bool operator <(Time left, Time right)
        {
            if ((left.Hour < right.Hour) || (left.Hour == right.Hour && left.Minute < right.Minute))
            {
                return true;
            }
            return false;
        }
        public static bool operator >=(Time left, Time right)
        {
            return left > right || left == right;
        }
        public static bool operator <=(Time left, Time right)
        {
            return left < right || left == right;
        }
        public override string ToString()
        {
            //in HH:mm format
            return this.Hour + ":" + this.Minute;
        }
    }

    struct OpenTimes
    {
        public DayOfWeek Day { get; set; }
        public Time OpeningTime { get; set; }
        public Time ClosingingTime { get; set; }

        public OpenTimes(DayOfWeek day, Time openingtime, Time closingTime)
        {
            this.OpeningTime = openingtime;
            this.ClosingingTime = closingTime;
            this.Day = day;
        }
        public OpenTimes(DayOfWeek day, String openingtime, String closingTime)
        {
            this.OpeningTime = new Time(openingtime);
            this.ClosingingTime = new Time(closingTime);
            this.Day = day;
        }
        public override string ToString()
        {
            return this.Day + " " + OpeningTime + " - " + ClosingingTime;
        }
    }

    public abstract class Building
    {
        private static List<Building> allBuildings = new List<Building>();
        //properties
        public virtual string Type { get; protected set; }
        public abstract Address BulidingAddress { get; protected set; }
        public abstract GeoCoordinate Coordinates { get; protected set; }
        public static List<Building> AllBuildings { get { return allBuildings; } }

        //constructor
        public Building(Address buildingAddress, GeoCoordinate cordinates, String type = "")
        {
            //this.Name = name;
            this.BulidingAddress = buildingAddress;
            this.Coordinates = cordinates;
            this.Type = type;
            allBuildings.Add(this);
        }

        //Methods
        public virtual List<Building> Nearby()
        {
            List<Building> nearbyBuildings = new List<Building>();
            foreach (Building b in AllBuildings)
            {
                if (this.Coordinates.GetDistanceTo(b.Coordinates) <= 1000)
                    nearbyBuildings.Add(b);
            }

            return nearbyBuildings;
        }
        public static List<Building> Nearby(Building build, List<Building> building)
        {
            List<Building> nearbyBuildings = new List<Building>();
            foreach (Building b in building)
            {
                if (build.Coordinates.GetDistanceTo(b.Coordinates) <= 1000)
                {
                    nearbyBuildings.Add(b);
                }
            }
            return nearbyBuildings;
        }
    }
}