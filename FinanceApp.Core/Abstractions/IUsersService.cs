using FinanceApp.Core.Models;

namespace FinanceApp.Core.Abstractions
{
    public interface IUsersService
    {
        public Task<List<User>> GetAllUsersAsync();
    }
}
