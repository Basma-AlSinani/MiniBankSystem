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
        static List<string> AccountName = new List<string>();

        const double MinimumBalance = 100.0;

        static int lastAccountNumber;
        static void Main()
        {
            accountNumber.Add(0);
            balances.Add(0);
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
                    case "1": UserMenu(); break;
                    case "2": AdminMenu(); break;
                    case "0":
                        // SaveAccountsInformationToFile();
                        // SaveReviews();
                        running = false;
                        break;
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
                Console.WriteLine("0. Return to Main Menu");
                string adminChoice = Console.ReadLine();
                switch (adminChoice)
                {
                    case "1": ProcessNextAccountRequest(); break;
                    case "2": ViewReviews(); break;
                    case "3": ViewAllAccounts(); break;
                    case "4": ViewPendingRequests(); break;
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
                Console.ReadLine();
                return;
            }
            // Combine the name and National ID into a single string formatted as "name|NationalID"
            string request = name + "|" + NationalID;

            // Add the formatted request string into a queue for account creation processing
            createAccountRequests.Enqueue(request);
            registedNationalDs.Add(NationalID);
            Console.WriteLine("Requst submitted successfuly.");
            Console.ReadLine();
        }
        static void Deposit()
        {

            // Get the index of the account where money will be deposited
            int index = GetAccountIndex();
            // If the account index is invalid (-1), exit the method
            if (index == -1) return;
            //{
            //    Console.WriteLine("Account Not found!");
            //    return;
            //}
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
                    Console.ReadLine();
                    return;//Exit the metthod
                }
                if (amount < 1)
                {
                    Console.WriteLine("Minimum depostit amount is 1 OMR!");
                    Console.ReadLine();
                    return;
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
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();

        }


        static void Withdraw()
        {
            //get the index of account to withdraw from
            int index = GetAccountIndex();
            if (index == -1) return;
            try
            {
                Console.WriteLine("Enter Withdraw amount: ");
                //read and convert the input amount
                double amount = Convert.ToDouble(Console.ReadLine());
                //validate that the amount positive
                if (amount <= 0)
                {
                    Console.WriteLine("The amount must be positive");
                    return;
                }
                // Check if the balance after withdrawal  greterthan or equal MinimumBalance
                if (balances[index]-amount>= MinimumBalance)
                {
                    //to Withdraw
                    balances[index] -= amount;
                    Console.WriteLine("Withdrawal successful.");

                }
                else
                {
                    Console.WriteLine("nsufficient balance after minimum limit");
                }
            }
            catch
            {
                //to handel invalid input
                Console.WriteLine("Invalid amount");
            }
        }
        static void ViewBalance()
        {

        }
        static void SubmitRiview()
        {

        }
        static void ProcessNextAccountRequest()
        {
            //check if there are any pending account creation requests.
            if (createAccountRequests.Count == 0)
            {
                Console.WriteLine("No pending account requests.");
                return;
            }
            //dequeue the next account creation request from the queue.
            string request = createAccountRequests.Dequeue();
            //to spilt the requst string by using |.
            string[] parts = request.Split('|');
            //extract the name frome request
            string name = parts[0];
            //extract the ID frome request
            string nationalID = parts[1];
            //generate a new account number based on the lastAccountNumber.
            int NewAccountNumber = lastAccountNumber + 1;
            //add account to list of account number
            accountNumber.Add(NewAccountNumber);
            //add name to list of account names
            AccountName.Add(name);
            //initialize the account balance to zero for the new account and add it to the balance list.
            balances.Add(0.0);
            //update to add new account 
            lastAccountNumber = NewAccountNumber;
            //message to user successful account creation
            Console.WriteLine($"Account created for {name} with Account Number:{NewAccountNumber}");
            Console.ReadLine();
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
                    Console.ReadLine();
                    return -1;
                }
                // If the account number is found, return its index
                return index;
            }
            catch
            {
                // Handle invalid input (e.g., non-numeric value entered by the user)
                Console.WriteLine("Invaild Input.");
                Console.ReadLine();
                // Return -1 to indicate an error occurred
                return -1;
            }
        }
        static void ViewReviews()
        {

        }

        static void ViewAllAccounts()
        {

        }
        static void ViewPendingRequests()
        {

        }
    }
}

