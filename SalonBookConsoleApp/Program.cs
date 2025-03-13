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
        LoginAndRegister loginManeger = new LoginAndRegister();
        CustomerProgram menu = new CustomerProgram();
        while (true)
        {
            string startMenuChoice = HelpMethods.MainProgramMenu();

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
        }
    }
}
