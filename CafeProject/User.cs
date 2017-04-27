using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeProject
{
    public class User
    {
        public String Name { get; set; }
        public string Email { get; private set; }
        public List<Building> Saved { get; set; }
        public List<Building> UserSearches;

        public User(string name)
        {
            this.Name = name;
            Saved = new List<Building>();
            UserSearches = new List<Building>();
        }

        public void Save(Building b)
        {
            if (this.Saved == null) { Saved = new List<Building>(); }
            this.Saved.Add(b);
        }
    }
}
