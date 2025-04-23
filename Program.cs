using System;
using System.Collections.Generic;
using System.IO;

namespace MiniBankSystem
{
    class Program
    {

        static Queue<string> createAccountRequests = new Queue<string>();

        static List<int> accountNumber = new List<int>();
        static List<double> balances = new List<double>();
        static List<string> registedNationalDs = new List<string>();
        static void Main()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Welcome to bank system");
                Console.WriteLine("1.User Menu");
                Console.WriteLine("2.Admin Menu");
                Console.WriteLine("0.Exit");
                Console.WriteLine("Select Option: ");
                string mainChoice = Console.ReadLine();

                switch (mainChoice)
                {
                    case "1":UserMenu();break;
                    case "2": AdminMenu(); break;
                    //case "0":
                    default: Console.WriteLine("Invalid choice."); break;
                }

            }
        }
        //User Menue
        static void UserMenu()
        {
            bool inUserMenue = true;
            while (inUserMenue)
            {
                Console.Clear();
                Console.WriteLine("    User Menu    ");
                Console.WriteLine("1.Request Account Creation");
                Console.WriteLine("2.Deposit");
                Console.WriteLine("3.Withdraw");
                Console.WriteLine("4.View Balance");
                Console.WriteLine("5.Submit Riview/Complaint");
                Console.WriteLine("0.Return to main menu");
                Console.WriteLine("Select option");
                string userChoies = Console.ReadLine();

                switch (userChoies)
                {
                    case "1": RequestAccountCreation(); break;
                    case "2": Deposit(); break;
                    case "3": Withdraw(); break;
                    case "4": ViewBalance(); break;
                    case "5": SubmitRiview(); break;
                    case "0": inUserMenue = false; break;
                    default: Console.WriteLine("Invalid choice."); break;


                }
            }
        }
        //Admin Menue
        static void AdminMenu()
        {
            bool inAdminMenu = true;
            while (inAdminMenu)
            {
                Console.Clear();
                Console.WriteLine("    Admain Menu    ");
                Console.WriteLine("1.Process Next Account Request");
                string adminChoice = Console.ReadLine();
                switch (adminChoice)
                {
                    case "1": ProcessNextAccountRequest(); break;
                    case "0": inAdminMenu = false; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }
        static void RequestAccountCreation()
        {
            //ask the user to enter full name and  National ID.
            Console.WriteLine("Enter your full name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter your National ID:");
            string NationalID = Console.ReadLine();
            //check if the national id is already registerd
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(NationalID))
            {
                Console.WriteLine("This National Id and name cannot be empty.Plase try agian!!");
                return;
            }
            if (registedNationalDs.Contains(NationalID))
            {
                Console.WriteLine("This National ID is already registered. Please try again.");
                return;
            }
            // Combine the name and National ID into a single string formatted as "name|NationalID"
            string request = name + "|" + NationalID;
            
            // Add the formatted request string into a queue for account creation processing
            createAccountRequests.Enqueue(request);
            registedNationalDs.Add(NationalID);
            Console.WriteLine("Requst submitted successfuly.");
        }
        static void Deposit()
        {
            // Get the index of the account where money will be deposited
            int index = GetAccountIndex();
            // If the account index is invalid (-1), exit the method
            if (index == -1) return;
            try
            {
                //ask user to enter deposit amount
                Console.WriteLine("Enter deposit amount: ");
                // Convert the user input into a double (decimal value)
                double amount = Convert.ToDouble(Console.ReadLine());
                // Validate that the deposit amount is greater than 0
                if (amount <= 0)
                {
                    Console.WriteLine("Amount must be positive.");// Inform the user of invalid input
                    return;//Exit the metthod
                }
                // Add the deposit amount to the account balance at the specified index
                balances[index] += amount;
                Console.WriteLine("Deposit Successful.");// Confirm successful deposit
            }
            catch
            {
                // Handle invalid input (e.g., user enters a non-numeric value)
                Console.WriteLine("Invalid amount");
            }
            }

        
        static void Withdraw()
        {

        }
        static void ViewBalance()
        {

        }
        static void SubmitRiview()
        {

        }
        static void ProcessNextAccountRequest()
        {

        }
        // Method to retrieve the index of an account number from the accountNumber list
        static int GetAccountIndex()
        {
            Console.WriteLine("Enter account number: ");
            try
            {
                // Attempt to read the user's input and convert it to an integer
                int AccNum = Convert.ToInt32(Console.ReadLine());
                // Search for the account number in the accountNumber list and get its index
                int index = accountNumber.IndexOf(AccNum);
                // If the account number is not found (index == -1), inform the user and return -1
                if (index == -1)
                {
                    Console.WriteLine("Account Not Found!!");
                    return -1;
                }
                // If the account number is found, return its index
                return index;
            }
            catch
            {
                // Handle invalid input (e.g., non-numeric value entered by the user)
                Console.WriteLine("Invaild Input.");
                // Return -1 to indicate an error occurred
                return -1;
            }
    }
    }
}

