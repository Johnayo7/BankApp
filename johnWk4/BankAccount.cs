using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace johnWk4
{
    public class BankAccount
    {
        public string AccountNumber { get; private set; }
        public double Balance { get; private set; }
        public AccountType Type { get; set; }
        private string transactionPath = @"C:\Users\Decagon\source\repos\JohnOlayemiWeek4\transferData.json";
        private List<Transaction> transactions = new List<Transaction>();

        public BankAccount(AccountType accountType)
        {
            AccountNumber = GenerateAccountNumber();
            Balance = 0.0;
            Type = accountType;
        }



        public void Deposit(double amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Invalid deposit amount. Amount must be greater than zero.");
            }
            else
            {
                Balance += amount;
                RecordTransaction("Deposit", amount);
                Console.WriteLine($"Deposit of {amount:F2} Naira. New balance: {Balance:F2} Naira");
            }
        }

        public bool Withdraw(double amount)
        {
            if (amount <= 0)
            {
                // Zero or negative amount is not valid
                return false;
            }
            else if (Type == AccountType.Savings && (Balance - amount) < 1000)
            {
                // Insufficient balance for savings account
                return false;
            }
            else if (Balance >= amount)
            {
                Balance -= amount;
                RecordTransaction("Withdrawal", -amount);
                return true;
            }
            else
            {
                // Insufficient balance for other account types
                return false;
            }
        }

        public bool Transfer(BankAccount destinationAccount, double amount)
        {
            if (Withdraw(amount))
            {
                destinationAccount.Deposit(amount);
                RecordTransaction("Transfer to " + destinationAccount.AccountNumber, -amount);
                destinationAccount.RecordTransaction("Transfer from " + AccountNumber, amount);
                return true;
            }
            return false;
        }

        public double GetBalance()
        {
            return Balance;
        }

        public void RecordTransaction(string description, double amount)
        {
            Transaction transaction = new Transaction
            {
                Date = DateTime.Now,
                Description = description,
                Amount = amount,
                Balance = Balance
            };
            transactions.Add(transaction);
            SaveTransactionDetails();
        }
        private void SaveTransactionDetails()
        {
            try
            {
                string jsonTransferData = JsonSerializer.Serialize(transactions);
                File.WriteAllText(transactionPath, jsonTransferData);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error saving transaction details: {ex.Message}");
            }

        }

        public void PrintAccountStatement()
        {
            Console.WriteLine("|----------------------|--------------------------|----------------|------------------|");
            Console.WriteLine("| Date                 | Description              | Amount (Naira) |  Balance (Naira) |");
            Console.WriteLine("|----------------------|--------------------------|----------------|------------------|");

            foreach (var transaction in transactions)
            {
                Console.WriteLine($"| {transaction.Date,-20:yyyy-MM-dd HH:mm:ss} | {transaction.Description,-24} | {transaction.Amount,-14:N2} | {transaction.Balance,-16:N2} |");
            }

            Console.WriteLine("|----------------------|--------------------------|----------------|------------------|");

        }

        public string GenerateAccountNumber()
        {
            // Generate a random 10-digit account number (for demonstration purposes)
            Random random = new Random();
            string accountNumber = "";
            for (int i = 0; i < 10; i++)
            {
                accountNumber += random.Next(10).ToString();
            }
            return accountNumber;
        }
    }
}
