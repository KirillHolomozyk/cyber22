using System;
using System.Security.Cryptography;
using System.Text;
//Хешуємо текст за різними алгоритмами для завдання 1 
//Так званий файл-двигун, до якого ми звертаємося для того, щоб хешувати інформацію 
namespace practice3_4
{
    public static class hashmade
    {
        

        public static string GMD5(string data)
        {
            using (var crypto = MD5.Create())
            {
                byte[] byt = Encoding.Unicode.GetBytes(data);
                return Convert.ToBase64String(crypto.ComputeHash(byt));
            }
        }

        public static string GSHA1(string data)
        {
            using (var crypto = SHA1.Create())
            {
                byte[] byt = Encoding.Unicode.GetBytes(data);
                return Convert.ToBase64String(crypto.ComputeHash(byt));
            }
        }

        public static string GSHA256(string data)
        {
            using (var crypto = SHA256.Create())
            {
                byte[] byt = Encoding.Unicode.GetBytes(data);
                return Convert.ToBase64String(crypto.ComputeHash(byt));
            }
        }

        public static string GSHA512(string data)
        {
            using (var crypto = SHA512.Create())
            {
                byte[] byt = Encoding.Unicode.GetBytes(data);
                return Convert.ToBase64String(crypto.ComputeHash(byt));
            }
        }
    }
}