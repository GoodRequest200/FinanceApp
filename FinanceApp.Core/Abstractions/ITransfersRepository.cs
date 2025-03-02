using FinanceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Core.Abstractions
{
    public interface ITransfersRepository
    {
        Task<int> CreateAsync(Transfer Transfer, int accountId, int appointmentAccountId);
        Task<int> DeleteAsync(int id);
        Task<List<Transfer>> GetAllAsync();
        Task<Transfer> GetByIdAsync(int id);
        Task<List<Transfer>> GetTransfersOfUserOwnAsync(int userId);
        Task<List<Transfer>> GetExternalTransfersAsync(int userId);
    }
}
