using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinanceApp.Data.Repositories;
using FinanceApp.Core.Abstractions;
using FinanceApp.Core.Models;

namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        //private readonly IUsersRepository _usersRepository;
        private readonly ITransfersRepository _transfersRepository;
        //private readonly IAccountsRepository _accountsRepository;

        public TestController (ITransfersRepository transfersRepository)
        { 
            //_usersRepository = usersRepository; 
            _transfersRepository = transfersRepository;
            //_accountsRepository = accountsRepository;
        }

        //[HttpGet]
        //public async Task<List<User>> GetUsers() 
        //{ 
        //    return await _usersRepository.GetAllAsync();
        //}

        [HttpGet]
        public async Task<Transfer> GetTransfer(int id)
        {
            return await _transfersRepository.GetByIdAsync(id);
        }

        //[HttpPost]
        //public async Task<int> CreateAccount(Account Account, int userId)
        //{
        //    return await _accountsRepository.CreateAsync(Account, userId);
        //}
    }
}
