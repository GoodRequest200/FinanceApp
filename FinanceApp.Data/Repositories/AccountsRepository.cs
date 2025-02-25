using FinanceApp.Core.Abstractions;
using FinanceApp.Core.Models;
using FinanceApp.Data.DataContext;
using FinanceApp.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly FinanceAppDbContext _context;

        public AccountsRepository(FinanceAppDbContext context) => _context = context;

        public async Task<List<Account>> GetAllAsync()
        {
            var accountsEntitites = await _context.Accounts
                .AsNoTracking() 
                .ToListAsync();

            var accounts = accountsEntitites
                .Select(a => Account.Create(
                    a.AccountId,
                    a.Balance,
                    a.CurrencyType).Account)
                .ToList();

            return accounts;            
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            var account = await _context.Accounts
                .AsNoTracking()
                .Where(a => a.AccountId == id)
                .Select(a => Account.Create(
                    a.AccountId,
                    a.Balance,
                    a.CurrencyType).Account)
                .FirstOrDefaultAsync();

            return account;
        }

        public async Task<int> CreateAsync(Account Account, int userId)
        {
            var userEntity = await _context.Users.FindAsync(userId) ?? throw new Exception($"Пользователь с id {userId} не найден");

            var accountEntity = new AccountEntity
            {
                AccountId = Account.Id,
                Balance = Account.Balance,
                CurrencyType = Account.CurrencyType,
                UserId = userEntity.UserId,
                User = userEntity
            };
                
            await _context.AddAsync(accountEntity);
            await _context.SaveChangesAsync();

            return Account.Id;
        }

        public async Task<int> UpdateAsync(int id, decimal balance, string currencyType)
        {
            await _context.Accounts
                .Where(a => a.AccountId == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.AccountId, id)
                    .SetProperty(a => a.Balance, balance)
                    .SetProperty(a => a.CurrencyType, currencyType)
                );

            return id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            await _context.Accounts
                .Where(a => a.AccountId == id)
                .ExecuteDeleteAsync();

            return id;
        }
    }
}
