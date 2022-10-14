using System;
using System.Collections.Generic;
using System.Text;

namespace practice5
{
    public class LoginApp
    {
        const int itColl = 8 * 10000;
        private static Dictionary<string, User> users = new Dictionary<string, User>();


        public static void Register()
        {
            Console.WriteLine("Введiть логiн: ");
            string login = Console.ReadLine();
            Console.WriteLine("Введiть пароль: ");
            string password = Console.ReadLine();

            
            byte[] bytePass = Encoding.UTF8.GetBytes(password);
            byte[] byteSalt = PBKDF2.GenerateSalt();
            byte[] hashPass = SaltedHash.HashPasswordWithSalt(bytePass, byteSalt, itColl);

            
            if (!users.ContainsKey(login))
            {
                Console.WriteLine("Реєстрацiя виконана успiшно");
                User newUser = new User(login, hashPass, byteSalt);
                users.Add(login, newUser);
            }
            else
                Console.WriteLine("Такого користувача не iснує");
        }

        public static void Login()
        {
            Console.Write("Введiть логiн: ");
            string login = Console.ReadLine();
            Console.Write("Введiть пароль: ");
            string password = Console.ReadLine();
            
            if (users.ContainsKey(login) == false)
                Console.WriteLine("Такого користувача не існує");
            else
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltBytes = users[login].Salt;
                byte[] passHash = SaltedHash.HashPasswordWithSalt(passwordBytes, saltBytes, itColl);
                string passInStr = Convert.ToBase64String(passHash);
                
                if (users[login].Password == passInStr)
                    Console.WriteLine("Ви увiйшли");
                else
                    Console.WriteLine("Невірний пароль");
            }
        }
    }
}