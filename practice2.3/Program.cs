using System.Security.Cryptography;
using System;
using System.IO;
using System.Text;

public class Find_Pas
{
    static void Main()
    {

        string res = @"/Users/Kirill/Projects/practices/practice2.3/result.txt";
        byte[] findContent = File.ReadAllBytes(@"/Users/Kirill/Projects/practices/practice2.3/encfile5.dat").ToArray();
        var f_x = new XOR_Program();
        string phrase = "Mit21";
        Console.WriteLine("\n");
        byte[] password_f = Encoding.UTF8.GetBytes(phrase);

        byte[] five_bytes = new byte[5];
        string decoded = "";
        string pass = "";
        for (int i = 0; i < findContent.Length - phrase.Length; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                five_bytes[j] = findContent[j + i];
            }
            var PassBin = f_x.Encryption(five_bytes, password_f);
            var stringBin = f_x.Encryption(findContent, PassBin);
            decoded = Encoding.UTF8.GetString(stringBin);
            if (decoded.Contains(" Mit21 "))
            {
                File.AppendAllText(res, "PASSWORD: " + Encoding.UTF8.GetString(PassBin) + "\n");
                File.AppendAllText(res, "CONTENT: " + decoded + "\n");
                File.AppendAllText(res, "-------------------------------");
            }

        }
    }


    public class XOR_Program
    {
        private byte[] GetSecretKey(byte[] key, byte[] array)
        {
            byte[] secret = new byte[array.Length];
            for (int i = 0; i < secret.Length; i++)
            {
                secret[i] = key[i % key.Length];
            }
            return secret;
        }

        private byte[] XoR(byte[] text, byte[] pas)
        {
            byte[] secretKey = GetSecretKey(pas, text);
            int array_size = text.Length;
            byte[] results = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                results[i] = (byte)(text[i] ^ secretKey[i]);
            }
            return results;
        }

        public byte[] Encryption(byte[] ourText, byte[] ourPas)
        {
            return XoR(ourText, ourPas);
        }

        public byte[] Decryption(byte[] encryptedText, byte[] ourPas)
        {
            return XoR(encryptedText, ourPas);
        }
    }
}