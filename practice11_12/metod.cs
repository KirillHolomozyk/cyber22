using System.Security.Cryptography;

namespace practice11_12
{
    public class PBKDF2
    {
        public static byte[] GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())//генеруємо сіль
            {
                var randomNumber = new byte[32];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        public static byte[] HashPassword(byte[] toBeHashed, byte[] salt,
            int numberOfRounds, HashAlgorithmName algorithm)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(
                toBeHashed, salt, numberOfRounds, algorithm))
            {
                return rfc2898.GetBytes(20);
            }
        }
    }
}