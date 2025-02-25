using System.Runtime.CompilerServices;

namespace FinanceApp.Core.Models
{
    public class User
    {
        private const int MAX_SYMBOLS = 255;

        private const int MAX_ACCOUNT_COUNT = 5;

        //Просто конструктор с большим количеством переменных
        private User(int id
            , string firstName
            , string lastName
            , string email
            , string password
            , int accountCount
            , string? middleName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Email = email;
            Password = password;
            AccountCount = accountCount;
        }

        public int Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string? MiddleName { get; }

        public string Email { get; }

        public string Password { get; }

        public int AccountCount { get; }

        //Метод по созданию экземпляра класса с использованием ранее объявленного конструктора
        public static (User User, string Error) Create(
              int id
            , string firstName
            , string lastName
            , string email
            , string password
            , int accountCount = 0
            , string? middleName = null)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(firstName)
                || string.IsNullOrEmpty(lastName)
                || string.IsNullOrEmpty(email)
                || string.IsNullOrEmpty(password))
                error = "Одно из обязательных полей не указано";

            //if (firstName.Length > MAX_SYMBOLS
            //    || lastName.Length > MAX_SYMBOLS
            //    || middleName.Length > MAX_SYMBOLS
            //    || email.Length > MAX_SYMBOLS
            //    || password.Length > MAX_SYMBOLS)
            //    error = "Превышено количество символов в одной из строк";

            if (accountCount > MAX_ACCOUNT_COUNT || accountCount < 0)
                error = "Недопустимое количество лицевых счетов";

            var user = new User(id, firstName, lastName, email, password, accountCount, middleName);

            return(user, error);
        }
    }
}

