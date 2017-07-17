using System;

namespace COLES
{
    class Program
    {
        static void Main(string[] args)
        {
            // SEARCHING FOR
            
            // John Doe who is a Male and lives in Zip 94587 or 94338
            var combo1 = new Name("John", "Doe") & new Gender(Gender.GenderType.Male) & (new ZipCode("94587") | new ZipCode("94338"));

            // John Doe who is a Male or Unknown Gender and lives in Zip 94587 or 94338
            var combo2 = new Name("John", "Doe") & (new Gender(Gender.GenderType.Male) | new Gender(Gender.GenderType.Unspecified)) & (new ZipCode("94587") | new ZipCode("94338"));

            // John Doe who is a Male and lives in Zip 94587 and his Age is 35, 40 or 45
            var combo3 = new Name("John", "Doe") & (new Gender(Gender.GenderType.Male) & (new ZipCode("94587") & (new Age(35) | new Age(40) | new Age(45) )));

            Console.WriteLine(combo1.ToString());
            Console.WriteLine();
            Console.WriteLine(combo2.ToString());
            Console.WriteLine();
            Console.WriteLine(combo3.ToString());

            Console.ReadLine();
        }
    }
}
