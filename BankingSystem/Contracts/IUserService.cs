using BankingSystem.Models;

namespace BankingSystem.Contracts
{
    public interface IUserService
    {
        public User CreateUser(string name);
        public bool DeleteUser(int userId);
        public User GetUser(int userId);
    }
}
