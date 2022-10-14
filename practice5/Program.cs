using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace practice5
{
    class MENU
    {
        public const int var = 8 * 10000;

        static void Main(string[] args)
        {
            Console.WriteLine("Введiть номер вашого завдання(3-5):");
            char menu = Console.ReadKey().KeyChar;
            Console.WriteLine();
            Console.WriteLine();
            switch (menu)
            {
                case '3':
                    Exc3();
                    break;
                case '4':
                    Console.WriteLine("Виберiть алгоритм хешування:\nSHA1->1\nSHA256->2\nSHA384->3\nSHA512->4");
                    char typeOfHash = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                    Exc4(typeOfHash);
                    break;
                case '5':
                    Exc5();
                    break;
            }
            Console.WriteLine();

        }

        private static void Exc3()
        {
            Console.WriteLine("Введiть ваш пароль ->");
            string password = Console.ReadLine();
            byte[] salt = SaltedHash.GenerateSalt();
            Console.WriteLine("Ваш пароль: " + password);
            Console.WriteLine("Сiль, яку ми додамо до паролю: " + Convert.ToBase64String(salt));
            Console.WriteLine();
            var firstHashedPass = SaltedHash.HashPasswordWithSalt(
                Encoding.UTF8.GetBytes(password), salt);
            Console.WriteLine("Ваш хешований пароль: " + Convert.ToBase64String(firstHashedPass));
        }

        private static void Exc4(char typeOfHash)
        {
            Console.WriteLine("Введiть ваш пароль ->");
            string password = Console.ReadLine();
            int iterations = var;
            HashAlgorithmName algorithmType;
            switch (typeOfHash)
            {
                case '1':
                    algorithmType = HashAlgorithmName.SHA1;
                    break;
                case '2':
                    algorithmType = HashAlgorithmName.SHA256;
                    break;
                case '3':
                    algorithmType = HashAlgorithmName.SHA384;
                    break;
                case '4':
                    algorithmType = HashAlgorithmName.SHA512;
                    break;
                default:
                    Console.WriteLine("Будь ласка, оберiть корректне значення");
                    return;
            }

            Stopwatch timer = new Stopwatch();
            timer.Start();

            for (int i = 0; i < 10; i++)
            {
                var hashedPassword = PBKDF2.HashPassword(
                    Encoding.UTF8.GetBytes(password),
                    PBKDF2.GenerateSalt(), iterations,
                    algorithmType);
                Console.WriteLine();
                Console.WriteLine("Ваш пароль: " + password);
                Console.WriteLine("Пароль у виглядi хешу: " + Convert.ToBase64String(hashedPassword));
                Console.WriteLine($"Витрачено часу: {timer.ElapsedMilliseconds}ms\n" +
                                  $"Виконано iтерацiй: {iterations}");
                iterations += 50000;
            }

            timer.Stop();
            Console.ReadKey();
        }

        private static void Exc5()
        {
            while (true)
            {
                Console.WriteLine($"Реєстрацiя -> 1\n" +
                              $"Вхiд в аккаунт -> 2");
                char operation = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (operation)
                {
                    case '1':
                    {
                    Console.WriteLine("Реєстрацiю розпочато: ");
                        LoginApp.Register();
                        break;
                    }
                    case '2':
                    {
                        Console.WriteLine("Вхiд в систему:");
                        LoginApp.Login();
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Введiть правильне значення");
                        return;
                    }
                }
            }
        }
    }
}