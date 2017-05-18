using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeProject
{
    public enum Rate
    {
        one = 1,
        two,
        three,
        four,
        five
    }
    public class UserRating
    {
        public String Name { get; set; }
        public Rate UserRate { get; set; }
        public String UserReview { get; set; }
        public User User { get; set; }

        public UserRating(User user, Rate rate, String review = "")
        {
            this.Name = user.Name;
            this.UserRate = rate;
            this.UserReview = review;
            this.User = user;
        }

        public void Print()
        {
            Console.WriteLine("User Name: {0}\t Rate:{1}", this.Name, this.UserRate);
            if (!UserReview.Equals(""))
            {
                Console.WriteLine("    Review: " + UserReview);
            }
        }
    }

    public class AllRates
    {
        private int countOfRaters;
        public List<UserRating> Ratings { get; set; }
        public int[] CountsOfRates { get; set; }
        public int CountOfRaters
        {
            get
            {
                int count = 0;
                foreach (int c in CountsOfRates)
                {
                    count += c;
                }
                return count;
            }
            set
            {
                this.countOfRaters = value;
            }
        }
        public double RatingAverage
        {
            get
            {

                int s = 0;
                for (int i = 1; i < CountsOfRates.Length; i++)
                {
                    s += CountsOfRates[i] * i;
                }
                if (CountOfRaters == 0)
                {
                    return 0;
                }
                return s / CountOfRaters;
            }
        }

        //constructors
        public AllRates()
        {
            this.CountsOfRates = new int[6];
            Ratings = new List<UserRating>();
        }
        public AllRates(List<UserRating> rates) : base()
        {
            foreach (var r in rates)
            {
                switch (r.UserRate)
                {
                    case Rate.one: CountsOfRates[1]++; break;
                    case Rate.two: CountsOfRates[2]++; break;
                    case Rate.three: CountsOfRates[3]++; break;
                    case Rate.four: CountsOfRates[4]++; break;
                    case Rate.five: CountsOfRates[5]++; break;
                }
            }
        }

        //methods

        public override string ToString()
        {
            return "Rating: " + RatingAverage + "\nCount Of 1: " + CountsOfRates[1] + "\nCount Of 2: " + CountsOfRates[2] + "\nCount Of 3: " + CountsOfRates[3] + "\nCount Of 4: " + CountsOfRates[4] + "\nCount Of 5: " + CountsOfRates[5];
        }
        public void Print()
        {
            Console.WriteLine("Rating: {0}\tcountOf1:{1} countOf2:{2} countOf3:{3} countOf4:{4} countOf5:{5}", RatingAverage, CountsOfRates[1], CountsOfRates[2], CountsOfRates[3], CountsOfRates[4], CountsOfRates[5]);
            foreach (var rate in Ratings)
            {
                rate.Print();
            }
        }
    }
}
