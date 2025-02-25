using FinanceApp.Core.Abstractions;
using FinanceApp.Core.Models;

namespace FinanceApp.Application.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountsRepository _accountsRepository;

        public AccountsService(IAccountsRepository accountsRepository)
            => _accountsRepository = accountsRepository;

        public async Task<List<Account>> GetAllAccountsAsync()
        {
           return await _accountsRepository.GetAllAsync();
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _accountsRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateAccountAsync(Account Account, int userId)
        {
            return await _accountsRepository.CreateAsync(Account, userId);
        }
               
        public async Task<int> UpdateAccountAsync(int id, decimal balance, string currencyType)
        {
            return await _accountsRepository.UpdateAsync(id, balance, currencyType);
        }

        public async Task<int> UpdateAccountСurrencyTypeAsync(int id, decimal balance, string currencyType)
        {
            var account = await _accountsRepository.GetByIdAsync(id);

            if (account.CurrencyType == "рубли" && currencyType == "у.е.")
            {
                balance = Math.Round(balance / 80, 2);

                return await _accountsRepository.UpdateAsync(id, balance, currencyType);
            }
            else if (account.CurrencyType == "у.е." && currencyType == "рубли")
            {
                balance = Math.Round(balance * 80, 2);

                return await _accountsRepository.UpdateAsync(id, balance, currencyType);
            }
            else 
            {
                return await _accountsRepository.UpdateAsync(id, balance, currencyType);
            }

        }

        public async Task<int> DeleteAccountAsync(int id)
        {
            return await _accountsRepository.DeleteAsync(id);
        }
    }
}
