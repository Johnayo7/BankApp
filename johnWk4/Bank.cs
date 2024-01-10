using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace johnWk4
{
    public class Bank
    {
        public List<User> users = new List<User>();
        private static string pathName = @"C:\Users\Decagon\source\repos\JohnOlayemiWeek4\userData.json";


        public void Run()
        {
            Console.WriteLine("\t******** Welcome to Primax Bank *******\n" +
                "Our Vision is that you get the Best Service Experience...\n");

            while (true)
            {
                Console.WriteLine("Input 1 for Sign In");
                Console.WriteLine("Input 2 for Sign Up");
                Console.WriteLine("Input 3 for Exit");
                Console.Write("Please Choose an Option: ");

                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        SignIn();
                        break;

                    case "2":
                        SignUp();
                        break;

                    case "3":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                        break;
                }
            }
        }

        public void SignUp()
        {
            User newUser = new User();

             newUser.CreateUser();

            if (newUser != null)
            {
                users.Add(newUser);
                Console.WriteLine($"\nCongratulations!!!...Sign up successful.\nYour Username is: \"{newUser.UserName}\"" +
                        $" \nYour account number is: " + newUser.Accounts.First().AccountNumber);
            }

            SaveUserToJson();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        public void SignIn()
        {
            RetrieveUserFromJson();

            Console.Write("Enter your username: ");
            string userName = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            User user = users.FirstOrDefault(u => u.UserName == userName && u.ValidatePassword(password));

            if (user != null)
            {
                Console.WriteLine("Sign in successful!");
                user.ShowAccountMenu(users);
            }
            else
            {
                Console.WriteLine("Invalid username or password.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }

        public void SaveUserToJson()
        {
            try 
            {
                //var humanReadable = new JsonSerializerOptions { WriteIndented = true };
                string jsonUserData = JsonConvert.SerializeObject(users);
                File.WriteAllText(pathName, jsonUserData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving Customer Details: {ex.Message}");
            }
           
        }

        public static List<User> RetrieveUserFromJson()
        {
            string jsonFromFile = File.ReadAllText(pathName);
            List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonFromFile);
            return users;
        }
    }
}
