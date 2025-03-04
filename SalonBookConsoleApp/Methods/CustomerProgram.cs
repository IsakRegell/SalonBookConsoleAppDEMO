using Microsoft.EntityFrameworkCore;
using SalonBookConsoleApp.Models;
using System;

namespace SalonBookConsoleApp.Methods
{
    public class CustomerProgram
    {
        public void CustomerMenu(Customer loggedInCustomer) // Tar emot inloggad kund
        {
            bool running = true; // Kontrollera om menyn ska köras

            while (running)
            {
                Console.Clear();
                Console.WriteLine($"=== Kundmeny ({loggedInCustomer.Name}) ==="); // Visar kundens namn
                Console.WriteLine("1. Visa bokningar");
                Console.WriteLine("2. Boka tid");
                Console.WriteLine("3. Avboka tid");
                Console.WriteLine("4. Logga ut");
                Console.Write("\nVälj ett alternativ: ");

                string customerMenuChoice = Console.ReadLine()!; // Tar in användarens val

                switch (customerMenuChoice)
                {
                    case "1":
                        ShowBookings(loggedInCustomer);
                        break;
                    case "2":
                        ShowAllServices(loggedInCustomer);
                        BookAService(loggedInCustomer);
                        break;
                    case "3":
                        Console.WriteLine($"{loggedInCustomer.Name} avbokar en tid...");
                        break;
                    case "4":
                        Console.WriteLine("Du loggas ut...");
                        running = false; // Avsluta loopen och gå tillbaka till huvudmenyn
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }

                // Förhindra att menyn omedelbart startar om utan att visa resultat
                if (running)
                {
                    Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                    Console.ReadKey();
                }
            }
        }

        public void BookAService(Customer loggedInCustomer)
        {
            Console.Write("\nVälj ett av alternativen ovan: ");
            string serviceChoice = Console.ReadLine()!;
        }
        public void ShowAllServices(Customer loggedInCustomer)
        {
            using (var db = new SalonBookContext()) // Ansluter till databasen
            {
                var services = db.Services.ToList(); // Hämtar alla tjänster

                Console.Clear();
                Console.WriteLine("=== Tillgängliga tjänster ===");

                if (services.Count == 0)
                {
                    Console.WriteLine("Inga tjänster hittades.");
                }
                else
                {
                    foreach (var service in services)
                    {
                        Console.WriteLine($"{service.ServiceId}. Tjänst: {service.Name}, Kostnad: {service.Price} kr, Beskrivning: {service.Description}");
                    }
                }
            }
        }

        private void ShowBookings(Customer loggedInCustomer)
        {
            using (var db = new SalonBookContext())
            {
                var bookings = db.Bookings
                                 .Where(b => b.CustomerId == loggedInCustomer.CustomerId)
                                 .Include(b => b.Service)
                                 .ToList();
                Console.Clear();
                Console.WriteLine($"=== Bokningar för {loggedInCustomer.Name} ===");

                if (bookings.Count == 0)
                {
                    Console.WriteLine("Du har inga bokningar");
                }
                else
                {
                    foreach (var booking in bookings)
                    {
                        Console.WriteLine($"Tjänst: {booking?.Service?.Name}, Datum: {booking?.DateTime}, Status: {booking?.Status}");
                    }
                }
            }
        }
    }
}
