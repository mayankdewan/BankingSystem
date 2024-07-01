using BankingSystem.Models;

namespace BankingSystem
{
    public class InMemoryDataStore
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Account> Accounts { get; set; } = new List<Account>();

        public User GetUserById(int userId) => Users.FirstOrDefault(u => u.Id == userId);
        public Account GetAccountById(int accountId) => Accounts.FirstOrDefault(a => a.Id == accountId);
    }
}
