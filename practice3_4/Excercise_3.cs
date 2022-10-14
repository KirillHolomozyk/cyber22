using System;
using System.IO;
using System.Linq;

namespace practice3_4
{
    public static class Excercise_3
    {
        const string file3 = "../../../exc3.txt";

        public static void poch()
        {
            Console.Write("Ваша строка: ");
            string entry = Console.ReadLine();

            
            string hash = hashmade.GMD5(entry);//хешуємо введене
            if (!File.Exists(file3)) File.Create(file3);//записуємо в наш файл
            string[] db = File.ReadAllLines(file3);//читаємо дані з файлу
            
            if (db.Contains(hash))//перевіряємо чи не повторюється код
            {
                Console.WriteLine("Вы уже хешировали это сообщение!");
            }
            else
            {
                Console.WriteLine("Данные успешно сохранены.");
                File.AppendAllText(file3, "\n" + hash);//записуємо хеш в кінець файлу(саме через AppendAllText)
            }

            Console.WriteLine();
        }
    }
}