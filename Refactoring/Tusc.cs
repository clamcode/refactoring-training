using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Refactoring
{
    public class Tusc
    {
        public static void Start(List<User> usrs, List<Product> prods)
        {
            DisplayWelcomeMsg();

        
            // Login
            Login:
            bool loggedIn = false; // Is logged in?

            Console.WriteLine();
            string name = PromptUserInputFor("Username");

            // Validate Username
            bool validUsr = false; // Is valid user?

            
            if (!string.IsNullOrEmpty(name))
            {
           
                validUsr = IsUserNameFound(usrs, name);

                // if valid user
                if (validUsr)
                {
                    // Prompt for user input
                    string pwd = PromptUserInputFor("Password"); 

                    // Validate Password
                    bool valPwd = false; // Is valid password?
                    for (int i = 0; i < 5; i++)
                    {
                        User user = usrs[i];

                        // Check that name and password match
                        if (user.Name == name && user.Pwd == pwd)
                        {
                            valPwd = true;
                        }
                    }

                    // if valid password
                    if (valPwd == true)
                    {
                        loggedIn = true;

                        // Show welcome message
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine();
                        Console.WriteLine("Login successful! Welcome " + name + "!");
                        Console.ResetColor();
                        
                        // Show remaining balance
                        double bal = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            User usr = usrs[i];

                            // Check that name and password match
                            if (usr.Name == name && usr.Pwd == pwd)
                            {
                                bal = usr.Bal;

                                // Show balance 
                                Console.WriteLine();
                                Console.WriteLine("Your balance is " + usr.Bal.ToString("C"));
                            }
                        }

                        // Show product list
                        while (true)
                        {
                            // Prompt for user input
                            Console.WriteLine();
                            Console.WriteLine("What would you like to buy?");
                            for (int i = 0; i < 7; i++)
                            {
                                Product prod = prods[i];
                                Console.WriteLine(i + 1 + ": " + prod.Name + " (" + prod.Price.ToString("C") + ")");
                            }
                            Console.WriteLine(prods.Count + 1 + ": Exit");


                            string answer = PromptUserInputFor("a number");

                            int num = Convert.ToInt32(answer);
                            num = num - 1; /* Subtract 1 from number
                            num = num + 1 // Add 1 to number */

                            // Check if user entered number that equals product count
                            if (num == 7)
                            {
                                // Update balance
                                foreach (var usr in usrs)
                                {
                                    // Check that name and password match
                                    if (usr.Name == name && usr.Pwd == pwd)
                                    {
                                        usr.Bal = bal;
                                    }
                                }

                                // Write out new balance
                                string json = JsonConvert.SerializeObject(usrs, Formatting.Indented);
                                File.WriteAllText(@"Data/Users.json", json);

                                // Write out new quantities
                                string json2 = JsonConvert.SerializeObject(prods, Formatting.Indented);
                                File.WriteAllText(@"Data/Products.json", json2);


                                PromptForClose();
                                return;
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("You want to buy: " + prods[num].Name);
                                Console.WriteLine("Your balance is " + bal.ToString("C"));

                                // Prompt for user input
                                answer = PromptUserInputFor("amount to purchase");
                                int qty = Convert.ToInt32(answer);

                                // Check if balance - quantity * price is less than 0
                                if (bal - prods[num].Price * qty < 0)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine();
                                    Console.WriteLine("You do not have enough money to buy that.");
                                    Console.ResetColor();
                                    continue;
                                }

                                // Check if quantity is less than quantity
                                if (prods[num].Qty <= qty)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine();
                                    Console.WriteLine("Sorry, " + prods[num].Name + " is out of stock");
                                    Console.ResetColor();
                                    continue;
                                }

                                // Check if quantity is greater than zero
                                if (qty > 0)
                                {
                                    // Balance = Balance - Price * Quantity
                                    bal = bal - prods[num].Price * qty;

                                    // Quanity = Quantity - Quantity
                                    prods[num].Qty = prods[num].Qty - qty;

                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("You bought " + qty + " " + prods[num].Name);
                                    Console.WriteLine("Your new balance is " + bal.ToString("C"));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    // Quantity is less than zero
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine();
                                    Console.WriteLine("Purchase cancelled");
                                    Console.ResetColor();
                                }
                            }
                        }
                    }
                    else
                    {
                        // Invalid Password
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("You entered an invalid password.");
                        Console.ResetColor();

                        goto Login;
                    }
                }
                else
                {
                    // Invalid User
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("You entered an invalid user.");
                    Console.ResetColor();

                    goto Login;
                }
            }

            PromptForClose();
        }

        private static void DisplayWelcomeMsg()
        {
            // Write welcome message
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");
        }

        public static bool IsUserNameFound(List<User> usrs, string name)
        {
         
            const int totalUsrs = 5;
            bool matched = false;

            for (int usr = 0; usr < totalUsrs; usr++)
            {
                User user = usrs[usr];
                // Check that name matches
                if (user.Name == name)
                {
                    matched = true;
                }
            }
            return matched;
        }

        public static string PromptUserInputFor(string inputFor)
        {
            //Set the input for and return the input
            Console.WriteLine("Enter " + inputFor + ":");
            string name = Console.ReadLine();
            return name;
        }

        public static void PromptForClose()
        {
            // Prevent console from closing
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }
    }
}
