using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeProject
{
    public class Address
    {
        public string Country { get; private set; }
        public string City { get; private set; }
        public string Street { get; private set; }
        public string NumberOfBuilding { get; private set; }

        public Address(string numberOfBuilding, string street, string city, string country)
        {
            this.Country = country;
            this.City = city;
            this.Street = street;
            this.NumberOfBuilding = numberOfBuilding;
        }

        public override string ToString()
        {
            return NumberOfBuilding + " " + Street + " Street," + City + ", " + Country;
        }
    }
}
