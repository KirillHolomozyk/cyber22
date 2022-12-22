using System;
using System.Threading;

namespace ConsoleApp11
{
    class Program
    {
        static void Main(string[] args)
        {
          
            Register();
            LogIn();
            menuOfUsers();
        }


        static void Register()
        {
            Console.Write("Введiть ваш логiн: ");
            string login = Console.ReadLine();
            Console.Write("Введiть ваш пароль: ");
            string password = Console.ReadLine();

            Console.Write("Вкажiть вашу роль: ");
            string userRole = Console.ReadLine();
            var roles = new string[] {userRole};    //Наш масив з ролями

            // реєструємо користувачів
            var user = userDataManager.Register(login, password, roles);
            if (user == null)
            {
                Console.WriteLine("Ви не зареєстровані");
            }
            else
            {
                Console.WriteLine($"Ваша роль: {user.Roles[0]}");
            }
        }


        static void LogIn()
        {
            Console.Write("login: ");
            string login = Console.ReadLine();
            Console.Write("password: ");
            string password = Console.ReadLine();

            if (userDataManager.LogIn(login, password))
            {
                Console.WriteLine("Вiтаю, ви зареєстрованi!");
            }
            else
            {
                Console.WriteLine("Ви не зареєстрованi!");
            }
        }


        static void menuOfUsers()
        {
            if (Thread.CurrentPrincipal == null)
            {
                Console.WriteLine("Будь ласка, залогiнтесь.");
            }
            else
            {
                // перевіряємо чи належить юзер до якоїсь ролі
                if (Thread.CurrentPrincipal.IsInRole("boss"))
                {
                    Console.WriteLine("You are Boss!");
                }
                else if (Thread.CurrentPrincipal.IsInRole("clerk"))
                {
                    Console.WriteLine("You are Clerk");
                }
                else if (Thread.CurrentPrincipal.IsInRole("manager"))
                {
                    Console.Write("Напишiть ваше повiдомлення для персоналу: ");
                    string msg = Console.ReadLine();
                    Console.Write("Менеджер постановив: " + msg);
                }
                else
                {
                    Console.WriteLine("Ви новий юзер!");
                }
            }
        }
    }
}