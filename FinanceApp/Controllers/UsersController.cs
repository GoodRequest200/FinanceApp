using FinanceApp.Api.Contracts;
using FinanceApp.Core.Abstractions;
using FinanceApp.Infrastructure;
using Microsoft.AspNetCore.Mvc;


namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
            => _usersService = usersService;

        [HttpGet]
        public async Task<ActionResult<List<UsersResponce>>> GetAllUsers()
        {
            var users = await _usersService.GetAllUsersAsync();

            var responce = users.Select(u 
                => new UsersResponce(u.Id, u.FirstName, u.LastName, u.AccountCount, u.MiddleName));

            return Ok(responce);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsersResponce>> GetUserById(int id)
        {
            var user = await _usersService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound($"Пользователь с ID {id} не найден");
            }

            var responce = new UsersResponce
                (user.Id,
                 user.FirstName,
                 user.LastName,
                 user.AccountCount,
                 user.LastName);

            return Ok(responce);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateUser([FromBody] UsersRequest request)
        {
            var (user, error) = FinanceApp.Core.Models.User.Create(
                RandomInteger.GenerateRandomPositiveInt(),
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.AccountCount,
                request.MiddleName
                );

            if (!String.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            return Ok(await _usersService.CreateUserAsync(user));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> UpdateUser(int id, [FromBody] UsersRequest request)
        {
            return Ok(await _usersService.UpdateUserAsync
                ( id
                , request.FirstName
                , request.LastName
                , /*request.Email,*/
                  request.Password
                , request.AccountCount
                , request.MiddleName));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> DeleteUser(int id)
            => Ok(await _usersService.DeleteUserAsync(id));
    }
}
