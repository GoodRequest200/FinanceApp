using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Core.Models
{
    public class Account
    {
        private Account(int id, string currencyType, decimal balance)
        {
            Id = id;
            CurrencyType = currencyType;
            Balance = balance;
        }

        public int Id { get; set; }

        public string CurrencyType { get; set; } = null!;

        public decimal Balance { get; set; }

        //Метод по созданию экземпляра класса с использованием ранее объявленного конструктора
        public static (Account Account, string Error) Create
            (int id, decimal balance, string currencyType = "рубли")
        {
            var error =  string.Empty;

            if (currencyType != "рубли" && currencyType != "у.е.") 
            {
                error = "Некорректный тип валюты(поддерживаются 'рубли' и 'у.е.')";
            } 

            var account = new Account(id, currencyType, balance);

            return (account, error);
        }   
    }
}
