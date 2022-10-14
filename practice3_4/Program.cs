using System;
//Це наше меню, яке запускає весь код за командою.
namespace practice3_4
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Choose task number №1-4: ");
                char oper = Console.ReadKey().KeyChar;
                Console.WriteLine();
                

                switch (oper)
                {
                    case '1':
                        Excercise_1.poch();
                        break;

                    case '2':
                        Excercise_2.poch();
                        break;

                    case '3':
                        Excercise_3.poch();
                        break;

                    case '4':
                        Excercise_4.poch();
                        break;

                    default:
                        Console.WriteLine("END");
                        return;
                }
            }
        }
    }
}