using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace CafeProject
{
    sealed class Caffe : Building
    {
        //properties
        public int Popularity { get; private set; }
        public string Name { get; private set; }
        public string Telephone { get; set; }
        public string Link { get; set; }
        public AllRates CaffeRates { get; private set; }
        public OpenTimes[] OpenTimes { get; private set; }
        public Boolean IsOpen
        {
            get
            {
                DateTime now = DateTime.Now;
                Time nowTime = new Time(now.Hour, now.Minute);
                int day;
                switch (now.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        day = (int)DayOfWeek.Sunday;
                        return OpenTimes[day].ClosingingTime > nowTime && nowTime >= OpenTimes[day].OpeningTime;
                        break;
                    case DayOfWeek.Monday:
                        day = (int)DayOfWeek.Monday;
                        return OpenTimes[day].ClosingingTime > nowTime && nowTime >= OpenTimes[day].OpeningTime;
                        break;
                    case DayOfWeek.Tuesday:
                        day = (int)DayOfWeek.Tuesday;
                        return OpenTimes[day].ClosingingTime > nowTime && nowTime >= OpenTimes[day].OpeningTime;
                        break;
                    case DayOfWeek.Wednesday:
                        day = (int)DayOfWeek.Wednesday;
                        return OpenTimes[day].ClosingingTime > nowTime && nowTime >= OpenTimes[day].OpeningTime;
                        break;
                    case DayOfWeek.Thursday:
                        day = (int)DayOfWeek.Thursday;
                        return OpenTimes[day].ClosingingTime > nowTime && nowTime >= OpenTimes[day].OpeningTime;
                        break;
                    case DayOfWeek.Friday:
                        day = (int)DayOfWeek.Friday;
                        return OpenTimes[day].ClosingingTime > nowTime && nowTime >= OpenTimes[day].OpeningTime;
                        break;
                    case DayOfWeek.Saturday:
                        day = (int)DayOfWeek.Saturday;
                        return OpenTimes[day].ClosingingTime > nowTime && nowTime >= OpenTimes[day].OpeningTime;
                        break;
                    default: return false;
                }
            }

        }
        public String OpeningStatus { get { return IsOpen ? "Opened " : "Closed "; } }
        public override Address BulidingAddress { get; protected set; }
        public override GeoCoordinate Coordinates { get; protected set; }
        public static List<Building> AllCaffes { get; private set; }
        private static List<Building> allCaffes = new List<Building>();
        //constructors
        public Caffe(string name, Address buildingAddress, GeoCoordinate cordinates, OpenTimes[] openTimes, string telephone = "", string link = "") : base(buildingAddress, cordinates, "Coffe")
        {
            this.CaffeRates = new AllRates();
            this.Name = name;
            this.OpenTimes = openTimes;
            this.Telephone = telephone;
            this.Link = link;
            allCaffes.Add(this);
        }
        public Caffe(string name, Address buildingAddress, GeoCoordinate cordinates, OpenTimes[] openTimes, AllRates coffeRates, string telephone = "", string link = "") : base(buildingAddress, cordinates, "Coffe")
        {
            this.CaffeRates = coffeRates;
            this.Name = name;
            this.OpenTimes = openTimes;
            this.Telephone = telephone;
            this.Link = link;
            allCaffes.Add(this);
        }
        //methods
        public override List<Building> Nearby()
        {
            return Building.Nearby(this, AllCaffes);
        }
        public void Print()
        {
            Popularity++;
            Console.WriteLine("Caffe Name: {0}\nPopularity: {5}\nCaffe Address: {1}\nRating: {4}\nCaffe Link: {2}\nCaffe Telephone: {3}",
                 Name, BulidingAddress, Link, Telephone, CaffeRates.RatingAverage, Popularity);
        }
        //public void AddPopularity(){Popularity++;}
    }
}