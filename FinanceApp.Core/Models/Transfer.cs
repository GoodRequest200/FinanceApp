namespace FinanceApp.Core.Models
{
    public class Transfer
    {
        public Transfer(int id, decimal transferSum, DateTime transferDate, string currencyType)
        {
            Id = id;
            TransferSum = transferSum;
            CurrencyType = currencyType;
            TransferDate = transferDate;
        }

        public int Id { get; }

        public string CurrencyType { get; } = null!;

        public DateTime TransferDate { get; }

        public decimal TransferSum { get; }

        //Метод по созданию экземпляра класса с использованием ранее объявленного конструктора
        public static (Transfer Transfer, string Error) Create
            (int id, decimal transferSum, DateTime transferDate, string currencyType = "рубли")
        {
            var error = string.Empty;

            if (transferSum <= 0)
                error = "некорректная сумма перевода";


            if (currencyType != "рубли" && currencyType != "у.е.")
            {
                error = "Некорректный тип валюты(поддерживаются 'рубли' и 'у.е.')";
            }

            var transfer = new Transfer(id, transferSum, transferDate, currencyType);

            return (transfer, error);
        }
    }
}
