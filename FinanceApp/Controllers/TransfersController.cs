using FinanceApp.Api.Contracts;
using FinanceApp.Application.Services;
using FinanceApp.Core.Abstractions;
using FinanceApp.Core.Models;
using FinanceApp.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private readonly ITransfersService _transfersService;

        public TransfersController(ITransfersService transfersService)
           => _transfersService = transfersService;

        [HttpGet]
        public async Task<ActionResult<List<TransfersResponce>>> GetAllTransfers(string sortingСondition = "По дате")
        {
            var transfers = await _transfersService.GetAllTransfersAsync();

            var responce = transfers.Select(t
                => new TransfersResponce(t.Id, t.AccountId, t.AppointmentAccountId, t.CurrencyType, t.TransferDate, t.TransferSum));

            return Ok(responce);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TransfersResponce>> GetTransferById(int id)
        {
            var transfer = await _transfersService.GetTransferByIdAsync(id);

            if (transfer == null)
            {
                return NotFound($"Перевод с ID {id} не найден");
            }

            var responce = new TransfersResponce
                (id, 
                 transfer.AppointmentAccountId, 
                 transfer.AppointmentAccountId, 
                 transfer.CurrencyType, 
                 transfer.TransferDate, 
                 transfer.TransferSum);

            return Ok(responce);
        }

        [HttpGet("{userId:int}/transfersBetweenUserAccounts")]
        public async Task<ActionResult<List<TransfersResponce>>> GetTransfersOfUserOwn(int userId)
        {
            var transfers = await _transfersService.GetTransfersOfUserOwnAsync(userId);

            if (transfers == null)
            {
                return NotFound($"Переводы не найдены");
            }

            var responce = transfers.Select(t
               => new TransfersResponce(t.Id, 
                                        t.AccountId, 
                                        t.AppointmentAccountId, 
                                        t.CurrencyType, 
                                        t.TransferDate, 
                                        t.TransferSum));

            return Ok(responce);
        }

        [HttpGet("{userId:int}/externalTransfers")]
        public async Task<ActionResult<List<TransfersResponce>>> GetExternalTransfers(int userId)
        {
            var transfers = await _transfersService.GetExternalTransfersAsync(userId);

            if (transfers == null)
            {
                return NotFound($"Переводы не найдены");
            }

            var responce = transfers.Select(t
               => new TransfersResponce(t.Id,
                                        t.AccountId,
                                        t.AppointmentAccountId,
                                        t.CurrencyType,
                                        t.TransferDate,
                                        t.TransferSum));

            return Ok(responce);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateTransfer(int accountId, int appointmentAccountId, [FromBody] TransfersRequest request)
        {
            var (transfer, error) = Transfer.Create(
                RandomInteger.GenerateRandomPositiveInt(),
                request.TransferSum,
                request.TransferDate,
                accountId,
                appointmentAccountId,
                request.CurrencyType);

            if (!String.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            return Ok(await _transfersService.CreateTransferAsync(transfer, transfer.AccountId, transfer.AppointmentAccountId));
        }
    }
}

