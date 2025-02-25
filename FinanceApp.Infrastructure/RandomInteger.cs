using System.Security.Cryptography;

namespace FinanceApp.Infrastructure
{
    public class RandomInteger
    {

        /// <summary>
        /// Генерирует случайное положительное целое число (int)
        /// </summary>
        /// <returns>Случайное положительное целое число</returns>
        public static int GenerateRandomPositiveInt()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);

                int randomInt = BitConverter.ToInt32(bytes, 0);

                return Math.Abs(randomInt);
            }
        }

        /// <summary>
        /// Генерирует случайное положительное целое число (int) в заданном диапазоне (от minInclusive до maxExclusive)
        /// </summary>
        /// <param name="minInclusive">Минимальное включительное значение</param>
        /// <param name="maxExclusive">Максимальное исключительное значение</param>
        /// <returns>Случайное положительное целое число в заданном диапазоне</returns>
        /// <exception cref="ArgumentException">Если minInclusive больше или равно maxExclusive</exception>
        /// <exception cref="ArgumentOutOfRangeException">Если minInclusive или maxExclusive меньше 0</exception>
        public static int GenerateRandomPositiveInt(int minInclusive, int maxExclusive)
        {
            if (minInclusive < 0 || maxExclusive < 0)
            {
                throw new ArgumentOutOfRangeException("minInclusive и maxExclusive должны быть неотрицательными.");
            }

            if (minInclusive >= maxExclusive)
            {
                throw new ArgumentException("minInclusive должен быть меньше maxExclusive.");
            }

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                long range = (long)maxExclusive - minInclusive;

                long maxValueMultiple = (long)int.MaxValue / range * range;

                int randomNumber;
                do
                {
                    randomNumber = GenerateRandomPositiveInt();
                }
                while (randomNumber > maxValueMultiple);

                return (int)(randomNumber % range) + minInclusive;
            }
        }
    }
}
