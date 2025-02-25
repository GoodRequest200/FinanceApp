using FinanceApp.Api.Contracts;
using FinanceApp.Application.Services;
using FinanceApp.Core.Abstractions;
using FinanceApp.Core.Models;
using FinanceApp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;

        public AccountsController(IAccountsService accountsService)
           => _accountsService = accountsService;

        [HttpGet]
        public async Task<ActionResult<List<AccountsResponce>>> GetAllAccounts() 
        { 
            var accounts = await _accountsService.GetAllAccountsAsync();

            var responce = accounts.Select(a 
                => new AccountsResponce(a.Id, a.CurrencyType, a.Balance));

            return Ok(responce);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AccountsResponce>> GetAccountById(int id) 
        { 
            var account = await _accountsService.GetAccountByIdAsync(id);

            if (account == null) 
            { 
                return NotFound($"Счёт с ID {id} не найден");
            }

            var responce =  new AccountsResponce(id, account.CurrencyType, account.Balance);

            return Ok(responce);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateAccount(int userId, [FromBody] AccountsRequest request) 
        {
            var (account, error) = Account.Create(
                RandomInteger.GenerateRandomPositiveInt(),
                request.Balance,
                request.CurrencyType               
                );

            if (!String.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            return Ok(await _accountsService.CreateAccountAsync(account, userId));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> UpdateAccount(int id, [FromBody] AccountsRequest request) 
        {
            var account = await _accountsService.GetAccountByIdAsync(id);

            if (account.CurrencyType == request.CurrencyType)
                return Ok(await _accountsService.UpdateAccountAsync(id, request.Balance, request.CurrencyType));
            else
                return Ok(await _accountsService.UpdateAccountСurrencyTypeAsync(id, request.Balance, request.CurrencyType));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> DeleteAccount(int id)
            => Ok(await _accountsService.DeleteAccountAsync(id));
    }
}
