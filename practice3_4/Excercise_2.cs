using System;

namespace practice3_4
{
    public static class Excercise_2
    {
        public static void poch()
        {
            const string source = "po1MVkAE7IjUUwu61XxgNg==";

            
            
            
            for (int i = 0; i < 100_000_000; i++)
            {
                
                if (i % 10_000 == 0) Console.Write($"\r{i.ToString("N0")}");
                string tempHash = hashmade.GMD5(i.ToString("d8"));
                // порівнюємо хэші з завдання и отримані за допомогою цикла
                if (tempHash == source)
                {
                    Console.Write($"\r{i.ToString("N0")} iterations");
                    Console.WriteLine("\nИзначальный пароль: " + i);
                    break;
                }
            }
        }
    }
}