namespace FinanceApp.Api.Contracts
{
    public record TransfersResponce
    (
        int TransferId,
        int AccountId,
        int AppointmentAccountId,
        string CurrencyType,
        DateTime TransferDate,
        decimal TransferSum
    );
}
