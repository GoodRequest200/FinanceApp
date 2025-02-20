using FinanceApp.Core.Abstractions;
using FinanceApp.Core.Models;
using FinanceApp.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly FinanceAppDbContext _context;

        public AccountsRepository(FinanceAppDbContext context)
            => _context = context;

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

        public Task<int> Create(Account Account)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(int id, decimal balance, string currencyType = "рубли")
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
