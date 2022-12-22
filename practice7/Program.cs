using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp7
{
    class Program
    {
        static string forAllKey = "public.xml";


        private static void
            Main(string[] args) //розставили номера завдань по завданням, тому що вони будуть одразу запускатись
        {
            Console.WriteLine("\n __1__ \n");
            First();

            Console.WriteLine("\n___2___\n");
            Second();

            Console.WriteLine("\n ___3___ \n");

            createNewPubKey();

            Console.WriteLine("\n__________________________________________________\n");

            encMess();

            Console.WriteLine("\n__________________________________________________\n");

            while (true)
            {
                decMess();

                Console.WriteLine("\n__________________________________________________\n");
            }
        }


        private static void First()
        {

            Console.Write("Введiть ваш текст: ");
            string sourceStr = Console.ReadLine();
            byte[] sourceBytes = Encoding.UTF8.GetBytes(sourceStr);

      
            RSAChipher.AssignNewKey();//ініціюємо створення нових ключів
            byte[] encInfo = RSAChipher.EncryptData(sourceBytes);
            Console.Write("Зашифрована iнформацiя: " + Convert.ToBase64String(encInfo));
            Console.WriteLine("\n___");

            byte[] decryptInfo = RSAChipher.DecryptData(encInfo);
            string decryptStr = Encoding.UTF8.GetString(decryptInfo);

            Console.WriteLine("Розшифроване повiдомлення: " + decryptStr);

            RSAChipher.DeleteKeyInCsp();//видаляємо ключі з памяті
        }


        private static void Second()
        {
            RSAChipher.AssignNewKey(forAllKey);//ПЕРЕГРУЗКА

            Console.Write("Введiть текст: ");
            string firstStr = Console.ReadLine();
            byte[] firstBytes = Encoding.UTF8.GetBytes(firstStr);

            byte[] encInfo = RSAChipher.EncryptData(firstBytes);

            byte[] decrInfo = RSAChipher.DecryptData(encInfo);
            string decStr = Encoding.UTF8.GetString(decrInfo);

            Console.WriteLine("Результат: " + Convert.ToBase64String(encInfo));
            Console.WriteLine("Розшифрована iнформацiя: " + decStr);

            RSAChipher.DeleteKeyInCsp();
        }


        private static void createNewPubKey()
        {
            Console.Write("Де зберiгаємо публiчний ключ: ");
            string publicKeyPath = Console.ReadLine();

            RSAChipher.GenerateOwnKeys(publicKeyPath);
            Console.WriteLine("Ключi успiшно збереженi.");
        }


        private static void encMess()//МЕТОД, ЩО ШИФРУЄ ТА ЗБЕРІГАЄ ПОВІДОМЛЕННЯ
        {
            Console.Write("Шлях до публiчного ключа шифрування: ");
            string enemyPublicKeyPath = Console.ReadLine();

            Console.Write("Iнформацiя для шифрування: ");
            string sourceString = Console.ReadLine();
            byte[] sourceBytes = Encoding.UTF8.GetBytes(sourceString);

            Console.Write("Шлях зберiгання зашифрованих данних: ");
            string secretMessagePath = Console.ReadLine();
            //(КЛЮЧ, ІНФА ДЛЯ ШИФРУВАННЯ, ШЛЯХ КУДИ ЗБЕРЕЖЕМО ЗАШИФРОВАНІ ДАНІ)
            RSAChipher.EncryptData(enemyPublicKeyPath, sourceBytes, secretMessagePath);

            Console.WriteLine("Данi успiшно зашифрованi та збереженi.");
        }


        private static void decMess()//МЕТОД, ЩО ДЕШИФРУЄ 
        {
            Console.Write("Шлях до зашифрованих данних: ");
            string encryptedDataPath = Console.ReadLine();

            byte[] secretBytes = File.ReadAllBytes(encryptedDataPath);
            byte[] decryptedData = RSAChipher.DecryptData(encryptedDataPath);

            Console.WriteLine("Зашифрованi данi для перевiрки: " + Convert.ToBase64String(secretBytes));
            Console.WriteLine("Вашi розшифрованi данi: " + Encoding.UTF8.GetString(decryptedData));
        }
    }

    public class RSAChipher
    {

       

        // XML-Based Keys
        public static void AssignNewKey(string publicKeyPath)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = true;
                File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
            }
        }

        private static readonly string CspContainerName = "RsaContainer";

        // Cryptographic Service Provider
        public static void AssignNewKey()
        {
            var cspParameters = new CspParameters(1)
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore, //Рівень пристрою
                ProviderName = "Microsoft Strong Cryptographic Provider"
            };

            using (var rsa = new RSACryptoServiceProvider(cspParameters))
            {
                rsa.PersistKeyInCsp = true;
            }
        }

        public static byte[] EncryptData(byte[] dataToEncrypt)
        {
            byte[] cypherBytes;
            var cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            using (var rsa = new RSACryptoServiceProvider(cspParams))
            {
                rsa.PersistKeyInCsp = true;
                // rsa.ImportParameters(_publicKey);
                cypherBytes = rsa.Encrypt(dataToEncrypt, true);
            }

            return cypherBytes;
        }

        public static void EncryptData(string publicKeyPath, byte[] dataToEncrypt, string chipherTextPath)
        {
            byte[] chipherBytes;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(publicKeyPath));
                chipherBytes = rsa.Encrypt(dataToEncrypt, true);
            }

            File.WriteAllBytes(chipherTextPath, chipherBytes);
        }


        public static byte[] DecryptData(byte[] dataToDecrypt)
        {
            byte[] plainBytes;
            CspParameters cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            using (var rsa = new RSACryptoServiceProvider(cspParams))
            {
                rsa.PersistKeyInCsp = true;
                // rsa.ImportParameters(_privateKey);
                plainBytes = rsa.Decrypt(dataToDecrypt, true);
            }

            return plainBytes;
        }

        public static byte[] DecryptData(string chipherTextPath)
        {
            byte[] chipherBytes = File.ReadAllBytes(chipherTextPath);
            byte[] plainTextBytes;
            var cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
            {
                rsa.PersistKeyInCsp = true;
                plainTextBytes = rsa.Decrypt(chipherBytes, true);
            }

            return plainTextBytes;
        }

        // Щоб видалити ключі із сховища
        public static void DeleteKeyInCsp()
        {
            CspParameters cspParameters = new CspParameters
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            var rsa = new RSACryptoServiceProvider(cspParameters)
            {
                PersistKeyInCsp = false
            };
            rsa.Clear();
        }

        public static void GenerateOwnKeys(string publicKeyPath)
        {
            CspParameters cspParameters = new CspParameters(1)
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore,
                ProviderName = "Microsoft Strong Cryptographic Provider",
            };
            using (var rsa = new RSACryptoServiceProvider(2048, cspParameters))
            {
                rsa.PersistKeyInCsp = true;
                File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
            }
        }
    }
}