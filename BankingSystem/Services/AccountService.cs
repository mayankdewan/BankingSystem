using BankingSystem.Contracts;
using BankingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly InMemoryDataStore _dataStore;

        public AccountService(InMemoryDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public Account CreateAccount(int userId)
        {
            var user = _dataStore.GetUserById(userId);
            if (user == null) return null;

            var account = new Account { Id = _dataStore.Accounts.Count + 1, UserId = userId, Balance = 100 };
            _dataStore.Accounts.Add(account);
            user.Accounts.Add(account);
            return account;
        }

        public bool DeleteAccount(int accountId)
        {
            var account = _dataStore.GetAccountById(accountId);
            if (account == null || account.Balance != 100) return false;
            _dataStore.Accounts.Remove(account);
            return true;
        }

        public bool Deposit(int accountId, decimal amount)
        {
            if (amount > 10000) return false;

            var account = _dataStore.GetAccountById(accountId);
            if (account == null) return false;
            account.Balance += amount;
            return true;
        }

        public bool Withdraw(int accountId, decimal amount)
        {
            var account = _dataStore.GetAccountById(accountId);
            if (account == null || amount > account.Balance * 0.9M || account.Balance - amount < 100) return false;
            account.Balance -= amount;
            return true;
        }

        public Account GetUserAccount(int accountId)
        {
            return _dataStore.GetAccountById(accountId);
        }
    }
}
