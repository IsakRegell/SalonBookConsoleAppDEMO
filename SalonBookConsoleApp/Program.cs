using SalonBookConsoleApp.Methods;
using SalonBookConsoleApp.Models;
using Spectre.Console;
using System;
using System.Linq;
using System.Security.Cryptography;

public class Program
{
    static void Main()
    {
        LoginManeger loginManeger = new LoginManeger();
        CustomerProgram menu = new CustomerProgram();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== SalonBook - Välkommen ===");
            Console.WriteLine("1. Logga in som kund");
            Console.WriteLine("2. Logga in som admin");
            Console.WriteLine("3. Registrera dig");
            Console.WriteLine("4. Avsluta");
            Console.Write("\nVälj ett alternativ: ");

            string startMenuChoice = Console.ReadLine()!;
            Console.Clear();

            switch (startMenuChoice)
            {
                case "1":
                    loginManeger.Login("customer");
                    break;
                case "2":
                    loginManeger.Login("admin");
                    break;
                case "3":
                    loginManeger.RegisterCustomer();
                    break;
                case "4":
                    Console.WriteLine("Programmet avslutas...");
                    return;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }

            Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn...");
            Console.ReadKey();
        }
    }
}
