using FinanceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Core.Abstractions
{
    public interface IAccountsRepository
    {
        Task<int> CreateAsync(Account Account, int userId);
        Task<int> DeleteAsync(int id);
        Task<List<Account>> GetAllAsync();
        Task<Account> GetByIdAsync(int id);
        Task<int> UpdateAsync(int id, decimal balance, string currencyType = "рубли");
    }
}
