using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace CafeProject
{

    public struct OpenTimes
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }

        public OpenTimes(DayOfWeek day, String openingtime, String closingTime)
        {
            string[] openingT = openingtime.Split(':');
            string[] closingT = openingtime.Split(':');

            this.OpeningTime = new TimeSpan(int.Parse(openingT[0]), int.Parse(openingT[1]), 0);
            this.ClosingTime = new TimeSpan(int.Parse(closingT[0]), int.Parse(closingT[1]), 0);
            this.Day = day;
        }

        public override string ToString()
        {
            return this.Day + " " + OpeningTime + " - " + ClosingTime;
        }
    }

    public class Building
    {

        //properties
        public virtual string Type { get;  set; }
        public virtual Address BulidingAddress { get;  set; }
        public virtual GeoCoordinate Coordinates { get;  set; }
        public virtual string Name { get; set; }
 
        //constructor
        public Building() { }
        public Building(Address buildingAddress, GeoCoordinate cordinates, string type = "", string name = "")
        {
            this.Name = name;
            this.BulidingAddress = buildingAddress;
            this.Coordinates = cordinates;
            this.Type = type;
        }

        //Methods
        //public virtual List<Building> Nearby(int distanceInMeters)
        //{
        //    List<Building> nearbyBuildings = new List<Building>();
        //    foreach (Building b in MyMap.AllCafes)
        //    {
        //        if (this.Coordinates.GetDistanceTo(b.Coordinates) <= distanceInMeters)
        //            nearbyBuildings.Add(b);
        //    }

        //    return nearbyBuildings;
        //}

        public virtual double Directions(Building building)
        {
            return this.Coordinates.GetDistanceTo(building.Coordinates);
        }

        public virtual void AddRate(UserRating rate)
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}