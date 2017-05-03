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
            Console.WriteLine("User Name: ");
            //User myUser = new User(Console.ReadLine());
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
                if (command == Command.nothing)
                {
                    Console.WriteLine("Error:  Write correct command");
                }
                else
                {

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
        private static string DetectCommand(String line)
        {
            line = line.Trim().Split()[0];
            foreach (string command in Enum.GetNames(typeof(Command)))
            {
                if (command.Equals(line))
                {
                    return command;
                }
            }
            
            return "nothing";
        }
        //public static Command DetectCommand(String line)
        //{
        //    Command c = CommandInLine(line);
        //    if (c != Command.nothing)
        //    {
        //        if (MyMap.CommandInLine(line.Replace(c.ToString(), "")) == Command.nothing)
        //        {
        //            return c;
        //        }
        //    }
        //    return Command.nothing;
        //}

        public static

        ////Prints
        static public void PrintAllCommands()
        {
            Console.WriteLine("All commands: {0}, {1}, {2}, {3}", Command.search, Command.save, Command.allCommands, Command.exit);
        }
    }
}
