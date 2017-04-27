using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeProject
{
    //public class Review
    //{
    //    public User UserName { get; private set; }
    //    public string UsersReview { get; set; }

    //    public Review(User user, string review)
    //    {
    //        this.UserName = user;
    //        this.UsersReview = review;
    //    }
    //}

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
        public String Name { get; private set; }
        public Rate UserRate { get; private set; }
        public String UserReview { get; private set; }

        public UserRating(String name, Rate rate, String review = "")
        {
            this.Name = name;
            this.UserRate = rate;
            this.UserReview = review;
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
        public List<UserRating> Ratings { get; private set; }
        public int CountOf1 { get; private set; }
        public int CountOf2 { get; private set; }
        public int CountOf3 { get; private set; }
        public int CountOf4 { get; private set; }
        public int CountOf5 { get; private set; }
        public int CountOfRaters { get { return CountOf1 + CountOf2 + CountOf3 + CountOf4 + CountOf5; } }
        public double RatingAverage { get { return (double)(CountOf1 + 2 * CountOf2 + 3 * CountOf3 + 4 * CountOf4 + 5 * CountOf5) / CountOfRaters; } }

        //constructors
        public AllRates()
        {
            this.CountOf1 = 0;
            this.CountOf2 = 0;
            this.CountOf3 = 0;
            this.CountOf4 = 0;
            this.CountOf5 = 0;
        }
        public AllRates(List<UserRating> rates) : base()
        {
            foreach (var r in rates)
            {
                switch (r.UserRate)
                {
                    case Rate.one: CountOf1++; break;
                    case Rate.two: CountOf2++; break;
                    case Rate.three: CountOf3++; break;
                    case Rate.four: CountOf4++; break;
                    case Rate.five: CountOf5++; break;
                }
            }
        }

        //methods
        public void AddRate(UserRating rate)
        {
            if (Ratings == null)
            {
                this.Ratings = new List<UserRating>();
            }
            Ratings.Add(rate);
            switch (rate.UserRate)
            {
                case Rate.one: CountOf1++; break;
                case Rate.two: CountOf2++; break;
                case Rate.three: CountOf3++; break;
                case Rate.four: CountOf4++; break;
                case Rate.five: CountOf5++; break;
            }
        }
        public override string ToString()
        {
            return "Rating: " + RatingAverage + "\nCount Of 1: " + CountOf1 + "\nCount Of 2: " + CountOf2 + "\nCount Of 3: " + CountOf3 + "\nCount Of 4: " + CountOf4 + "\nCount Of 5: " + CountOf5;
        }
        public void Print()
        {
            Console.WriteLine("Rating: {0}\tcountOf1:{1} countOf2:{2} countOf3:{3} countOf4:{4} countOf5:{5}", RatingAverage, CountOf1, CountOf2, CountOf3, CountOf4, CountOf5);
            foreach (var rate in Ratings)
            {

                rate.Print();
            }
        }
    }
}
