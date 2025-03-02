using FinanceApp.Core.Models;

namespace FinanceApp.Core.Abstractions
{
    public interface ITransfersService
    {
        Task<List<Transfer>> GetAllTransfersAsync(string sortingСondition = "По дате");
        Task<Transfer> GetTransferByIdAsync(int id);
        Task<int> CreateTransferAsync(Transfer Transfer, int accountId, int appointmentAccountId);
        Task<int> DeleteTransferAsync(int id);
        Task<List<Transfer>> GetTransfersOfUserOwnAsync(int userId);
        Task<List<Transfer>> GetExternalTransfersAsync(int userId);
    }
}
