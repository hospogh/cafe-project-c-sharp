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
        public int Popularity { get; private set; }
        public string Name { get; private set; }
        public string Telephone { get; set; }
        public string Link { get; set; }
        public AllRates CafeRates { get; private set; }
        public OpenTimes[] OpenTimes { get; private set; }
        public String OpeningStatus { get { return IsOpen() ? "Opened " : "Closed "; } }
        public override Address BulidingAddress { get; protected set; }
        public override GeoCoordinate Coordinates { get; protected set; }
        public static List<Building> AllCafes { get; private set; }    
         
        //constructors
        public Cafe(string name, Address buildingAddress, GeoCoordinate cordinates, OpenTimes[] openTimes, string telephone = "", string link = "") : base(buildingAddress, cordinates, "Coffe")
        {
            this.CafeRates = new AllRates();
            this.Name = name;
            this.OpenTimes = openTimes;
            this.Telephone = telephone;
            this.Link = link;
            allCafes.Add(this);
        }
        public Cafe(string name, Address buildingAddress, GeoCoordinate cordinates, OpenTimes[] openTimes, AllRates coffeRates, string telephone = "", string link = "") : base(buildingAddress, cordinates, "Coffe")
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
                    if (now.DayOfWeek.ToString() == dayOfWeek)
                    {
                        int day = int.Parse(dayOfWeek);
                        if (now.TimeOfDay.CompareTo(OpenTimes[day].OpeningTime) == 1 && now.TimeOfDay.CompareTo(OpenTimes[day].ClosingingTime) == -1)
                            return true;
                        else
                            return false;
                    }
                }
                return false;
        }

        public override List<Building> Nearby()
        {
            return base.Nearby();
        }
        public void Print()
        {
            Popularity++;
            Console.WriteLine("Caffe Name: {0}\nPopularity: {5}\nCaffe Address: {1}\nRating: {4}\nCaffe Link: {2}\nCaffe Telephone: {3}",
                 Name, BulidingAddress, Link, Telephone, CafeRates.RatingAverage, Popularity);
        }
        //public void AddPopularity(){Popularity++;}

        public override double Directions(Building building)
        {
            return base.Directions(building);
        }

        public override string ToString()
        {
            string res = "";
            res += this.Name + "\n" + this.BulidingAddress + "\n" + this.Link  + "\n"+ this.Telephone + "\n" + this.OpeningStatus + "\n";
            foreach (OpenTimes openT in OpenTimes)
            {
                res += openT + "\n";
            }

            res += this.CafeRates + "\n";

            return res;

        }
    }
}