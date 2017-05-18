using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using System.Device.Location;
using System.Windows.Forms;
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
        unselect,
        addCafe,
        allCommands,
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
        //Command Prefix
        private static string CommandPrefix(User currentUser, Cafe selectedBuilding)
        {
            string commandPrefix = (currentUser == null ? "" : currentUser.Email) + ":/GMaps";
            if (selectedBuilding != null)
            {
                commandPrefix += String.Format("/{0}/{1}/{2}/{3}/{4}", selectedBuilding.Address.Country,
                    selectedBuilding.Address.City, selectedBuilding.Address.Street,
                    selectedBuilding.Address.NumberOfBuilding, selectedBuilding.Name);
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
        public static User SignUp()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine().Replace("Name: ", "");
            Console.Write("Email: ");
            string email = Console.ReadLine().Replace("Email: ", "");
            if (!IsCorrectGMail(email))
            {
                Console.WriteLine("Write right gmail account.");
                return null;
            }
            if (UserWithEmail(email) != null)
            {
                Console.WriteLine("This mail already exist.");
                return null;
            }
            string password = "";
            Console.Write("Password: ");
            password = GetPasswordFromUser();
            User newUser = new User(name, email, password);
            AllUsers.Add(newUser);
            Console.WriteLine("\nYou've successfully singed up.");
            return newUser;
        }
        //SignIn
        public static User SignIn(User currentUser)
        {
            Console.Write("Email: ");
            string email = Console.ReadLine().Replace("Email: ", "");
            User foundedUser = UserWithEmail(email);
            Console.Write("Password: ");
            string password = GetPasswordFromUser();
            Console.WriteLine();
            if (foundedUser != null)
            {
                if (foundedUser.Password == password)
                {
                    return foundedUser;
                }
            }
            Console.WriteLine("Incorrect email or password");
            return null;
        }
        //Save
        public static void Save(User currentUser, Cafe selectedBuilding)
        {
            if (selectedBuilding != null)
            {
                if (currentUser == null)
                {
                    Console.WriteLine("\nPlease log in. \n");
                }
                else
                {
                    currentUser.Save(selectedBuilding);
                }
            }
            else
            {
                MessageBox.Show("There is no selected building.");
            }
        }
        //Rate
        public static void Rate(User currentUser, Cafe selectedBuilding, string rateLine)
        {
            if (selectedBuilding != null)
            {
                if (currentUser == null)
                {
                    MessageBox.Show("Please log in");
                    return;
                }
                if (rateLine[0] - '0' > 5 || rateLine[0] - '0' < 1)
                {
                    MessageBox.Show("Your rate must be from 1 to 5.");
                    return;
                }
                UserRating rate = new UserRating(currentUser, (Rate)(rateLine[0] - '0'), rateLine.Trim());
                selectedBuilding.AddRate(rate);
                return;
            }
            MessageBox.Show("There is no selected building.");
        }
        //Nearby
        private static void Nearby(string line, User currentUser, Cafe selectedBuilding)
        {
            if (line.Split()[0].ToLower() == "me")
            {
                try
                {
                    List<Cafe> nearbyBuildings = currentUser.Nearby(int.Parse(line.Split()[1]));
                    foreach (Cafe b in nearbyBuildings)
                    {
                        Console.WriteLine(b.Name + "\n" + "Address: " + b.Address + "\n");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Incorrect distance!!!");
                    return;
                }
            }
            else if (selectedBuilding != null)
            {
                try
                {
                    List<Cafe> nearbyBuildings = selectedBuilding.Nearby(int.Parse(line));
                    foreach (Cafe b in nearbyBuildings)
                    {
                        Console.WriteLine(b.Name + "\n" + "Address: " + b.Address + "\n");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Write correct distance.");
                }
            }
            else
            {
                Console.WriteLine();
            }
        }
        //AddCafe
        private static OpenTimes[] SetOpenTimes(bool t)
        {
            Console.WriteLine("(in HH:mm format)");
            OpenTimes[] op = new OpenTimes[7];
            string line;
            if (t)
            {
                try
                {
                    foreach (string dw in Enum.GetNames(typeof(DayOfWeek)))
                    {
                        Console.WriteLine(dw + ": ");
                        Console.WriteLine("   openingTime: ");
                        line = Console.ReadLine();
                        Console.WriteLine("   closingTime: ");
                        string line2 = Console.ReadLine();
                        DayOfWeek dayw = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dw);
                        op[(int)dayw] = new OpenTimes(dayw, line, line2);
                    }
                    return op;
                }
                catch
                {
                    MessageBox.Show("Incorrect Times!!!");
                }
            }
            foreach (string dw in Enum.GetNames(typeof(DayOfWeek)))
            {
                DayOfWeek dayw = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dw);
                op[(int)dayw] = new OpenTimes(dayw, "08:00", "20:00");
            }
            return op;
        }
        private static void AddCafe()
        {
            String[] lineSplit;
            string line;
            Console.Write("Name: ");
            var name = Console.ReadLine();
            Console.Write("Address(Countr City Street NumberOfBuilding): ");
            lineSplit = (Console.ReadLine() + "           ").Split();
            var address = new Address(lineSplit[3], lineSplit[2], lineSplit[1], lineSplit[0]);
            GeoCoordinate coordinates;
            try
            {
                Console.Write("Coordinates(Latitude Longitude): ");
                lineSplit = Console.ReadLine().Split();
                coordinates = new GeoCoordinate(double.Parse(lineSplit[0]), double.Parse(lineSplit[1]));
            }
            catch
            {
                MessageBox.Show("Coordinates is incorrect!!!");
                return;
            }
            foreach (Cafe cafe in allCafes)
            {
                if (cafe.Address == address || cafe.Coordinates == coordinates)
                {
                    MessageBox.Show("Seted address or coordinates are incorrect(adr. or cord. is busy)!!!");
                    return;
                }
            }
            Console.Write("Link: ");
            var link = Console.ReadLine();
            Console.Write("Telephone: ");
            var telephone = Console.ReadLine();
            OpenTimes[] openTimeses = new OpenTimes[7];
            Console.Write("Set open times automaticly(08:00-20:00)?(Y/n)\n>> ");
            line = Console.ReadLine().ToLower();
            if (line == "n")
            {
                SetOpenTimes(true);
            }
            else if (line == "y")
            {
                SetOpenTimes(false);
                Console.WriteLine("Open Times seted automaticly.");
            }
            else
            {
                MessageBox.Show("Answer is incorrect!!!");
                return;
            }
            allCafes.Add(new Cafe(address, coordinates, openTimeses, name, telephone, link));
            Console.WriteLine("Cafe added.");
        }
        //General function
        public static void MyConsole()
        {
            const string deviderTildes = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";
            //Users and Cafes JSON deserialization
            allUsers = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(userPath));
            allCafes = JsonConvert.DeserializeObject<List<Cafe>>(File.ReadAllText(buildingPath));

            Console.WriteLine(deviderTildes);
            Console.WriteLine("Every line can contain only one command");
            PrintAllCommandes();
            Command command = Command.nothing;
            Cafe selectedBuilding = null;
            User currentUser = null;
            while (command != Command.exit)
            {
                Console.Write(CommandPrefix(currentUser, selectedBuilding));
                String line = Console.ReadLine();
                command = DetectCommand(line);
                line = line.Replace(command.ToString(), "").Trim();
                if (command == Command.nothing)
                {
                    MessageBox.Show("Command is incorrect!!!");
                    continue;
                }
                switch (command)
                {
                    case Command.allCommands:
                        PrintAllCommandes();
                        break;
                    case Command.search:
                        selectedBuilding = Search(line);
                        break;
                    case Command.signUp:
                        currentUser = SignUp();
                        break;
                    case Command.signIn:
                        if (currentUser != null)
                        {
                            Console.WriteLine("You are already logged in.");
                            break;
                        }
                        currentUser = SignIn(currentUser);
                        break;
                    case Command.addCafe:
                        if (currentUser != null)
                        {
                            Console.Write("Password: ");
                            string pass = GetPasswordFromUser();
                            if (currentUser.Password == pass)
                            {
                                Console.WriteLine();
                                AddCafe();
                                break;
                            }
                            MessageBox.Show("Password is incorrect!!!");
                            break;
                        }
                        MessageBox.Show("You aren't signed in!!!");
                        break;
                    case Command.signOut:
                        currentUser = null;
                        break;
                    case Command.save:
                        Save(currentUser, selectedBuilding);
                        break;
                    case Command.rate:
                        Rate(currentUser, selectedBuilding, line);
                        break;
                    case Command.nearby:
                        Nearby(line, currentUser, selectedBuilding);
                        break;
                    case Command.changeMyCoordinates:
                        if (currentUser != null)
                        {
                            try
                            {
                                currentUser.Coordinates =
                                    new GeoCoordinate(double.Parse(line.Split()[0]), double.Parse(line.Split()[1]));
                            }
                            catch
                            {
                                MessageBox.Show("Coordinates is Incorrect!!!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You must log in at first.");
                        }
                        break;
                    case Command.mySavedCafes:
                        foreach (Cafe b in currentUser.Saved)
                        {
                            Console.WriteLine(b);
                        }
                        break;
                    case Command.unselect:
                        selectedBuilding = null;
                        break;
                }
                //Users and Cafes JSON serialization
                File.WriteAllText(buildingPath, JsonConvert.SerializeObject(allCafes));
                File.WriteAllText(userPath, JsonConvert.SerializeObject(allUsers));
            }
            Console.WriteLine(deviderTildes + "\n Authors: AnahitMartirosyan, ManeHarutyunyan, SonaTigranyan, HosPogh ;)");
        }
    }
}