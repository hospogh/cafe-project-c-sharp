using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        allCommands,
        exit
    }
    public static class MyMap
    {
        private static List<Building> allBuildings = new List<Building>();
        public static List<Building> AllBuildings
        {
            get { return allBuildings; }
            set { allBuildings = value; }
        }
        static public void MyConsole()
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine().Replace("Enter Name: ", "");
            Console.Write("Enter email: ");
            string email = Console.ReadLine().Replace("Enter email: ", "");
            User myUser = new User(name, email);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Every line can contain only one command");
            Console.WriteLine();
            Command command = Command.nothing;
            Building selectedBuilding = null;
            while (command != Command.exit)
            {
                Console.Write(email + ":");
                String line = Console.ReadLine().Replace(email + ":", "");
                command = DetectCommand(line);
                line = line.Replace(command.ToString(), "").Trim();
                if (command == Command.nothing)
                {
                    Console.WriteLine("Error:  Write correct command");
                }
                else
                {
                    switch (command)
                    {
                        case Command.search:
                            List<Building> foundBuildings = Search(line);
                            if (foundBuildings != null)
                            {
                                if (foundBuildings.Count == 1)
                                {
                                    selectedBuilding = foundBuildings[0];
                                    Console.WriteLine(selectedBuilding);
                                }
                                else if (foundBuildings.Count > 1)
                                {
                                    Console.WriteLine("There are several buildings with this name:\n");
                                    foreach (Building b in foundBuildings)
                                    {
                                        Console.WriteLine(b.Name + "\n" + "Address: " + b.BulidingAddress);
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine("Which one did you mean ?");
                                }
                                else if (foundBuildings.Count == 0)
                                {
                                    foundBuildings = SearchBuildingsWithSimilarName(line);
                                    if (foundBuildings != null)
                                    {
                                        Console.WriteLine("There are several buildings with similar name:\n");
                                        foreach (Building b in foundBuildings)
                                        {
                                            Console.WriteLine(b.Name + "\n" + "Address: " + b.BulidingAddress + "\n");
                                        }
                                        Console.WriteLine("Which one did you mean ?");
                                    }
                                    else
                                    {
                                        Console.WriteLine("There are no buildings with simular name.");
                                    }
                                }
                            }
                            break;
                        case Command.save:
                            if (selectedBuilding != null)
                                myUser.Save(selectedBuilding);
                            else
                                Console.WriteLine("There is no selected building");
                            break;
                        case Command.rate:
                            if (selectedBuilding != null)
                            {
                                UserRating rate = new UserRating(myUser, (Rate)(line[0] - '0'), line.Remove(0).Trim());
                                selectedBuilding.AddRate(rate);
                            }
                            else
                            {
                                Console.WriteLine("There is no selected building");
                            }
                            break;
                        case Command.nearby:
                            if (selectedBuilding != null)
                            {
                                List<Building> nearbyBuildings = selectedBuilding.Nearby(int.Parse(line));
                                foreach (Building b in nearbyBuildings)
                                {
                                    Console.WriteLine(b.Name + "\n" + "Address: " + b.BulidingAddress + "\n");
                                }
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
                if (b.Name.Equals(nameOfCafe))
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
                if (b.Name.Contains(nameOfCafe))
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
 * user sign up
 * user log in
 * search simialr names
 * geolocation
 * rate, review
 * nearby with given distance
*/
