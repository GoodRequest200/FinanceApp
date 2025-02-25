namespace FinanceApp.Api.Contracts
{
    public record AccountsRequest
        (
            string CurrencyType,
            decimal Balance
        );
}
