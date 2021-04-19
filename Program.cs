using System;

namespace VendingMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            VendingMachine vending = new VendingMachine();
            Console.WriteLine("\t\t\tThis is the Vending Machine!\n\n");

            vending.StartVending();
            Environment.Exit(0);
        }
    }
}
