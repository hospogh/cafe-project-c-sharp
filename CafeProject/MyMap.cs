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
        signIn,
        signOut,
        removeAccount,
        changeMyCoordinates,
        mySavedCafes,
        cafeInDestance,
        exit
    }


    public static class MyMap
    {
        //Files paterns for serialization
        private const string userPath = @"../../users.json";

        private const string buildingPath = @"../../buildings.json";

        //Buildings
        private static List<Cafe> allCafes = new List<Cafe>();

        public static List<Cafe> AllCafes
        {
            get { return allCafes; }
            set { allCafes = value; }
        }

        //Users
        private static List<User> allUsers = new List<User>();

        public static List<User> AllUsers
        {
            get { return allUsers; }
            set { allUsers = value; }
        }


        /* Console */
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

        public static void PrintAllCommandes()
        {
            Console.WriteLine("all commandes: ");
            foreach (string c in Enum.GetNames(typeof(Command)))
            {
                if (!c.Equals(Command.nothing.ToString()))
                {
                    Console.WriteLine(c);
                }
            }
            Console.WriteLine();
        }

        //search
        private static List<Cafe> StrictSearch(string cafeName)
        {
            List<Cafe> foundCafes = new List<Cafe>();
            foreach (Cafe b in allCafes)
            {
                string name = b.Name.ToLower();
                if (name.Equals(cafeName))
                {
                    foundCafes.Add(b);
                }
            }
            return foundCafes;
        }

        private static List<Cafe> NonstrictSearch(string cafeName)
        {
            List<Cafe> foundedCafes = new List<Cafe>();
            foreach (Cafe b in allCafes)
            {
                string name = b.Name.ToLower();
                if (name.Contains(cafeName))
                {
                    foundedCafes.Add(b);
                }
            }
            return foundedCafes;
        }

        private static void PrintFoundedCafes(List<Cafe> foundedCafes)
        {
            for (int i = 0; i < foundedCafes.Count; i++)
            {
                Console.WriteLine("{0}. {1}\nAddress: {2}", i, foundedCafes[i].Name, foundedCafes[i].Address + "\n");
            }
        }

        private static int GetNumberFromUser(int foundCafesCount)
        {
            int num;
            while (true)
            {
                try
                {
                    num = int.Parse(Console.ReadLine());
                    if (num > foundCafesCount || num <= 0)
                    {
                        Console.WriteLine("There is incorrect number. \n>> ");
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("Write the number of cafe you want to choose.\n>> ");
                }
            }
            return num - 1;
        }

        public static Cafe Search(String cafeName)
        {
            cafeName = cafeName.ToLower();
            List<Cafe> foundedCafes = StrictSearch(cafeName);
            if (foundedCafes.Count == 0)
            {
                foundedCafes = NonstrictSearch(cafeName);
            }
            if (foundedCafes.Count == 1)
            {
                return foundedCafes[0];
            }
            if (foundedCafes.Count > 1)
            {
                Console.WriteLine("\nThere are several buildings with this or simular name:\n");
                PrintFoundedCafes(foundedCafes);
                Console.Write("Which one did you mean ? \n>> ");
                Cafe selectedCafe = foundedCafes[GetNumberFromUser(foundedCafes.Count)];
                Console.WriteLine(selectedCafe);
                return selectedCafe; //GetNumberFromUser method get a only correct number
            }
            Console.WriteLine("\nThere are no buildings with this or similar name. \n ");
            return null;
        }

        public static void Rate(string line, Cafe selectedBuilding, User user)
        {
            if (line[0] - '0' > 5 || line[0] - '0' < 1)
            {
                Console.WriteLine("Your rate must be from 1 to 5.");
            }
            else
            {
                for (int i = 0; i < selectedBuilding.CafeRates.Ratings.Count; i++)
                {
                    if (selectedBuilding.CafeRates.Ratings[i].User.Email == user.Email)
                    {
                        selectedBuilding.CafeRates.CountOfRaters--;
                        selectedBuilding.CafeRates.CountsOfRates[(int)selectedBuilding.CafeRates.Ratings[i].UserRate]--;
                        selectedBuilding.CafeRates.Ratings.Remove(selectedBuilding.CafeRates.Ratings[i]);
                    }

                }
                UserRating rate = new UserRating(user, (Rate)(line[0] - '0'), line.Substring(1).Trim());
                selectedBuilding.AddRate(rate);
            }
        }

        //General function
        public static void MyConsole()

        //Command Prefix
        private static string CommandPrefix(User currentUser, Cafe selectedBuilding)
        {
            string commandPrefix = (currentUser == null ? "" : currentUser.Email) + ":/GMaps";
            if (selectedBuilding != null)
            {
                commandPrefix += String.Format("/{0}/{1}/{2}/{3}/Cafe:{4}", selectedBuilding.Address.Country,
                    selectedBuilding.Address.City, selectedBuilding.Address.Street,
                    selectedBuilding.Address.NumberOfBuilding, selectedBuilding);
            }
            return commandPrefix + "$  ";
        }

        //Sign Up method return newUser
        private static bool IsCorrectGMail(String email)
        {
            return Regex.Replace(email, @"^[a-z0-9](\.?[a-z0-9]){5,}@g(oogle)?mail\.com$", "").Equals("");
        }

        private static User UserWithEmail(String email)
        {
            foreach (User user in AllUsers)
            {
                if (user.Email == email)
                {
                    return user;
                }
            }
            return null;
        }

        private static string GetPasswordFromUser()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            } while (key.Key != ConsoleKey.Enter);
            return Encode.Encrypt(password);
        }

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
                            break;
                        case Command.signIn:
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
                        case Command.signOut:
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
            Console.WriteLine(deviderTildes);
            //Users and Cafes JSON serialization
            File.WriteAllText(buildingPath, JsonConvert.SerializeObject(allCafes));
            File.WriteAllText(userPath, JsonConvert.SerializeObject(allUsers));
        }
    }
}

