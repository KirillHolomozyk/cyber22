using System;
using System.Security.Cryptography;

namespace ConsoleApp11
{
    public class SaltedHash
    {
        public static byte[] GenerateSalt()
        {
            const int saltLength = 32;
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[saltLength];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length]; //компонуємо хеш та сіль в один массив
            
            Buffer.BlockCopy(first, 0, ret, 0, first.Length); //хешований пароль
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);//хешована сіль
            return ret;
        }

       
        public static byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt, int iterationsCount)
        {
            using (var sha512 = SHA512.Create())
            {
                byte[] hash = sha512.ComputeHash(Combine(toBeHashed, salt));
                for (int i = 0; i < iterationsCount; i++)
                {
                    hash = sha512.ComputeHash(Combine(toBeHashed, salt));
                }

                return hash;
            }
        }
    }
}