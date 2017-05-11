using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace CafeProject
{
    sealed class Cafe : Building
    {
        private static List<Building> allCafes = new List<Building>();

        //properties
        public override string Name { get; set; }
        public string Telephone { get; set; }
        public string Link { get; set; }
        public AllRates CafeRates { get; set; }
        public OpenTimes[] OpenTimes { get; set; }
        public String OpeningStatus { get { return IsOpen() ? "Open now " : "Close now"; } }
        public override Address BulidingAddress { get; set; }
        public override GeoCoordinate Coordinates { get; set; }
        public static List<Building> AllCafes { get; private set; }

        //constructors
        public Cafe(Address buildingAddress, GeoCoordinate cordinates, OpenTimes[] openTimes, string name, string telephone = "", string link = "") : base(buildingAddress, cordinates, "Coffe", name)
        {
            this.CafeRates = new AllRates();
            this.Name = name;
            this.OpenTimes = openTimes;
            this.Telephone = telephone;
            this.Link = link;
            allCafes.Add(this);
        }
        public Cafe(string name, Address buildingAddress, GeoCoordinate cordinates, OpenTimes[] openTimes, AllRates coffeRates, string telephone = "", string link = "") : base(buildingAddress, cordinates, "Coffe", name)
        {
            this.CafeRates = coffeRates;
            this.Name = name;
            this.OpenTimes = openTimes;
            this.Telephone = telephone;
            this.Link = link;
            allCafes.Add(this);
        }
        //methods

        private Boolean IsOpen()
        {
            DateTime now = DateTime.Now;
            foreach (string dayOfWeek in Enum.GetNames(typeof(DayOfWeek)))
            {
                if (now.DayOfWeek.ToString().Equals(dayOfWeek))
                {
                    int day = (int)(DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeek);
                    if (now.TimeOfDay.CompareTo(OpenTimes[day].OpeningTime) == 1 && now.TimeOfDay.CompareTo(OpenTimes[day].ClosingingTime) == -1)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public override List<Building> Nearby(int distanceInMeters)
        {
            return base.Nearby(distanceInMeters);
        }
        public void Print()
        {
            Console.WriteLine("Caffe Name: {0}\nCaffe Address: {1}\nRating: {4}\nCaffe Link: {2}\nCaffe Telephone: {3}",
                 Name, BulidingAddress, Link, Telephone, CafeRates.RatingAverage);
        }
        //public void AddPopularity(){Popularity++;}

        public override double Directions(Building building)
        {
            return base.Directions(building);
        }

        public override string ToString()
        {
            string res = "";
            res += this.Name + "\nAddress: " + this.BulidingAddress + "\nWebsite:" + this.Link + "\nTelephone: " + this.Telephone + "\n" + this.OpeningStatus + "\n";
            foreach (OpenTimes openT in OpenTimes)
            {
                res += openT + "\n";
            }

            res += this.CafeRates + "\n";

            return res;

        }

        public override void AddRate(UserRating rate)
        {
            CafeRates.Ratings.Add(rate);
            switch (rate.UserRate)
            {
                case Rate.one: CafeRates.CountsOfRates[1]++; break;
                case Rate.two: CafeRates.CountsOfRates[2]++; break;
                case Rate.three: CafeRates.CountsOfRates[3]++; break;
                case Rate.four: CafeRates.CountsOfRates[4]++; break;
                case Rate.five: CafeRates.CountsOfRates[5]++; break;
            }
        }
    }
}