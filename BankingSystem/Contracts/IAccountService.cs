using BankingSystem.Models;

namespace BankingSystem.Contracts
{
    public interface IAccountService
    {
        public Account CreateAccount(int userId);
        public bool DeleteAccount(int accountId);
        public bool Deposit(int accountId, decimal amount);
        public bool Withdraw(int accountId, decimal amount);
        public Account GetUserAccount(int accountId);
    }
}
