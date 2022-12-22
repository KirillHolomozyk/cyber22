using System.Security.Cryptography;
using System;
using System.IO;
using System.Text;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Виберіть завдання зі списку:\n");
        Console.WriteLine("Прочитаты вміст файлу -> 1\n");
        Console.WriteLine("Зашифрувати файл і помістити у папку з розширенням *.dat -> 2\n");
        Console.WriteLine("Розшифрувати зашифрований файл -> 3\n");
        int number = Convert.ToInt32(Console.ReadLine());
        string path_toFile = "/Users/Kirill/Projects/practices/practice2/encrypted_sample.dat";
        byte[] readContent = File.ReadAllBytes("/Users/Kirill/Projects/practices/practice2/file_sample.txt").ToArray();

        switch (number)
        {
            case 1:
                Console.WriteLine(Encoding.UTF8.GetString(readContent));
                break;
            case 2:
                var encryption = new XOR_program();
                Console.WriteLine("Введіть пароль для шифрування: ");
                string password = Console.ReadLine();
                Console.WriteLine("\n");
                byte[] passwordForEnc = Encoding.UTF8.GetBytes(password);
                var encryptedProcess = encryption.Encryption(readContent, passwordForEnc);
                Console.WriteLine("Записано у файл encrypted_sample.dat\n");              
                File.WriteAllBytes(path_toFile, encryptedProcess);
                break;
            case 3:
                byte[] encryptContent = File.ReadAllBytes(path_toFile).ToArray();
                var decryption = new XOR_program();
                Console.WriteLine("Введіть пароль для розшифровування: ");
                string password_d = Console.ReadLine();
                Console.WriteLine("\n");
                byte[] passwordDecryption = Encoding.UTF8.GetBytes(password_d);
                var decryptedProcess = decryption.Decryption(encryptContent, passwordDecryption);
                Console.WriteLine("чекаємо...\n");
                Console.WriteLine(Encoding.UTF8.GetString(decryptedProcess));
                Console.WriteLine("\n");
                Console.WriteLine("Розшифровування завершене");
                break;
        }



    }


}

public class XOR_program
{
    private byte[] GetSecretKey(byte[] key, byte[] array)
    {
        byte[] secret = new byte[array.Length];
        for(int i = 0; i<secret.Length; i++)
        {
            secret[i] = key[i % key.Length];
        }
        return secret;
    }
    private byte[] XoR(byte[] text, byte[] pas)
    {
        byte[] secretKey = GetSecretKey(pas, text);
        int array_size = text.Length;
        for (int i = 0; i < text.Length; i++)
        {
            text[i] = (byte)(text[i] ^ secretKey[i]);
        }
        return text;
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


