namespace FinanceApp.Api.Contracts
{
    public record UsersResponce
        (
           int Id, 
           string FirstName, 
           string LastName, 
           int AccountCount, 
           string? MiddleName
        );   
}
