using FinanceApp.Core.Models;

namespace FinanceApp.Core.Abstractions
{
    public interface IUsersRepository
    {
        Task<int> Create(User user);
        Task<int> Delete(int id);
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<int> Update(int id, string firstName, string lastName, string email, string password, int accountCount, string? middleName);
    }
}