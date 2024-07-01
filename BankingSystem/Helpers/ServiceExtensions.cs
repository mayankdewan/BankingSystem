using BankingSystem.Contracts;
using BankingSystem.Services;

namespace BankingSystem.Helpers
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryDataStore>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddControllers();
            return services;
        }
    }
}
