using johnWk4;
using System.Reflection.Metadata;
using Xunit;

namespace ValidTest
{
    public class UnitTest1
    {
        [Fact]
        public void Validate_GeneratedAccountNumber()
        {
            // Arrange
            var account = new BankAccount(AccountType.Savings);

            // Act
            string accountNumber = account.GenerateAccountNumber();

            // Assert
            Assert.NotNull(accountNumber);
            Assert.Equal(10, accountNumber.Length);
            Assert.Matches("^[0-9]{10}$", accountNumber);
        }


        [Fact]
            public void Validate_Deposited_Amount()
            {
                // Arrange
                var account = new BankAccount(AccountType.Current);

                // Act
                account.Deposit(1000);

                // Assert
                Assert.Equal(1000, account.GetBalance(), 2); 
            }

            [Fact]
            public void Validate_Deposit_Of_ZeroAmount()
            {
                // Arrange
                var account = new BankAccount(AccountType.Savings);

                // Act
                account.Deposit(0);

                // Assert
                Assert.Equal(0, account.GetBalance(), 2);
            }

            [Fact]
            public void Validate_Withdraw_Of_ValidAmount()
            {
                // Arrange
                var account = new BankAccount(AccountType.Current);
                account.Deposit(2000);

                // Act
                var result = account.Withdraw(1000);

                // Assert
                Assert.True(result);
                Assert.Equal(1000, account.GetBalance(), 2);
            }

            [Fact]
            public void Validate_Withdrawal_from_InsufficientBalance()
            {
                // Arrange
                var account = new BankAccount(AccountType.Savings);
                account.Deposit(500);

                // Act
                var result = account.Withdraw(1000);

                // Assert
                Assert.False(result);
                Assert.Equal(500, account.GetBalance(), 2);
            }

            [Fact]
            public void Validate_Withdrawal_Of_NegativeAmount()
            {
                // Arrange
                var account = new BankAccount(AccountType.Current);
                account.Deposit(2000);

                // Act
                var result = account.Withdraw(-1000);

                // Assert
                Assert.False(result);
                Assert.Equal(2000, account.GetBalance(), 2);
            }

            [Fact]
            public void Validate_Transfer_from_InsufficientBalance()
            {
                // Arrange
                var sourceAccount = new BankAccount(AccountType.Current);
                var destinationAccount = new BankAccount(AccountType.Savings);
                sourceAccount.Deposit(500);

                // Act
                var result = sourceAccount.Transfer(destinationAccount, 1000);

                // Assert
                Assert.False(result);
                Assert.Equal(500, sourceAccount.GetBalance(), 2);
                Assert.Equal(0, destinationAccount.GetBalance(), 2);
            }
       


    }
}