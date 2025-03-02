namespace FinanceApp.Api.Contracts
{
    public record TransfersRequest
        (
            string CurrencyType,
            DateTime TransferDate,
            decimal TransferSum
        );

}
