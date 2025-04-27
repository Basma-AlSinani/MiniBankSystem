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
        // static List<string> Password = new List<string>();

        static Stack<string> reviewsStack = new Stack<string>();

        const double MinimumBalance = 100.0;
        const string AccountsFilePath = "accounts.txt";
        const string ReviewsFilePath = "reviews.txt";
        const string RequestsFilePath = "requests.txt";
        const string AdminPassword = "1233";

        static int lastAccountNumber = 0;
        static void Main()
        {
            //accountNumber.Add(0);
            //balances.Add(0);
            LoadAccountsInformationFromFile();
            LoadReviews();
            LoadRequests();


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
                    case "1": UserAccess(); break;
                    case "2": if (AdmainLogin()) AdminMenu(); break;
                    case "0":
                        SaveAccountsInformationFromFile();
                        SaveReviews();
                        SaveRequests();
                        running = false;
                        break;
                    default: Console.WriteLine("Invalid choice."); break;
                }

            }
        }
        //usre Login
        static void UserAccess()
        {
            bool inAccessMenue = true;
            while (inAccessMenue)
            {
                Console.Clear();
                Console.WriteLine("  User Access  ");
                Console.WriteLine("1.Request Account Creation");
                Console.WriteLine("2.Login to Existing Account");
                Console.WriteLine("0.Return to main menu");
                Console.WriteLine("Select Option: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": RequestAccountCreation(); break;
                    case "2":
                        if (UserLogin())
                            UserMenu(); break;
                    case "0": inAccessMenue = false; break;
                    default:
                        Console.WriteLine("Invalid choice. Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }

            }
        }
        static bool UserLogin()
        {
            Console.WriteLine("Enter you account number: ");
            try
            {
                int accNum = Convert.ToInt32(Console.ReadLine());
                int index = accountNumber.IndexOf(accNum);
                if (index != -1)
                {
                    Console.WriteLine($"Welcome,{AccountName[index]}!");
                    Console.ReadLine();
                    return true;
                }
                else {
                    Console.WriteLine("Account not found!");
                    Console.ReadLine();
                    return false;

                }
            } catch
            {
                Console.WriteLine("Invaild Input");
                Console.ReadLine();
                return false;
            }
        }
        //Admain Login
        static bool AdmainLogin()
        {
            Console.WriteLine("Enter Admain Password: ");
            string Password = Console.ReadLine();
            if (Password == AdminPassword)
            {
                Console.WriteLine("Login Successful.Welcome Admin!");
                Console.ReadLine();
                return true;
            }
            else
            {
                Console.WriteLine("Incorrect password!");
                Console.ReadLine();
                return false;
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
                //Console.WriteLine("1.Request Account Creation");
                Console.WriteLine("1.Deposit");
                Console.WriteLine("2.Withdraw");
                Console.WriteLine("3.View Balance");
                Console.WriteLine("4.Submit Riview");
                Console.WriteLine("0.Return to main menu");
                Console.WriteLine("Select option");
                string userChoies = Console.ReadLine();

                switch (userChoies)
                {
                    //case "1": RequestAccountCreation(); break;
                    case "1": Deposit(); break;
                    case "2": Withdraw(); break;
                    case "3": ViewBalance(); break;
                    case "4": SubmitRiview(); break;
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
                Console.WriteLine("2.View Submitted Reviews");
                Console.WriteLine("3.View All Accounts");
                Console.WriteLine("4.View Pending Account Requests");
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
            if (NationalID.Length < 7 || NationalID.Length > 10)
            {
                Console.WriteLine("The National ID must be between 7 and 10 digits. Please try again!");
                Console.ReadLine();
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
                //Display balance after deposit
                Console.WriteLine($"Deposit Successful.New Balance: {balances[index]}");// Confirm successful deposit
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
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter Withdraw amount: ");
                    //read and convert the input amount
                    double amount = Convert.ToDouble(Console.ReadLine());
                    //validate that the amount positive
                    if (amount == 0)
                    {
                        Console.WriteLine("Withdraw cancelled");
                        break;
                    }
                    if (amount < 0)
                    {
                        Console.WriteLine("The amount must be positive");
                        continue;
                    }
                    // Check if the balance after withdrawal  greterthan or equal MinimumBalance
                    if (balances[index] - amount >= MinimumBalance)
                    {
                        //to Withdraw
                        balances[index] -= amount;
                        Console.WriteLine($"Withdrawal successful.New Balance: {balances[index]}");
                        Console.ReadLine();
                        break;

                    }
                    else
                    {
                        Console.WriteLine("Insufficient balance after minimum limit");
                        continue;
                    }
                }
                catch
                {
                    //to handel invalid input
                    Console.WriteLine("Invalid amount");
                }
            }
        }
        static void ViewBalance()
        {
            int index = GetAccountIndex();
            if (index == -1) return;
            //check if the account index is vaild and the account data exists
            if (index < 0 || index >= accountNumber.Count || index >= AccountName.Count || index >= balances.Count)
            {
                //print error message if the account data invaild  
                Console.WriteLine("ERROR:Acount information is missing.");
                Console.ReadLine();
                return;
            }
            //if index vaild display account details
            Console.WriteLine($"Account Number: {accountNumber[index]}");
            Console.WriteLine($"Holder Name: {AccountName[index]}");
            Console.WriteLine($"Current Balance:{balances[index]}");
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
        }
        //user submeits review
        static void SubmitRiview()
        {
            Console.Write("Enter your reviw: ");
            //read the review input frome user
            string review = Console.ReadLine();
            //add (push) the review to the top of the review stack
            reviewsStack.Push(review);
            Console.WriteLine("Thank you! Your feedback has been recorded.");
            Console.ReadLine();//to allow user to read the massege before continuing
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
            int Attempts = 0;
            const int MaxAttempts = 3;
            while (Attempts < MaxAttempts)// Keep asking until the user enters a valid account number
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
                        Attempts++;
                        //if account not found inform the user and ask again
                        Console.WriteLine($"Account not found! You Have {MaxAttempts - Attempts}attempts");
                        continue;//try agin
                    }
                    // If vaild account number is found, return its index
                    return index;
                }
                catch
                {
                    //if not vaild inform the user and ask agin
                    Console.WriteLine("Invaild Input.Please enter a valid number.");
                    Attempts++;
                    // Console.ReadLine();
                    // Return -1 to indicate an error occurred
                    //return -1;
                }
            }
            Console.WriteLine("You have exceeded the maximum number of attempts. Returning to main menu.");
            Console.ReadLine();
            return -1;

        }
        static void ViewReviews()
        {
            //check if there are no reviews submeitted yet
            if (reviewsStack.Count == 0)
            {
                Console.WriteLine("No reviews submeitted yet!");
                Console.ReadLine();
                return;//exit method
            }
            //if there are rivews 
            Console.WriteLine("Recent Reviews:");
            //loop through each review in stak
            foreach (string Re in reviewsStack)
            {
                //display each review
                Console.WriteLine("- " + Re);
                Console.ReadLine();
            }
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
        }

        static void ViewAllAccounts()
        {
            Console.WriteLine("== All Accounts ==");
            //check if there are no account
            if (accountNumber.Count == 0)
            {
                Console.WriteLine("NO Acount FOUND!!");
            } else
                //loop through all accounts and display their detalis
                for (int i = 0; i < accountNumber.Count; i++)
                {
                    Console.WriteLine($"Account Number: {accountNumber[i]}\nName: {AccountName[i]}\nBalance: {balances[i]}");
                    Console.WriteLine("------------");
                }
            // After displaying all accounts, wait for user to press Enter
            Console.WriteLine("press Enter to Continue");
            Console.ReadLine();
        }
        static void ViewPendingRequests()
        {
            Console.WriteLine("== Pending Account Requests ==");
            if (createAccountRequests.Count == 0)
            {
                Console.WriteLine("No pending requests.");
                Console.ReadLine();
                return;
            }
            foreach (string request in createAccountRequests)
            {
                string[] parts = request.Split('|');
                Console.WriteLine($"Name: {parts[0]}, National ID: {parts[1]}");
                Console.ReadLine();
            }
        }

        static void SaveAccountsInformationFromFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AccountsFilePath))
                {
                    for (int i = 0; i < accountNumber.Count; i++)
                    {
                        string dataLine = $"{accountNumber[i]},{AccountName[i]},{balances[i]}";
                        writer.WriteLine(dataLine);
                    }
                }
                Console.WriteLine("Accounts saved successfully.");
            }
            catch
            {
                Console.WriteLine("Error saving file.");
            }
        }

        static void LoadAccountsInformationFromFile()
        {
            if (!File.Exists(AccountsFilePath))
            {
                Console.WriteLine("No saved data found.");
                return;
            }
            try
            {


                accountNumber.Clear();
                AccountName.Clear();
                balances.Clear();
                using (StreamReader reader = new StreamReader(AccountsFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        int accNum = Convert.ToInt32(parts[0]);
                        accountNumber.Add(accNum);
                        AccountName.Add(parts[1]);
                        balances.Add(Convert.ToDouble(parts[2]));

                        if (accNum > lastAccountNumber)
                            lastAccountNumber = accNum;
                    }
                }

                Console.WriteLine("Accounts loaded successfully.");
            }
            catch
            {
                Console.WriteLine("Error loading file.");
            }
        }
        static void SaveReviews()
            {
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(ReviewsFilePath))
                        {
                            foreach (var review in reviewsStack)
                            {
                                writer.WriteLine(review);
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error saving reviews.");
                    }
                }
            }
            static void LoadReviews()
            {
                try
                {
                    if (!File.Exists(ReviewsFilePath)) return;

                    using (StreamReader reader = new StreamReader(ReviewsFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            reviewsStack.Push(line);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Error loading reviews.");
                }
            }

            static void SaveRequests()
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(RequestsFilePath))
                    {
                        foreach (var request in createAccountRequests)
                        {
                            writer.WriteLine(request);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Error savin account requests.");
                }
            }
            static void LoadRequests()
            {
                try
                {
                    if (!File.Exists(RequestsFilePath)) return;
                    using (StreamReader reader = new StreamReader(RequestsFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            createAccountRequests.Enqueue(line);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Error lodaing account requests.");
                }
            }

        }
    }


