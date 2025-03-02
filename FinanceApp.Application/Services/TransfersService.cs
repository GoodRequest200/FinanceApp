using FinanceApp.Core.Abstractions;
using FinanceApp.Core.Models;

namespace FinanceApp.Application.Services
{
    public class TransfersService : ITransfersService
    {
        private readonly ITransfersRepository _transfersRepository;

        public TransfersService(ITransfersRepository transfersRepository)
        {
            _transfersRepository = transfersRepository;
        }

        //public async Task<List<Transfer>> GetAllTransfersAsync()
        //{
        //    return await _transfersRepository.GetAllAsync();
        //}

        public async Task<List<Transfer>> GetAllTransfersAsync(string sortingСondition = "По дате")
        {
            var transfers = await _transfersRepository.GetAllAsync();

            List<Transfer> orederedTransfers;

            switch (sortingСondition)
            {
                case "По дате":

                    orederedTransfers = transfers.OrderByDescending(t => t.TransferDate).ToList();

                    return orederedTransfers;

                case "По убыванию суммы":

                    orederedTransfers = transfers.OrderBy(t => t.TransferDate).ToList();

                    return orederedTransfers;
                    
                default:
                    orederedTransfers = transfers.OrderByDescending(t => t.TransferDate).ToList();

                    return orederedTransfers;
            }
            
        }

        public async Task<List<Transfer>> GetExternalTransfersAsync(int userId) 
        {
            return await _transfersRepository.GetExternalTransfersAsync(userId);
        }

        public async Task<List<Transfer>> GetTransfersOfUserOwnAsync(int userId)
        {
            return await _transfersRepository.GetTransfersOfUserOwnAsync(userId);
        }

        public Task<Transfer> GetTransferByIdAsync(int id)
        {
            return _transfersRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateTransferAsync(Transfer Transfer, int accountId, int appointmentAccountId)
        {
            return await _transfersRepository.CreateAsync(Transfer, accountId, appointmentAccountId);
        }



        public async Task<int> DeleteTransferAsync(int id)
        {
            return await _transfersRepository.DeleteAsync(id);
        }
    }
}
