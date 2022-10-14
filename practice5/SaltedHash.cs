using System;
using System.Security.Cryptography;

namespace practice5
{
    public class SaltedHash
    {
        public static byte[] GenerateSalt()
        {
            const int saltLength = 19;
            using var randomNumberGenerator = new RNGCryptoServiceProvider();
            var randomNumber = new byte[saltLength];
            randomNumberGenerator.GetBytes(randomNumber);
            return randomNumber;
        }

        public static byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Combine(toBeHashed, salt));
        }

        public static byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt, int countIter)
        {
            using var sha256 = SHA512.Create();
            byte[] sh = sha256.ComputeHash(Combine(toBeHashed, salt));
            for (int i = 0; i < countIter; i++)
                sh = sha256.ComputeHash(Combine(sh, salt));
            return sh;
        }
   
        private static byte[] Combine(byte[] first, byte[] second)
        {
            var result = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, result, 0, first.Length);
            Buffer.BlockCopy(second, 0, result, first.Length, second.Length);
            return result;
        }
    }
}