using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace practice11_12
{
    static class userDataManager
    {
        // наш словник для вже зареэстрованих користувачів
        private static Dictionary<string, User> _users = new Dictionary<string, User>();

        private const int countOfHash = 10000;


       //передаємо логіни і паролі, як константи методу
        public static User Register(string userName, string password, string[] roles = null)
        {
            
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            // генеруємо сіль
            byte[] saltBytes = PBKDF2.GenerateSalt();
            
            // використовуюємо байти з хешем паролів з сіллю
            byte[] passwordHash = SaltedHash.HashPasswordWithSalt(passwordBytes, saltBytes, countOfHash);
            string passwordHashString = Convert.ToBase64String(passwordHash);

            //первірка: якщо юзера раніше не створювалось, то створюємо
            if (!_users.ContainsKey(userName))
            {
                Console.WriteLine("Реєстрацiя виконана успiшно.");
                var newUser = new User
                {
                    Login = userName,
                    PasswordHash = passwordHashString,
                    Salt = saltBytes,
                    Roles = roles
                };
                _users.Add(userName, newUser);
                return newUser;
            }

            Console.WriteLine("Дані повторюються.");
            return null;
        }


        
        public static bool LogIn(string userName, string password) //тут автентифікуємо користувачів
        {
            
            if (CheckPassword(userName, password)) // перевіряємо пароль
            {
                var identity = new GenericIdentity(userName, "OIBAuth"); //створюємо профіль користувача

               
                var principal = new GenericPrincipal(identity, _users[userName].Roles);//відводимо користувачів до певних ролей

                
                Thread.CurrentPrincipal = principal;

                return true;
            }

            return false; //користувач з його ролями відаводиться до певного потоку
        }


        
        private static bool CheckPassword(string userName, string password) //працюємо з паролем, спочатку перевіряємо його
        {
            
            if (!_users.ContainsKey(userName))    //перевірка логіну, якщо не однакові, то відміняємо
            {
                Console.WriteLine("Такого логiну не iснує!");
                return false;
            }

            
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password); //робимо з байтів строку
            
            byte[] saltBytes = _users[userName].Salt;
            byte[] passwordHash = SaltedHash.HashPasswordWithSalt(passwordBytes, saltBytes, countOfHash);
            
            string passwordHashString = Convert.ToBase64String(passwordHash);     //повертаємо пароль з вигляду байтів у вигляд строки


            return _users[userName].PasswordHash == passwordHashString;    //порівнюємо паролі та чи вони співпадають
        }
    }
}