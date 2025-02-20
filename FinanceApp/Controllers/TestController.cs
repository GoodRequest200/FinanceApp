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
       private readonly IUsersRepository _usersRepository;

        public TestController(IUsersRepository usersRepository)
            => _usersRepository = usersRepository;
         
        [HttpGet]
        public async Task<List<User>> GetUsers() 
        { 
            return await _usersRepository.GetAllAsync();
        }
    }
}
