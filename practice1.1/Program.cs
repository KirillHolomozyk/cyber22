using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Enter the seed number: ");
        int seed = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("How many numbers do you want to generate?");
        int count = Convert.ToInt32(Console.ReadLine());

        Random rnd = new Random(seed);
        for (int ctr = 0; ctr < count; ctr++)
        {
            Console.Write("{0,3}   ", rnd.Next(-10, 11));
        }

    }
}
