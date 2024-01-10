using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace johnWk4
{
    public class User

    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName
        {
            get
            {
                string userName = $"{FirstName} {LastName}";
                return userName;
            }
        }
        //public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; private set; }
        public List<BankAccount> Accounts { get; set; }
        public User()
        {
            Accounts = new List<BankAccount>();
        }
        public void CreateUser()
        {
            Console.WriteLine("Sign Up:");


            Console.Write("Enter your FirstName: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter your LastName: ");
            string lastName = Console.ReadLine();

            string password;
            do
            {
                Console.Write("Enter your password: ");
                password = Console.ReadLine();

                if (!IsValidPassword(password))
                {
                    Console.WriteLine("Invalid password. It must be at least 6 characters and contain at least one alphanumeric character and one special character (@, #, $, %, ^, &, !).");
                }
            } while (!IsValidPassword(password));

            string email;
            do
            {
                Console.Write("Enter your email: ");
                email = Console.ReadLine();

                if (!IsValidEmail(email))
                {
                    Console.WriteLine("Invalid email address. It must contain the '@' symbol.");
                }
            } while (!IsValidEmail(email));

            // User newUser = new User();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Accounts = new List<BankAccount>();

            CreateBankAccount();

           // return newUser;
        }

        public bool ValidatePassword(string password)
        {
            return Password == password;
        }

        public void CreateBankAccount()
        {
            Console.Write("Enter your account type (Savings or Current): ");
            string accountTypeInput = Console.ReadLine();

            if (Enum.TryParse(accountTypeInput, true, out AccountType accountType))
            {
                BankAccount newAccount = new BankAccount(accountType)
                {
                    Type = accountType
                };

                if (Accounts == null)
                {
                    Accounts = new List<BankAccount>();
                }

                Accounts.Add(newAccount);
              

            }
            else
            {
                Console.WriteLine("Invalid account type. Account creation failed.");
            }
        }

        public void ShowAccountMenu(List<User> users)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Account Menu:");
                Console.WriteLine($"Hello {UserName}");
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1. Deposit");
                Console.WriteLine("2. Withdraw");
                Console.WriteLine("3. Transfer Money");
                Console.WriteLine("4. View Account Information");
                Console.WriteLine("5. Interbank Transfer");
                Console.WriteLine("6. Check Account Balance");
                Console.WriteLine("7. Logout");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Deposit();
                        break;

                    case "2":
                        Withdraw();
                        break;

                    case "3":
                        TransferMoney();
                        break;

                    case "4":
                        ViewAccountInformation();
                        break;

                    case "5":
                        InterbankTransfer(users);
                        break;

                    case "6":
                        CheckAccountBalance();
                        break;

                    case "7":
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option (1-7).");
                        break;
                }
            }
        }

        private void Deposit()
        {
            Console.Clear();
            Console.Write("Enter the account number to deposit into: ");
            string accountNumber = Console.ReadLine();

            BankAccount account = GetAccount(accountNumber);

            if (account == null)
            {
                Console.WriteLine("Account not found.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter the amount to deposit: ");
            if (double.TryParse(Console.ReadLine(), out double amount))
            {
                account.Deposit(amount);
                Console.WriteLine($"Deposit successful! Your new balance is: {account.GetBalance()} Naira");
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private void Withdraw()
        {
            Console.Clear();
            Console.Write("Enter the account number to withdraw from: ");
            string accountNumber = Console.ReadLine();

            BankAccount account = GetAccount(accountNumber);

            if (account == null)
            {
                Console.WriteLine("Account not found.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter the amount to withdraw: ");
            if (double.TryParse(Console.ReadLine(), out double amount))
            {
                if (account.Withdraw(amount))
                {
                    Console.WriteLine($"Withdrawal of {amount} Naira successful! New balance: {account.GetBalance()} Naira");
                }
                else
                {
                    Console.WriteLine("Withdrawal not allowed. Minimum balance for a savings account is 1000 Naira.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private void TransferMoney()
        {
            Console.Clear();
            Console.Write("Enter the source account number: ");
            string sourceAccountNumber = Console.ReadLine();
            Console.Write("Enter the destination account number: ");
            string destinationAccountNumber = Console.ReadLine();

            BankAccount sourceAccount = GetAccount(sourceAccountNumber);
            BankAccount destinationAccount = GetAccount(destinationAccountNumber);

            if (sourceAccount == null || destinationAccount == null)
            {
                Console.WriteLine("One or both accounts not found.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter the amount to transfer: ");
            if (double.TryParse(Console.ReadLine(), out double amount))
            {
                if (sourceAccount.Transfer(destinationAccount, amount))
                {
                    Console.WriteLine($"Transfer of {amount} Naira successful!");
                    Console.WriteLine($"Source account balance: {sourceAccount.GetBalance()} Naira");
                    Console.WriteLine($"Destination account balance: {destinationAccount.GetBalance()} Naira");
                }
                else
                {
                    Console.WriteLine("Transfer failed. Insufficient funds in the source account.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private void InterbankTransfer(List<User> users)
        {
            Console.Clear();
            Console.Write("Enter the source account number: ");
            string sourceAccountNumber = Console.ReadLine();
            Console.Write("Enter the destination account number: ");
            string destinationAccountNumber = Console.ReadLine();

            BankAccount sourceAccount = GetAccount(sourceAccountNumber);
            BankAccount destinationAccount = GetAccount(destinationAccountNumber);

            if (sourceAccount == null || destinationAccount == null)
            {
                Console.WriteLine("One or both accounts not found.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            User destinationUser = users.FirstOrDefault(u => u.Accounts.Contains(destinationAccount));

            if (destinationUser == null)
            {
                Console.WriteLine("Destination account not found.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter the amount to transfer: ");
            if (double.TryParse(Console.ReadLine(), out double amount))
            {
                if (sourceAccount.Transfer(destinationAccount, amount))
                {
                    Console.WriteLine($"Interbank Transfer of {amount} Naira successful!");
                    Console.WriteLine($"Source account balance: {sourceAccount.GetBalance()} Naira");
                    Console.WriteLine($"Destination account balance: {destinationAccount.GetBalance()} Naira");
                }
                else
                {
                    Console.WriteLine("Interbank Transfer failed. Insufficient funds in the source account.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private void CheckAccountBalance()
        {
            Console.Clear();
            Console.Write("Enter the account number to check the balance: ");
            string accountNumber = Console.ReadLine();

            BankAccount account = GetAccount(accountNumber);

            if (account == null)
            {
                Console.WriteLine("Account not found.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Account Balance for Account Number {accountNumber}: {account.GetBalance()} Naira");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private void ViewAccountInformation()
        {
            Console.Clear();
            Console.WriteLine("Account Information:");
            Console.WriteLine("|----------------------|--------------------------|----------------|");
            Console.WriteLine("| Account Number       | Account Type             | Balance (Naira)|");
            Console.WriteLine("|----------------------|--------------------------|----------------|");


            foreach (var account in Accounts)
            {
                Console.WriteLine($"| {account.AccountNumber,-20} | {account.Type,-24} | {account.GetBalance(),-15:N2}|");
                //  account.PrintAccountStatement();
            }

            Console.WriteLine("|----------------------|--------------------------|----------------|");

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private BankAccount GetAccount(string accountNumber)
        {
            return Accounts.FirstOrDefault(acc => acc.AccountNumber == accountNumber);
        }

        private static bool IsValidPassword(string password)
        {
            // Use a regular expression pattern to validate the password format
            string pattern = @"^(?=.*[a-zA-Z0-9])(?=.*[@#$%^&!]).{6,}$";
            return System.Text.RegularExpressions.Regex.IsMatch(password, pattern);
        }

        private static bool IsValidEmail(string email)
        {
            return email.Contains("@");
        }
    }


}

