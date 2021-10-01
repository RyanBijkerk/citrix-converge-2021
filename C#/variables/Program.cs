using System;

namespace variables
{
    class Program
    {
        static void Main(string[] args)
        {
            var var1 = 5;
            Console.WriteLine(var1);

            var1 += 1;
            Console.WriteLine(var1);

            var var2 = "This is a sting";
            Console.WriteLine(var2);

            Console.WriteLine(var2.ToUpper());

            var var3 = true;
            Console.WriteLine(var3.GetType());

            if (var3 == true) {
                Console.WriteLine(true);
            } else {
                Console.WriteLine(false);
            }
        }
    }
}
