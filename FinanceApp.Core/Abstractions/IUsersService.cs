using FinanceApp.Core.Models;

namespace FinanceApp.Core.Abstractions
{
    public interface IUsersService
    {
        public Task<List<User>> GetAllUsersAsync();
        public Task<User> GetUserByIdAsync(int id);
        public Task<int> CreateUserAsync(User user);
        public Task<int> UpdateUserAsync(int id, string firstName, string lastName, /*string email,*/ string password, int accountCount, string? middleName);
        public Task<int> DeleteUserAsync(int id);
    }
}
