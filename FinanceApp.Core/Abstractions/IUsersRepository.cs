using FinanceApp.Core.Models;

namespace FinanceApp.Core.Abstractions
{
    public interface IUsersRepository
    {
        Task<int> CreateAsync(User user);
        Task<int> DeleteAsync(int id);
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<int> UpdateAsync(int id, string firstName, string lastName, /*string email,*/ string password, int accountCount, string? middleName);
    }
}