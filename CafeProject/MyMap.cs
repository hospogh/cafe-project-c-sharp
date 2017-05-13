using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using System.Device.Location;
using Newtonsoft.Json;

namespace CafeProject
{
    enum Command
    {
        nothing,
        search,
        save,
        rate,
        nearby,
        signUp,
        logIn,
        logOut,
        removeAccount,
        changeMyCoordinates,
        mySavedCafes,
        exit
    }
    public static class MyMap
    {
        private const string userPath = @"../../users.json";
        private const string buildingPath = @"../../buildings.json";
        private static List<Cafe> allCafes = new List<Cafe>();
        public static List<Cafe> AllCafes
        {
            get { return allCafes; }
            set { allCafes = value; }
        }
        private static List<User> allUsers = new List<User>();
        public static List<User> AllUsers
        {
            get { return allUsers; }
            set { allUsers = value; }
        }
        static public void MyConsole()
        {
            allUsers = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(userPath));
            allCafes = JsonConvert.DeserializeObject<List<Cafe>>(File.ReadAllText(buildingPath));
            int i;
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Every line can contain only one command");
            Console.WriteLine("All commands:");
            for (i = 1; i < Enum.GetNames(typeof(Command)).Length; i++)
            {
                Console.WriteLine((Command)i);
            }
            Console.WriteLine();
            Command command = Command.nothing;
            Command previousCommand = Command.nothing;
            Cafe selectedBuilding = null;
            User myUser = null;
            while (command != Command.exit)
            {
                if (selectedBuilding != null)
                {
                    Console.Write(">>Cafe: " + selectedBuilding.Name + "\n");
                    Console.Write("  Address: " + selectedBuilding.BulidingAddress.City + ", " + selectedBuilding.BulidingAddress.Street + "\t");
                }
                if (myUser != null)
                {
                    Console.Write("\n>>User: " + myUser.Email + "\t");
                }
                previousCommand = command;
                String line;
                if (myUser != null)
                {
                    line = Console.ReadLine().Replace(">>User: " + myUser.Email, "");
                }
                else if (selectedBuilding != null)
                {
                    line = Console.ReadLine().Replace("Address: " + selectedBuilding.BulidingAddress.City + ", " + selectedBuilding.BulidingAddress.Street, "");
                }
                else
                {
                    line = Console.ReadLine();
                }
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
                            selectedBuilding = Search(line);
                            if (selectedBuilding != null)
                            {
                                Console.WriteLine(selectedBuilding);
                            }   
                            break;
                        case Command.signUp:
                            Console.Write("Name: ");
                            string name = Console.ReadLine().Replace("Name: ", "");
                            Console.Write("Email: ");
                            string email = Console.ReadLine().Replace("Email: ", "");
                            if (!Regex.Replace(email, @"^[a-z0-9](\.?[a-z0-9]){5,}@g(oogle)?mail\.com$", "").Equals(""))
                            {
                                Console.WriteLine("Write right gmail account.");
                                break;
                            }

                            string password = "";
                            Console.Write("Password: ");
                            ConsoleKeyInfo key;

                            do
                            {
                                key = Console.ReadKey(true);
                                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                                {
                                    password += key.KeyChar;
                                    Console.Write("*");
                                }
                                else
                                {
                                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                                    {
                                        password = password.Substring(0, (password.Length - 1));
                                        Console.Write("\b \b");
                                    }
                                }
                            }
                            while (key.Key != ConsoleKey.Enter);


                            for (i = 0; i < allUsers.Count; i++)
                            {
                                if (allUsers[i].Email == email)
                                {
                                    Console.WriteLine("This mail already exist.");
                                    break;
                                }
                            }
                            if (i == allUsers.Count)
                            {
                                myUser = new User(name, email, password);
                                AllUsers.Add(myUser);
                                Console.WriteLine("\nYou've successfully singed up.");
                            }
                            password = Encode.Encrypt(password);
                            break;
                        case Command.logIn:
                            if (myUser == null)
                            {
                                Console.Write("Email: ");
                                email = Console.ReadLine().Replace("Email: ", "");
                                for (i = 0; i < allUsers.Count; i++)
                                {
                                    if (allUsers[i].Email == email)
                                    {
                                        myUser = allUsers[i];
                                        break;
                                    }
                                }
                                if (i == allUsers.Count)
                                {
                                    Console.WriteLine("There is no user with this email.");
                                }
                                else
                                {
                                    password = "";
                                    Console.Write("Password: ");

                                    do
                                    {
                                        key = Console.ReadKey(true);
                                        if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                                        {
                                            password += key.KeyChar;
                                            Console.Write("*");
                                        }
                                        else
                                        {
                                            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                                            {
                                                password = password.Substring(0, (password.Length - 1));
                                                Console.Write("\b \b");
                                            }
                                        }
                                    }
                                    while (key.Key != ConsoleKey.Enter);
                                    //if (myUser.Password != Encode.Encrypt(password))
                                    //{
                                    //    Console.WriteLine("\nIncorrect password.");
                                    //    myUser = null;
                                    //}
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("You are already logged in.");
                            }
                            break;
                        case Command.logOut:
                            myUser = null;
                            break;
                        case Command.save:
                            if (selectedBuilding != null)
                            {
                                if (myUser == null)
                                {
                                    Console.WriteLine("\nPlease log in. \n");
                                }
                                else
                                {
                                    myUser.Save(selectedBuilding);
                                }
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
                                    if (line[0] - '0' > 5 || line[0] - '0' < 1)
                                    {
                                        Console.WriteLine("Your rate must be from 1 to 5.");
                                    }
                                    else
                                    {
                                        UserRating rate = new UserRating(myUser, (Rate)(line[0] - '0'), line.Remove(0).Trim());
                                        selectedBuilding.AddRate(rate);
                                    }
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
                                try
                                {
                                    List<Cafe> nearbyBuildings = myUser.Nearby(int.Parse(line.Split()[1]));
                                    foreach (Cafe b in nearbyBuildings)
                                    {
                                        Console.WriteLine(b.Name + "\n" + "Address: " + b.BulidingAddress + "\n");
                                    }
                                }
                                catch (FormatException)
                                {
                                    throw new Exception("Write correct distance.");
                                }
                            }
                            else if (selectedBuilding != null)
                            {
                                try
                                {
                                    List<Cafe> nearbyBuildings = selectedBuilding.Nearby(int.Parse(line));
                                    foreach (Cafe b in nearbyBuildings)
                                    {
                                        Console.WriteLine(b.Name + "\n" + "Address: " + b.BulidingAddress + "\n");
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Write correct distance.");
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
                        case Command.mySavedCafes:
                            foreach (Cafe b in myUser.Saved)
                            {
                                Console.WriteLine(b);
                            }
                            break;
                    }
                }
            }
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            File.WriteAllText(buildingPath, JsonConvert.SerializeObject(allCafes));
            File.WriteAllText(userPath, JsonConvert.SerializeObject(allUsers));
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
        public static Cafe Search(String nameOfCafe)
        {
            List<Cafe> foundBuildings = new List<Cafe>();
            foreach (Cafe b in allCafes)
            {
                string name = b.Name.ToLower();
                string cafeName = nameOfCafe.ToLower();
                if (name.Equals(cafeName))
                {
                    foundBuildings.Add(b);
                }
            }
            if (foundBuildings.Count == 1)
            {
                return foundBuildings[0];
            }
            else if (foundBuildings.Count > 1)
            {
                Console.WriteLine("\nThere are several buildings with this name:\n");
                for (int i = 0; i < foundBuildings.Count; i++)
                {
                    Console.WriteLine(i + 1 + ". " + foundBuildings[i].Name + "\n" + "Address: " + foundBuildings[i].BulidingAddress + "\n");
                }
                Console.WriteLine("Which one did you mean ? \n");
                string choose = Console.ReadLine();
                bool b = true;
                while (b == true)
                {
                    try
                    {
                        int num = int.Parse(choose);
                        if (num > foundBuildings.Count || num < 1)
                        {
                            Console.WriteLine("There is incorrect number. \n");
                            choose = Console.ReadLine();
                        }
                        else
                        {
                            b = false;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Write the number of cafe you want to choose.");
                        choose = Console.ReadLine();
                    }

                }
                return foundBuildings[int.Parse(choose) - 1];
            }
            else
            {
                foreach (Cafe b in allCafes)
                {
                    string name = b.Name.ToLower();
                    string cafeName = nameOfCafe.ToLower();
                    if (name.Contains(cafeName))
                    {
                        foundBuildings.Add(b);
                    }
                }
                if (foundBuildings.Count != 0)
                {
                    Console.WriteLine("\nThere are several buildings with similar name:\n");
                    for (int i = 0; i < foundBuildings.Count; i++)
                    {
                        Console.WriteLine(i + 1 + ". " + foundBuildings[i].Name + "\n" + "Address: " + foundBuildings[i].BulidingAddress + "\n");
                    }
                    Console.WriteLine("Which one did you mean ? \n");
                    string choose = Console.ReadLine();
                    bool b = true;
                    while (b == true)
                    {
                        try
                        {
                            int num = int.Parse(choose);
                            if (num > foundBuildings.Count || num < 1)
                            {
                                Console.WriteLine("There is incorrect number. \n");
                                choose = Console.ReadLine();
                            }
                            else
                            {
                                b = false;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Write the number of cafe you want to choose.");
                            choose = Console.ReadLine();
                        }

                    }
                    return foundBuildings[int.Parse(choose) - 1];
                }
                else
                {
                    Console.WriteLine("\nThere are no buildings with similar name. \n ");
                    return null;
                }
            }
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

