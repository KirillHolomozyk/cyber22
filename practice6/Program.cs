using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;


namespace practice6 {

    class Program
    {
        public static int variant = 8000;
        static void Main(string[] args)
        {
            string task;

            while (true)
            {
                Console.WriteLine("Оберіть номер вашого завдання:\nПерше завдання -> 1\nДруге завдання -> 2");
                do { task = Console.ReadLine(); }
                while ((task != "1") && (task != "2"));

                int intTaskNum = Convert.ToInt32(task);
                Console.Write("\n");

                switch (intTaskNum)
                {
                    case 1:
                        Task1();
                        break;

                    case 2:
                        Task2();
                        break;
                }
            }
        }


        public static void Task1()
        {
            var aesCh = new AesChipher();
            var desCh = new desChipher();
            var desTr = new trippleDES();//Це наші алгоритми
            var key_aes = aesCh.GenerateRandomNumbers(32);
            var iv_aes = aesCh.GenerateRandomNumbers(16);
            var key_des = desCh.GenerateRandomNumber(8);
            var iv_des = desCh.GenerateRandomNumber(8);
            var key_tdes = desTr.GenerateRandomNumber(24);
            var iv_tdes = desTr.GenerateRandomNumber(8);//це наші ключі відповідних розмірів

            //вводимо пароль
            Console.Write("Введiть, будь ласка, пароль: ");
            string firstData = Console.ReadLine();
            byte[] bytePass = Encoding.ASCII.GetBytes(firstData);//перетворюємо в байтовий вигляд, тому що тільки так можемо шифрувати
            //Це наше АЕС шифрування
            //шифруємо використовуючи повідомлення, ключ, вектор ініціалізації
            var aesEnc = aesCh.Encrypt(bytePass, key_aes, iv_aes);//тут: текст для шифрування, ключ, вектор ініціалізації
            var desEnc = aesCh.Decrypt(aesEnc, key_aes, iv_aes);//розшифрували
            var aesDecMes = Encoding.UTF8.GetString(desEnc);//повертаємо наші байти у вигляд тексту
            Console.WriteLine("\nAES-шифрування:");
            Console.WriteLine("Що шифруємо: " + firstData);
            Console.WriteLine("Зашифрований текст: " + Convert.ToBase64String(aesEnc));
            Console.WriteLine("Розшифрований текст " + aesDecMes + "\n");//розшифроване повідомлення
            //Це ДЕС шифрування________________________________________________________________________________________
            //шифруємо використовуючи повідомлення, ключ, вектор ініціалізації
            var encrypted_des = desCh.Encrypt(bytePass, key_des, iv_des);
            //розшифровуємо зашифроване повідомлення для перевірки
            var decrypted_des = desCh.Decrypt(encrypted_des, key_des, iv_des);
            //перетворюємо в стрічку розшифроване повідомлення
            var decryptedMessage_des = Encoding.UTF8.GetString(decrypted_des);
            Console.WriteLine("DES-шифрування");
            Console.WriteLine("Що шифруємо: " + firstData);//вивели вхідне повідомлення
            Console.WriteLine("Зашифрований текст: " + Convert.ToBase64String(encrypted_des));//виводимо зашифроване повідомлення
            Console.WriteLine("Розшифрований текст: " + decryptedMessage_des + "\n\n");//виводимо розшифроване повідомлення


            var encrypted_tdes = desTr.Encrypt(bytePass, key_tdes, iv_tdes);//тут: текст для шифрування, ключ, вектор ініціалізації

            var decrypted_tdes = desTr.Decrypt(encrypted_tdes, key_tdes, iv_tdes);//розшифрували в байти
            //перетворюємо в стрічку розшифроване повідомлення
            var decryptedMessage_tdes = Encoding.UTF8.GetString(decrypted_tdes);
            Console.WriteLine("Triple DES-шифрування");
            Console.WriteLine("Що шифруємо: " + firstData);// вхідне повідомлення
            Console.WriteLine("Зашифрований текст: " + Convert.ToBase64String(encrypted_tdes));// зашифроване повідомлення
            Console.WriteLine("Розшифрований текст: " + decryptedMessage_tdes);// розшифроване повідомлення
            Console.ReadKey();//ЧЕКАЄМО, ПОКИ НАТИСНУТЬ


        }
        public static void Task2()
        {
            var aesCh = new AesChipher();
            var desCh = new desChipher();
            var trDes = new trippleDES();//знову повторюємо наші змінні з алгорритмами шифрування

            //вводимо пароль
            Console.Write("Введiть, будь ласка, ваш пароль: ");
            string firstData = Console.ReadLine();
            byte[] ori_bytes = Encoding.ASCII.GetBytes(firstData);//знову в масив байтів


            byte[] a_key = PBKDF2.Generator(ori_bytes, 32);
            byte[] a_iv = PBKDF2.Generator(ori_bytes, 16);
            byte[] d_key = PBKDF2.Generator(ori_bytes, 8);
            byte[] d_iv = PBKDF2.Generator(ori_bytes, 8);
            byte[] key_tdes = PBKDF2.Generator(ori_bytes, 24);
            byte[] iv_tdes = PBKDF2.Generator(ori_bytes, 8);//знову ключі відповідних розмірів

            var encrypted_AES = aesCh.Encrypt(ori_bytes, a_key, a_iv);//тут: повідомлення, ключ, вектор ініціалізації

            var decrypted_DES = aesCh.Decrypt(encrypted_AES, a_key, a_iv);
            //перетворюємо в стрічку розшифроване повідомлення
            var decMes_DES = Encoding.UTF8.GetString(decrypted_DES);
            Console.WriteLine("\nAES-шифрування");
            Console.WriteLine("Що шифруємо: " + firstData);
            Console.WriteLine("Зашифрований текст: " + Convert.ToBase64String(encrypted_AES));//виводимо зашифроване повідомлення
            Console.WriteLine("Розшифрований текст: " + decMes_DES + "\n\n");//виводимо розшифроване повідомлення

            var encrypted_des = desCh.Encrypt(ori_bytes, d_key, d_iv);//повідомлення, ключ, вектор ініціалізації

            var decrypted_des = desCh.Decrypt(encrypted_des, d_key, d_iv);//розшифровуємо
            var decryptedMessage_des = Encoding.UTF8.GetString(decrypted_des);//з байтів в букви
            Console.WriteLine("DES-шифрування");
            Console.WriteLine("Що шифруємо" + firstData);
            Console.WriteLine("Зашифрований текст: " + Convert.ToBase64String(encrypted_des));//зашифроване повідомлення
            Console.WriteLine("Розифрований текст: " + decryptedMessage_des + "\n\n");//розшифроване повідомлення


            var encrypted_tdes = trDes.Encrypt(ori_bytes, key_tdes, iv_tdes);//повідомлення, ключ, вектор ініціалізації
            var decrypted_tdes = trDes.Decrypt(encrypted_tdes, key_tdes, iv_tdes);
            var decryptedMessage_tdes = Encoding.UTF8.GetString(decrypted_tdes);//знову з байтів в букви
            Console.WriteLine("Triple DES-шифрування: ");
            Console.WriteLine("Що шифруємо: " + firstData);//вхідне повідомлення
            Console.WriteLine("Зашифроване повiдомлення: " + Convert.ToBase64String(encrypted_tdes));//зашифроване повідомлення
            Console.WriteLine("Розшифроване повiдомлення: " + decryptedMessage_tdes);//розшифроване повідомлення
            Console.ReadKey();//щоб одразу не виліповідомленнятало
        }

        class AesChipher
        {
            public byte[] GenerateRandomNumbers(int length)
            {
                using (var randomNumberGenerator = new RNGCryptoServiceProvider())//симетрично шифруємо
                {
                    var randomNumber = new byte[length];
                    randomNumberGenerator.GetBytes(randomNumber);
                    return randomNumber;
                }
            }

            public byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
            {
                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;//робимо дописування тексту в кінець
                    aes.Key = key;//ключ
                    aes.IV = iv;//наш вектор
                    using (var memoryStream = new MemoryStream())
                    {
                        var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                        //записуємо байти для обробки, зміщення, довжину масиву
                        cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                        //оновлюємо данні з буфферу і чистимо його
                        cryptoStream.FlushFinalBlock();
                        //повертаємо шифроване повідомлення
                        return memoryStream.ToArray();
                    }

                }
            }
            public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
            {
                //створюємо новий екземпляр AesCryptoServiceProvider()
                using (var aes = new AesCryptoServiceProvider())
                {
                    //задаємо режим симетричного шифрування
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;//режим додавання тексту, для доповнення до блоку
                    aes.Key = key;//ключ
                    aes.IV = iv;//вектор
                    using (var memoryStream = new MemoryStream())
                    {
                        //новий екземпляр потоку криптографічних перетворень
                        var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
                        //записуємо байти для обробки, зміщення, довжину масиву
                        cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();//виводимо дешифровану строку
                    }

                }
            }
        }


        class desChipher
        {
            //генератор ключа заданої довжини
            public byte[] GenerateRandomNumber(int length)
            {
                using (var randomNumberGenerator = new RNGCryptoServiceProvider())
                {
                    var randomNumber = new byte[length];
                    randomNumberGenerator.GetBytes(randomNumber);//вивели випадкові значення в массив заданої довжини
                    return randomNumber;
                }
            }

            public byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
            {
                using (var des = new DESCryptoServiceProvider())//симетричне шифрування
                {
                    des.Mode = CipherMode.CBC;
                    //режим додавання тексту, для доповнення до блоку
                    des.Padding = PaddingMode.Zeros;
                    //ключ
                    des.Key = key;
                    //ініціалізація вектору
                    des.IV = iv;
                    using (var memoryStream = new MemoryStream())
                    {
                        //новий екземпляр потоку криптографічних перетворень
                        var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                        //записуємо байти для обробки, зміщення, довжину масиву
                        cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                        //оновлюємо данні з буфферу і чистимо його
                        cryptoStream.FlushFinalBlock();
                        //повертаємо шифроване повідомлення
                        return memoryStream.ToArray();
                    }
                }
            }

            public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
            {
                using (var des = new DESCryptoServiceProvider())
                {
                    //задаємо режим симетричного шифрування
                    des.Mode = CipherMode.CBC;
                    //режим додавання тексту, для доповнення до блоку
                    des.Padding = PaddingMode.Zeros;
                    //ключ
                    des.Key = key;
                    //ініціалізація вектору
                    des.IV = iv;
                    using (var memoryStream = new MemoryStream())
                    {
                        //новий екземпляр потоку криптографічних перетворень
                        var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                        //записуємо байти для обробки, зміщення, довжину масиву
                        cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                        //оновлюємо данні з буфферу і чистимо його
                        cryptoStream.FlushFinalBlock();
                        //повертаємо дешифроване повідомлення
                        return memoryStream.ToArray();
                    }
                }
            }
        }
        class trippleDES
        {
            //генератор ключа заданої довжини
            public byte[] GenerateRandomNumber(int length)
            {
                //створюємо новий екземпляр випадкової послідовності
                using (var randomNumberGenerator = new RNGCryptoServiceProvider())
                {
                    //створюємо масив байтів для виводу заданої довжини
                    var randomNumber = new byte[length];
                    //виводимо випадкові байти в масив для виводу
                    randomNumberGenerator.GetBytes(randomNumber);
                    //повертаємо значення
                    return randomNumber;
                }
            }
            public byte[] Encrypt(byte[] encData, byte[] key, byte[] iv)
            {
                using (var des = new TripleDESCryptoServiceProvider())
                {
                    //задаємо режим симетричного шифрування
                    des.Mode = CipherMode.CBC;
                    //режим додавання тексту, для доповнення до блоку
                    des.Padding = PaddingMode.PKCS7;
                    //ключ
                    des.Key = key;
                    //ініціалізація вектору
                    des.IV = iv;
                    using (var mem_iv = new MemoryStream())
                    {
                        //новий екземпляр потоку криптографічних перетворень
                        var cryptoStream = new CryptoStream(mem_iv, des.CreateEncryptor(), CryptoStreamMode.Write);
                        //записуємо байти для обробки, зміщення, довжину масиву
                        cryptoStream.Write(encData, 0, encData.Length);
                        //оновлюємо данні з буфферу і чистимо його
                        cryptoStream.FlushFinalBlock();
                        //повертаємо дешифроване повідомлення
                        return mem_iv.ToArray();
                    }
                }
            }
            public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
            {
                using (var des = new TripleDESCryptoServiceProvider())
                {
                    //задаємо режим симетричного шифрування
                    des.Mode = CipherMode.CBC;
                    //режим додавання тексту, для доповнення до блоку
                    des.Padding = PaddingMode.PKCS7;
                    //ключ
                    des.Key = key;
                    //ініціалізація вектору
                    des.IV = iv;
                    using (var mem_iv = new MemoryStream())
                    {
                        //новий екземпляр потоку криптографічних перетворень
                        var cryptoStream = new CryptoStream(mem_iv, des.CreateDecryptor(), CryptoStreamMode.Write);
                        //записуємо байти для обробки, зміщення, довжину масиву
                        cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                        //оновлюємо данні з буфферу і чистимо його
                        cryptoStream.FlushFinalBlock();
                        //повертаємо дешифроване повідомлення
                        return mem_iv.ToArray();
                    }
                }
            }
        }
        class PBKDF2
        {
            public static byte[] GenerateSalt(int length)
            {
                using (var randomNumberGenerator = new RNGCryptoServiceProvider())
                {
                    var randomNumber = new byte[length];//випадкове наше число, в массив перетворюємо
                    randomNumberGenerator.GetBytes(randomNumber);
                    //повертаємо масив байтів випадкового числа
                    return randomNumber;
                }
            }
            public static byte[] Generator(byte[] toBeHashed, int length)
            {
                byte[] salt = GenerateSalt(16);
                using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, variant))//створили на основі паролю
                {
                    return rfc2898.GetBytes(length);//повернули ключ
                }
            }
        }



    }

}

