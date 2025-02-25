namespace FinanceApp.Api.Contracts
{
    public record AccountsResponce
        (
            int Id,
            string CurrencyType,
            decimal Balance
        );
}
