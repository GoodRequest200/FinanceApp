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
                    t.AccountId,
                    t.AppointmentAccountId,
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
                    t.AccountId,
                    t.AppointmentAccountId,
                    t.CurrencyType).Transfer)
                .FirstOrDefaultAsync();

            return transfer;
        }

        //возвращает внешние перевводы на счета не принадлежищие пользователю
        public async Task<List<Transfer>> GetExternalTransfersAsync(int userId) 
        {
            var accounts = _context.Accounts.Where(a => a.UserId == userId);

            var IdOfAccounts = accounts.Select(a => a.AccountId).ToList();

            var transfersEntitites = await _context.Transfers
                .AsNoTracking()
                .Where(t => (IdOfAccounts.Contains(t.AccountId)) && (!IdOfAccounts.Contains(t.AppointmentAccountId)))
                .ToListAsync();

            var transfers = transfersEntitites
                .Select(t => Transfer.Create(
                    t.TransferId,
                    t.TransferSum,
                    t.TransferDate,
                    t.AccountId,
                    t.AppointmentAccountId,
                    t.CurrencyType).Transfer)
                .ToList();

            return transfers;
        }

        //возвращает переводы между своими счетами
        public async Task<List<Transfer>> GetTransfersOfUserOwnAsync(int userId)
        {
            var accounts = _context.Accounts.Where(a => a.UserId == userId);

            var IdOfAccounts = accounts.Select(a => a.AccountId).ToList();

            var transfersEntitites = await _context.Transfers
                .AsNoTracking()
                .Where(t => (IdOfAccounts.Contains(t.AccountId)) && (IdOfAccounts.Contains(t.AppointmentAccountId)))               
                .ToListAsync();

            var transfers = transfersEntitites
                .Select(t => Transfer.Create(
                    t.TransferId,
                    t.TransferSum,
                    t.TransferDate,
                    t.AccountId,
                    t.AppointmentAccountId,
                    t.CurrencyType).Transfer)
                .ToList();

            return transfers;
        }

        public async Task<int> CreateAsync(Transfer Transfer, int accountId, int appointmentAccountId)
        {           
            using var transaction = await _context.Database.BeginTransactionAsync();

            try 
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId)
                //var account = await _context.Accounts.FindAsync(accountId)
                      ?? throw new Exception($"Счёт с id {accountId} не найден");

                var appointmentAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == appointmentAccountId)
                //var appointmentAccount = await _context.Accounts.FindAsync(appointmentAccountId)
                      ?? throw new Exception($"Счёт назначения с id {appointmentAccountId} не найден");

                if (account.CurrencyType != appointmentAccount.CurrencyType)
                    throw new Exception($"Счёт назначения ведёт расчёты в валюте {appointmentAccount.CurrencyType}");

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

                if (Transfer.TransferSum <= 0)
                {
                    throw new ArgumentException("Сумма перевода должна быть положительной");
                }
                if (account.Balance < Transfer.TransferSum)
                {
                    throw new Exception($"Недостаточно средств на счете {account.AccountId}");
                }

                account.Balance -= Transfer.TransferSum;
                appointmentAccount.Balance += Transfer.TransferSum;

                //_context.Accounts
                //    .Where(a => a.AccountId == accountId)
                //    .ExecuteUpdate(s => s.SetProperty(a => a.Balance, a => a.Balance - Transfer.TransferSum));

                //_context.Accounts
                //    .Where(account => account.AccountId == appointmentAccountId)
                //    .ExecuteUpdate(s => s.SetProperty(a => a.Balance, a => a.Balance + Transfer.TransferSum));

                await _context.AddAsync(transferEntity);
                

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Transfer.Id;                
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }           
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
