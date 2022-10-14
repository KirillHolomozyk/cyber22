using System;
using System.Collections.Generic;
using System.IO;

namespace practice3_4
{
    public class Excercise_4
    {
        public static void poch()
        {
            // Путь до файлу за даними (бекап)
            const string file4 = "../../../ex4.txt";
            //Меню реєстрацію на входу в акаунт
            Console.Write("\nРеєстрація -- 1\nУвійти в свій акаунт -- 2\nвведіть 1 или 2: ");
            char operation = Console.ReadKey().KeyChar; 
            Console.WriteLine(); 

            
            if (operation != '1' && operation != '2') return;
            bool notCompleted = true;

            while (notCompleted)
            {
                Console.Write("Логін: ");
                string log = Console.ReadLine();
                Console.Write("Пароль: ");
                string password = Console.ReadLine();

                
                string passwordHash = hashmade.GMD5(password);

                
                if (!File.Exists(file4)) File.Create(file4);
                
                string[] fileData = File.ReadAllLines(file4);

                var db = new Dictionary<string, string>();
                //дані з файлу записуємо в словарь
                foreach (string item in fileData)
                {
                    var temp = item.Split('\t');
                    db.Add(temp[0], temp[1]);
                }

                
                if (operation == '1')
                {
                    if (db.ContainsKey(log))
                    {
                        Console.WriteLine("Цей логій вже зареєстрований в системі");//якщо реєструємо по існуючому логіну, то виводимо повідомлення
                        notCompleted = true;
                    }
                    else
                    {
                        
                        Console.WriteLine("Реєстрація виконана!");
                        
                        string dataToSave = $"{log}\t{passwordHash}\n";//пишемо данні у файл
                        File.AppendAllText(file4, dataToSave);
                        notCompleted = false;
                    }
                }
                else if (operation == '2')
                {
                    
                    
                    if (db.ContainsKey(log) && db[log] == passwordHash)// за ключем отримуємо хеш
                    {
                        Console.WriteLine("You are logged in");
                        notCompleted = false;
                    }
                    else
                    {
                        Console.WriteLine("User not exist");
                        notCompleted = true;
                    }
                }
            }

            Console.WriteLine();//знову виправляємо консоль
        }
    }
}