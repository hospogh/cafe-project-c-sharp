using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace CafeProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //AllRates rate = new AllRates();
            //rate.AddRate(new UserRating("Ara", Rate.five, "sdfsfsfsdf"));
            //rate.AddRate(new UserRating("Ara", Rate.five, "sdfsfsfsdf"));
            //rate.AddRate(new UserRating("Ara", Rate.five, "sdfsfsfsdf"));
            //rate.AddRate(new UserRating("Ara", Rate.five, "sdfsfsfsdf"));
            ////rate.Print();
            //OpenTimes[] op = new OpenTimes[7];
            //op[0] = new OpenTimes(DayOfWeek.Friday, "10:00", "24:00");
            //op[0] = new OpenTimes(DayOfWeek.Wednesday, "10:00", "24:00");
            //op[0] = new OpenTimes(DayOfWeek.Saturday, "10:00", "24:00");
            //op[0] = new OpenTimes(DayOfWeek.Sunday, "10:00", "24:00");
            //op[0] = new OpenTimes(DayOfWeek.Monday, "10:00", "24:00");
            //op[0] = new OpenTimes(DayOfWeek.Thursday, "10:00", "24:00");
            //op[0] = new OpenTimes(DayOfWeek.Tuesday, "10:00", "24:00");
            //Cafe myCafe = new Cafe("Big Book", new Address("a", "a", "ak", "k"), new System.Device.Location.GeoCoordinate(23.12, 21.21), op, "321321321321", "bigBook.am");
            //myCafe.Print();

            OpenTimes[] op = new OpenTimes[7];
            op[0] = new OpenTimes(DayOfWeek.Monday, "12:12", "23:12");
            op[1] = new OpenTimes(DayOfWeek.Friday, "12:12", "23:12");
            op[2] = new OpenTimes(DayOfWeek.Saturday, "12:12", "23:12");
            op[3] = new OpenTimes(DayOfWeek.Thursday, "12:12", "23:12");
            op[4] = new OpenTimes(DayOfWeek.Tuesday, "12:12", "23:12");
            op[5] = new OpenTimes(DayOfWeek.Sunday, "12:12", "23:12");
            op[6] = new OpenTimes(DayOfWeek.Wednesday, "12:12", "23:12");
            Building building1 = new Cafe(new Address("2/9", "Northen Aveneue", "Yerevan", "Armenia"), new GeoCoordinate(43.047550, -84.698465), op, "AnnMan", "+37477553364", "annmanya.com");
            Building building2 = new Cafe(new Address("2/9", "Komitas", "Yerevan", "Armenia"), new GeoCoordinate(43.049300, -84.699813), op, "Jose", "+122121321", "annmanya.com");
            Building building3 = new Cafe(new Address("2/9", "Pushkin", "Yerevan", "Armenia"), new GeoCoordinate(43.050970, -84.694992), op, "Jazzve", "+37477553364", "annmanya.com");
            Building building4 = new Cafe(new Address("2/9", "Ani", "Musaler", "Armenia"), new GeoCoordinate(43.059503, -84.698707), op, "Tashir Cafe", "+37477553364", "annmanya.com");
            MyMap.AllBuildings.Add(building1);
            MyMap.AllBuildings.Add(building2);
            MyMap.AllBuildings.Add(building3);
            MyMap.AllBuildings.Add(building4);
            MyMap.MyConsole();


        }
    }
}
