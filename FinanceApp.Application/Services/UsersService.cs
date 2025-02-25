using FinanceApp.Core.Abstractions;
using FinanceApp.Core.Models;

namespace FinanceApp.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository userRepository)
            => _usersRepository = userRepository;

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _usersRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _usersRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateUserAsync(User user)
        {
            return await _usersRepository.CreateAsync(user);
        }

        public async Task<int> UpdateUserAsync(int id, string firstName, string lastName, /*string email,*/ string password, int accountCount, string? middleName)
        {
            return await _usersRepository.UpdateAsync(id, firstName, lastName, /*email,*/ password, accountCount, middleName);
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            return await _usersRepository.DeleteAsync(id);
        }
    }
}
