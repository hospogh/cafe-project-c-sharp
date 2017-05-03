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
        public GeoCoordinate Coordinates { get; set; }
        public List<Building> Saved { get; set; }
        public List<Building> UserSearches;

        public User(string name, string email)
        {
            //if (!Regex.Replace(email, @"^[a-z0-9](\.?[a-z0-9]){5,}@g(oogle)?mail\.com$", "").Equals(""))
            //{
            //    throw new Exception("Gmail is incorrect!!!!!!!!!!!!!!!");
            //}
            this.Email = email;
            this.Name = name;
            Saved = new List<Building>();
            UserSearches = new List<Building>();
        }

        public void Save(Building b)
        {
            if (this.Saved.IndexOf(b) == -1)
                this.Saved.Add(b);
        }
    }
}
