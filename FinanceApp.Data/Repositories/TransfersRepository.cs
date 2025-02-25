using FinanceApp.Core.Abstractions;
using FinanceApp.Core.Models;
using FinanceApp.Data.DataContext;
using FinanceApp.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data.Repositories
{
    public class TransfersRepository : ITransfersRepository
    {
        private readonly FinanceAppDbContext _context;

        public TransfersRepository(FinanceAppDbContext context) => _context = context; 

        public async Task<List<Transfer>> GetAllAsync()
        {
            var transfersEntitites = await _context.Transfers
                .AsNoTracking()
                .ToListAsync();

            var transfers = transfersEntitites
                .Select(t => Transfer.Create(
                    t.TransferId,
                    t.TransferSum,
                    t.TransferDate,
                    t.CurrencyType).Transfer)
                .ToList();

            return transfers;
        }

        public async Task<Transfer> GetByIdAsync(int id)
        {
            var transfer = await _context.Transfers
                .AsNoTracking()
                .Where(t => t.TransferId == id)
                .Select(t => Transfer.Create(
                    t.TransferId,
                    t.TransferSum,
                    t.TransferDate,
                    t.CurrencyType).Transfer)
                .FirstOrDefaultAsync();

            return transfer;
        }

        public async Task<int> CreateAsync(Transfer Transfer, int accountId, int appointmentAccountId)
        {
            var account = await _context.Accounts.FindAsync(accountId)
                  ?? throw new Exception($"Счёт с id {accountId} не найден");

            var appointmentAccount = await _context.Accounts.FindAsync(appointmentAccountId)
                  ?? throw new Exception($"Счёт назначения с id {appointmentAccountId} не найден");

            var transferEntity = new TransferEntity
            {
                TransferId = Transfer.Id,
                TransferSum = Transfer.TransferSum,
                TransferDate = Transfer.TransferDate,
                CurrencyType = Transfer.CurrencyType,
                AccountId = accountId,
                AppointmentAccountId = appointmentAccountId,
                Account = account,
                AppointmentAccount = appointmentAccount
            };

            await _context.AddAsync(transferEntity);
            await _context.SaveChangesAsync();

            return Transfer.Id;

        }

        public async Task<int> DeleteAsync(int id)
        {
            await _context.Transfers
                .Where(t => t.TransferId == id)
                .ExecuteDeleteAsync();

            return id;
        }              
    }
}
