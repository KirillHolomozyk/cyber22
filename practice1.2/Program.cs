using System.Security.Cryptography;


class RandomCryptoNumbers
{
    public static void Main()
    {
        int c = 1;
        Console.WriteLine("How many numbers do you want to generate?");
        int count = Convert.ToInt32(Console.ReadLine());
        for (int i = 0; i <= count; i++)
        {
            var rnd = GenerateRandomNumber();
            Console.WriteLine(Convert.ToBase64String(rnd));
            c++;

        }
    }
    public static byte[] GenerateRandomNumber()
    {
        using (var randomNumberGenerator =
               new RNGCryptoServiceProvider())
        {
            int bit = 32;
            var randomNumber = new byte[bit];
            randomNumberGenerator.GetBytes(randomNumber);
            return randomNumber;
        }
    }

}
