using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using System.Threading.Tasks;
using System.Device.Location;
namespace CafeProject
{
    public class User
    {
        public String Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public GeoCoordinate Coordinates { get; set; }
        public List<Cafe> Saved { get; set; }
        public List<Cafe> UserSearches;

        public User()
        {
            this.Email = "";
            this.Name = "";
            this.Password = "";
            Saved = new List<Cafe>();
            UserSearches = new List<Cafe>();
        }

        public User(string name, string email, string password)
        {
            if (!Regex.Replace(email, @"^[a-z0-9](\.?[a-z0-9]){5,}@g(oogle)?mail\.com$", "").Equals(""))
            {
                throw new Exception("Gmail is incorrect!!!!!!!!!!!!!!!");
            }
            this.Email = email;
            this.Name = name;
            this.Password = password;
            Saved = new List<Cafe>();
            UserSearches = new List<Cafe>();
        }

        public void Save(Cafe b)
        {
            if (this.Saved.IndexOf(b) == -1)
                this.Saved.Add(b);
        }

        public virtual List<Cafe> Nearby(int distanceInMeters)
        {
            List<Cafe> nearbyBuildings = new List<Cafe>();
            foreach (Cafe b in MyMap.AllCafes)
            {
                if (this.Coordinates.GetDistanceTo(b.Coordinates) <= distanceInMeters)
                    nearbyBuildings.Add(b);
            }

            return nearbyBuildings;
        }
    }
}
