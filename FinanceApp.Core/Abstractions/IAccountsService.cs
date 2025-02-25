using FinanceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Core.Abstractions
{
    public interface IAccountsService
    {
        Task<List<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountByIdAsync(int id);
        Task<int> CreateAccountAsync(Account Account, int userId);
        Task<int> UpdateAccountAsync(int id, decimal balance, string currencyType);
        public Task<int> UpdateAccountСurrencyTypeAsync(int id, decimal balance, string currencyType);
        Task<int> DeleteAccountAsync(int id);                
    }
}
