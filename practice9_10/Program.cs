using System;
using System.IO;
using System.Security.Cryptography;



namespace UwpApplication10
{
    class Program
    {
        static void Main(string[] args)
        {
            //наші шляхи збереження
            string docs = "../../../secretData.txt";
            string signaturePath = "../../../secretSQign.bin";
            string publicKeyPath = "../../../publicKey.xml";

            //перевірка чи міститься такий файл, який нам потрібен
            if (File.Exists(docs))
            {
                byte[] fileBytes = File.ReadAllBytes(docs);
                Console.WriteLine("Створити пiдпис-----> 1\nПеревiрити пiдпис-----> 2");//меню
                char option = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (option)
                {
                    case '1': // створюємо підпис
                        {
                            byte[] signature = DigitalSignature.CreateSignature(publicKeyPath, fileBytes);

                            Console.WriteLine("Пiдпис: " + Convert.ToBase64String(signature));
                            File.WriteAllBytes(signaturePath, signature);

                            break;
                        }

                    case '2':
                        //перевіряємо підпис
                        {
                            if (File.Exists(signaturePath))
                            {
                                byte[] signature = File.ReadAllBytes(signaturePath);
                                bool isVerified = DigitalSignature.VerifySignature(publicKeyPath, fileBytes, signature);

                                Console.WriteLine(isVerified ? "Пiдпис перевiрено" : "Пiдпис не перевiрено.");
                            }
                            else Console.WriteLine("Файл не знайдено    ");

                            break;
                        }
                }
            }
            else Console.WriteLine("Файл не знайдено");
        }
    }


    //Тут створюємо підпис та його перевіряємо
    static class DigitalSignature
    {
        private static readonly string CspContainerName = "RsaContainer";


        public static byte[] CreateSignature(string publicKeyPath, byte[] data)
        {
            // Initialization a new instance of the Cryptographic service provider Parameters
            var cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };



            using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
            {
                // Using private key stored in the key container
                rsa.PersistKeyInCsp = true;
                File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));

                // Creating an RSASignatureFormatter object and passing it the
                // RSACryptoServiceProvider to transfer the key information.
                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);

                //Set the hash algorithm to SHA512.
                rsaFormatter.SetHashAlgorithm(nameof(SHA512));

                byte[] hashOfData = GetHash(data);
                return rsaFormatter.CreateSignature(hashOfData);
            }
        }


        public static bool VerifySignature(string publicKeyPath, byte[] data, byte[] signature)
        {
            // Creating a new instanceof the RSA Cryptographic  Service Provider
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                // Using the public key stored in the XML-file
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(publicKeyPath));

                // Creating an RSAPKCS1SignatureDeformatterobject and pass it the 
                // RSACryptoServiceProvider to transfer the key information.
                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);

                //Set the hash algorithm to SHA512.
                rsaDeformatter.SetHashAlgorithm(nameof(SHA512));

                byte[] hashOfData = GetHash(data);
                return rsaDeformatter.VerifySignature(hashOfData, signature);
            }
        }


        public static byte[] GetHash(byte[] data)
        {
            byte[] hashOfData;

            using (var sha512 = SHA512.Create())
            {
                // The hash to sign.
                hashOfData = sha512.ComputeHash(data);
            }

            return hashOfData;
        }
    }
}