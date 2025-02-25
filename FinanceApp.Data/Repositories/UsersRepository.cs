using FinanceApp.Core.Models;
using FinanceApp.Data.DataContext;
using FinanceApp.Data.DataModels;
using FinanceApp.Core.Abstractions;
using Microsoft.EntityFrameworkCore;


namespace FinanceApp.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly FinanceAppDbContext _context;

        public UsersRepository(FinanceAppDbContext context) => _context = context;

        public async Task<List<User>> GetAllAsync()
        {
            var userEntities = await _context.Users
                .AsNoTracking()
                .ToListAsync();

            var users = userEntities
                .Select(u => User.Create(
                      u.UserId
                    , u.FirstName
                    , u.LastName
                    , u.Email
                    , u.Password
                    , u.AccountCount
                    , u.MiddleName).User)
                .ToList();

            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.UserId == id)
                .Select(u => User.Create(
                      u.UserId
                    , u.FirstName
                    , u.LastName
                    , u.Email
                    , u.Password
                    , u.AccountCount
                    , u.MiddleName).User)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<int> CreateAsync(User user)
        {
            var userEntity = new UserEntity
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Email = user.Email,
                Password = user.Password,
                AccountCount = user.AccountCount
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.UserId;
        }

        public async Task<int> UpdateAsync(int id
            , string firstName
            , string lastName
            //, string email
            , string password
            , int accountCount
            , string? middleName)
        {
            await _context.Users
                .Where(u => u.UserId == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.UserId, id)
                    .SetProperty(u => u.LastName, lastName)
                    //.SetProperty(u => u.Email, email)
                    .SetProperty(u => u.Password, password)
                    .SetProperty(u => u.AccountCount, accountCount)
                    .SetProperty(u => u.MiddleName, middleName)
                );

            return id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            await _context.Users
                .Where(u => u.UserId == id)
                .ExecuteDeleteAsync();

            return id;
        }
    }
}
