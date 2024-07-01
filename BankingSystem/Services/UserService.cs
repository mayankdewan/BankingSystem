using BankingSystem.Contracts;
using BankingSystem.Models;

namespace BankingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly InMemoryDataStore _dataStore;

        public UserService(InMemoryDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public User CreateUser(string name)
        {
            var user = new User { Id = _dataStore.Users.Count + 1, Name = name };
            _dataStore.Users.Add(user);
            return user;
        }

        public bool DeleteUser(int userId)
        {
            var user = _dataStore.GetUserById(userId);
            if (user == null) return false;
            _dataStore.Users.Remove(user);
            _dataStore.Accounts.RemoveAll(a => a.UserId == userId);
            return true;
        }

        public User GetUser(int userId)
        {
            return _dataStore.GetUserById(userId);
        }
    }
}
