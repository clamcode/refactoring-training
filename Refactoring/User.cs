using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    [Serializable]
    public class User
    {
        [JsonProperty("Username")] public string Name;
        [JsonProperty("Password")] public string Pwd;
        [JsonProperty("Balance")] public double Bal;

        const Int16 TotalUsers = 5;

        public  bool ValidateUser(List<User> usrs)
        {

            // Login
            Login:
            bool loggedIn = false; // Is logged in?

            Console.WriteLine();
            string name = PromptUserInputFor("Username");
            bool matchedNamePassword = false;

            if (!string.IsNullOrEmpty(name))
            {
                bool validUsr = false; // Is valid user?
             
                validUsr = IsUserNameFound(usrs, name);

                // if valid user
                if (validUsr)
                {
                    // Prompt for user input
                    string pwd = PromptUserInputFor("Password");

                    // Validate Password
                    matchedNamePassword = IsUserNamePasswordMatched(usrs, name, pwd);
                }
                
            }
            return matchedNamePassword;
        }

        public  bool IsUserNamePasswordMatched(List<User> usrs, string name, string pwd)
        {
            bool valPwd = false;
            for (int usr = 0; usr < TotalUsers; usr++)
            {
                User user = usrs[usr];

                // Check that name and password match
                if (user.Name == name && user.Pwd == pwd)
                {
                    valPwd = true;
                }
            }
            return valPwd;  
        }

        public  bool IsUserNameFound(List<User> usrs, string name)
        {
            bool matched = false;

            for (int usr = 0; usr < TotalUsers; usr++)
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

        public  string PromptUserInputFor(string inputFor)
        {
            //Set the input for and return the input
            Console.WriteLine("Enter " + inputFor + ":");
            string name = Console.ReadLine();
            return name;
        }
    }
}
