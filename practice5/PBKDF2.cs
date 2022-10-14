using System.Security.Cryptography;

namespace practice5
{
    public class PBKDF2
    {
        public static byte[] GenerateSalt()
        {
            using var  NumGeneration = new RNGCryptoServiceProvider();
            var ranNum = new byte[32];
            NumGeneration.GetBytes(ranNum);
            return ranNum;
        }

        public static byte[] HashPassword(byte[] toBeHashed, byte[] salt, int ronNum, HashAlgorithmName hashType)
        {
            using var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, ronNum, hashType);
            int bLength = 0;
            if (hashType == HashAlgorithmName.SHA1)
                bLength = 20;
            else if (hashType == HashAlgorithmName.SHA256)
                bLength = 32;
            else if (hashType == HashAlgorithmName.SHA384)
                bLength = 48;
            else if (hashType == HashAlgorithmName.SHA512)
                bLength = 64;
            return rfc2898.GetBytes(bLength);
        }
    }
}    