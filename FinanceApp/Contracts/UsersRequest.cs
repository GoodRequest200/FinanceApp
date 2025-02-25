namespace FinanceApp.Api.Contracts
{
    public record UsersRequest
        (           
            string LastName,
            string FirstName,
            string? MiddleName,
            string Email,
            string Password,
            int AccountCount           
        );
}
