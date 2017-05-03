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
        static private Command[] commands = { Command.search, Command.save, Command.allCommands, Command.exit };

        private static List<Building> allBuildings = new List<Building>();
        public static List<Building> AllBuildings { get { return allBuildings; } }

        static public void MyConsole()
        {
            Console.WriteLine("Enter Name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter email: ");
            string email = Console.ReadLine();
            User myUser = new User(name, email);
            Console.WriteLine("=======================================================================================================");
            Console.WriteLine("Console Name: AnnManЯ");
            Console.WriteLine("Every line can contain only one command");
            PrintAllCommands();
            Console.WriteLine();
            Command command = Command.nothing;

            while (command != Command.exit)
            {
                String line = Console.ReadLine();
                command = DetectCommand(line);
                line = line.Replace(command.ToString(), "").Trim();
                Building selectedBuilding = null;
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
                            if (foundBuildings.Count == 1)
                            {
                                selectedBuilding = foundBuildings[0];
                                Console.WriteLine(selectedBuilding);
                            }
                            else if (foundBuildings.Count > 1)
                            {
                                Console.WriteLine("There are several buildings with this name:");
                                foreach (Building b in foundBuildings)
                                {
                                    Console.WriteLine(b.Name + "\n" +"Address: " +  b.BulidingAddress);
                                    Console.WriteLine();
                                }
                                Console.WriteLine("Which one did you mean ?");
                            }
                            else if(foundBuildings.Count == 0)
                            {
                                foundBuildings = SearchBuildingsWithSimilarName(line);
                                Console.WriteLine("There are several buildings with similar name:");
                                foreach (Building b in foundBuildings)
                                {
                                    Console.WriteLine(b.Name + "\n" + "Address: " + b.BulidingAddress + "\n");
                                }
                                Console.WriteLine("Which one did you mean ?");
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
                                UserRating rate = new UserRating(myUser, (Rate)line[0], line.Remove(0).Trim());
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
                    }
                }

                }

            Console.WriteLine("=======================================================================================================");
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
            return null;
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
            return null;
        }


        static public void PrintAllCommands()
        {
            Console.WriteLine("All commands: {0}, {1}, {2}, {3}", Command.search, Command.save, Command.allCommands, Command.exit);
        }
    }
}
