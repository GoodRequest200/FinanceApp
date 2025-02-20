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
        Task<int> Create(Transfer Transfer);
        Task<int> Delete(int id);
        Task<List<Transfer>> GetAllAsync();
        Task<Transfer> GetByIdAsync(int id);
    }
}
