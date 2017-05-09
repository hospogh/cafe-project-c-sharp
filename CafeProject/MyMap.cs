using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using System.Device.Location;

namespace CafeProject
{
    enum Command
    {
        nothing,
        search,
        save,
        rate,
        nearby,
        directions,
        select,
        signUp,
        logIn,
        logOut,
        changeMyCoordinates,
        allCommands,
        exit
    }
    /****/
    /****/
    /****/
    public static class MyMap
    {
        private static List<Building> allBuildings = new List<Building>();
        public static List<Building> AllBuildings
        {
            get { return allBuildings; }
            set { allBuildings = value; }
        }
        private static List<User> allUsers = new List<User>();
        public static List<User> AllUsers
        {
            get { return allUsers; }
            set { allUsers = value; }
        }
        static public void MyConsole()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Every line can contain only one command");
            Console.WriteLine();
            Command command = Command.nothing;
            Command previousCommand = Command.nothing;
            Building selectedBuilding = null;
            User myUser = null;
            List<Building> foundBuildings = new List<Building>();
            while (command != Command.exit)
            {
                if (selectedBuilding != null)
                {
                    Console.WriteLine( "Cafe: " + selectedBuilding.Name + "\n");
                }
                if (myUser != null)
                {
                    Console.WriteLine("User: " + myUser.Email + "\n");
                }
                previousCommand = command;
                String line = Console.ReadLine();
                command = DetectCommand(line);
                line = line.Replace(command.ToString(), "").Trim();
                if (command == Command.nothing)
                {
                    Console.WriteLine("Error:  Write correct command \n");
                }
                else
                {
                    switch (command)
                    {
                        case Command.search:
                            foundBuildings.Clear();
                            foundBuildings = Search(line);
                            if (foundBuildings.Count == 1)
                            {
                                selectedBuilding = foundBuildings[0];
                                Console.WriteLine("\n" + selectedBuilding + "\n");
                            }
                            else if (foundBuildings.Count > 1)
                            {
                                Console.WriteLine("\nThere are several buildings with this name:\n");
                                for (int i = 0; i < foundBuildings.Count; i++)
                                {
                                    Console.WriteLine(i + 1 + " " + foundBuildings[i].Name + "\n" + "Address: " + foundBuildings[i].BulidingAddress + "\n");
                                }
                                Console.WriteLine("Which one did you mean ? \n");
                            }
                            else if (foundBuildings.Count == 0)
                            {
                                foundBuildings.Clear();
                                foundBuildings = SearchBuildingsWithSimilarName(line);
                                if (foundBuildings.Count != 0)
                                {
                                    Console.WriteLine("\nThere are several buildings with similar name:\n");
                                    for (int i = 0; i < foundBuildings.Count; i++)
                                    {
                                        Console.WriteLine(i + 1 + " " + foundBuildings[i].Name + "\n" + "Address: " + foundBuildings[i].BulidingAddress + "\n");
                                    }
                                    Console.WriteLine("Which one did you mean ? \n");
                                }
                                else
                                {
                                    Console.WriteLine("\nThere are no buildings with similar name. \n ");
                                }
                            }
                            break;
                        case Command.select:
                            if (!(previousCommand == Command.search || previousCommand == Command.select))
                            {
                                Console.WriteLine("There is nothing to choose. \n");
                            }
                            else
                            {
                                if (int.Parse(line) > foundBuildings.Count || int.Parse(line) < 1)
                                {
                                    Console.WriteLine("There is incorrect number. \n");
                                }
                                else
                                {
                                    selectedBuilding = foundBuildings[int.Parse(line) - 1];
                                    Console.WriteLine(selectedBuilding + "\n");
                                }
                            }
                            break;
                        case Command.signUp:
                            Console.Write("Name: ");
                            string name = Console.ReadLine().Replace("Name: ","");
                            Console.Write("Email: ");
                            string email = Console.ReadLine().Replace("Email: ", "");
                            if (!Regex.Replace(email, @"^[a-z0-9](\.?[a-z0-9]){5,}@g(oogle)?mail\.com$", "").Equals(""))
                            {
                                Console.WriteLine("Your account must be gmail.");
                                break;
                            }
                            Console.Write("Password: ");
                            string password = Console.ReadLine().Replace("Password: ","");
                            foreach (User user in AllUsers)
                            {
                                if (user.Email == email)
                                {
                                    Console.WriteLine("This mail already exist.");
                                }
                                else
                                {
                                    myUser = new User(name, email, password);
                                }
                            }
                            Console.WriteLine("You've successfully singed up.");
                            break;
                        case Command.logIn:
                            if (myUser == null)
                            {
                                Console.Write("Email: ");
                                email = Console.ReadLine().Replace("Email: ","");
                                Console.Write("Password: ");
                                password = Console.ReadLine().Replace("Password: ","");
                                foreach (User user in AllUsers)
                                {
                                    if (user.Email == email)
                                    {
                                        if (user.Password == password)
                                        {
                                            myUser = new User(user.Name, email, password);
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Incorrect password.");
                                            break;
                                        }
                                    }
                                    Console.WriteLine("There is no user with this email. You must sign up at first.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("You're already loged in.");
                            }
                            break;
                        case Command.logOut:
                            myUser = null;
                            break;
                        case Command.save:
                            if (selectedBuilding != null)
                            {
                                if(myUser == null)
                                {
                                    Console.WriteLine("\n Please log in. \n");
                                }
                                myUser.Save(selectedBuilding);
                            }
                            else
                                Console.WriteLine("There is no selected building \n");
                            break;
                        case Command.rate:
                            if (selectedBuilding != null)
                            {
                                if (myUser == null)
                                {
                                    Console.WriteLine("Please log in. \n");
                                }
                                else
                                {
                                    UserRating rate = new UserRating(myUser, (Rate)(line[0] - '0'), line.Remove(0).Trim());
                                    selectedBuilding.AddRate(rate);
                                }
                            }
                            else
                            {
                                Console.WriteLine("There is no selected building. \n");
                            }
                            break;
                        case Command.nearby:
                            if (line.Split()[0].ToLower() == "me")
                            {
                                List<Building> nearbyBuildings = myUser.Nearby(int.Parse(line.Split()[1]));
                                foreach (Building b in nearbyBuildings)
                                {
                                    Console.WriteLine(b.Name + "\n" + "Address: " + b.BulidingAddress + "\n");
                                }
                            }
                            else if (selectedBuilding != null)
                            {
                                List<Building> nearbyBuildings = selectedBuilding.Nearby(int.Parse(line));
                                foreach (Building b in nearbyBuildings)
                                {
                                    Console.WriteLine(b.Name + "\n" + "Address: " + b.BulidingAddress + "\n");
                                }
                            }
                            else
                            {
                                Console.WriteLine();
                            }
                            break;
                        case Command.changeMyCoordinates:
                            if (myUser != null)
                            {
                                myUser.Coordinates = new GeoCoordinate(double.Parse(line.Split()[0]), double.Parse(line.Split()[1]));
                            }
                            else
                            {
                                Console.WriteLine("You must log in at first.");
                            }
                            break;
                        case Command.allCommands:
                            PrintAllCommandes();
                            break;
                    }
                }
            }
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        public static double Directions(Building building1, Building building2)
        {
            return building1.Coordinates.GetDistanceTo(building2.Coordinates);
        }

        static private void AllComandes()
        {

        }



        //Detecting Command in line
        private static Command DetectCommand(String line)
        {
            line = line.Trim().Split()[0];
            foreach (string command in Enum.GetNames(typeof(Command)))
            {
                if (command.Equals(line))
                {
                    return (Command)Enum.Parse(typeof(Command), command);
                }
            }
            return Command.nothing;
        }
        public static List<Building> Search(String nameOfCafe)
        {
            List<Building> searchedBuildings = new List<Building>();
            foreach (Building b in allBuildings)
            {
                string name = b.Name.ToLower();
                string cafeName = nameOfCafe.ToLower();
                if (name.Equals(cafeName))
                {
                    searchedBuildings.Add(b);
                }
            }
            return searchedBuildings;
        }

        public static List<Building> SearchBuildingsWithSimilarName(String nameOfCafe)
        {
            List<Building> searchedBuildings = new List<Building>();
            foreach (Building b in allBuildings)
            {
                string name = b.Name.ToLower();
                string cafeName = nameOfCafe.ToLower();
                if (name.Contains(cafeName))
                {
                    searchedBuildings.Add(b);
                }
            }
            return searchedBuildings;
        }
        public static void PrintAllCommandes()
        {
            Console.WriteLine();
            foreach (string c in Enum.GetNames(typeof(Command)))
            {
                if (!c.Equals(Command.nothing.ToString()))
                {
                    Console.WriteLine(c);
                }
            }
            Console.WriteLine();
        }
    }
}

/*
 * user sign up ?
 * user log in ?
 * search simialr names +
 * user geolocation +?
 * rate, review +
 * directions ???
 * nearby with given distance +
*/
