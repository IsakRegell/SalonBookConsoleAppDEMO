using Microsoft.EntityFrameworkCore;
using SalonBookConsoleApp.Models;
using Spectre.Console;

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
                Console.WriteLine("1. Visa mina bokningar");
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
            using (var db = new SalonBookContext())
            {
                // Välj tjänst
                var services = db.Services.ToList();
                if (services.Count == 0)
                {
                    Console.WriteLine("\nInga tjänster finns tillgängliga.");
                    return;
                }

                var serviceSelection = AnsiConsole.Prompt(
                    new SelectionPrompt<Service>()
                        .Title("Välj en tjänst:\n")
                        .PageSize(5)
                        .AddChoices(services)
                        .UseConverter(s => $"{s.ServiceId}. {s.Name} - {s.Price} kr")
                );

                int serviceId = serviceSelection.ServiceId;
                var service = serviceSelection;

                // Begränsa datumval till max 7 dagar framåt och skapa en lista av valbara datum
                DateTime today = DateTime.Today;
                DateTime maxDate = today.AddDays(7);
                var availableDates = Enumerable.Range(0, 8)
                                               .Select(offset => today.AddDays(offset))
                                               .ToList();

                // Använd navigeringsmeny för att välja datum med piltangenter
                DateTime requestedDate = AnsiConsole.Prompt(
                    new SelectionPrompt<DateTime>()
                        .Title("Välj önskat datum:")
                        .PageSize(8)
                        .AddChoices(availableDates)
                        .UseConverter(date => date.ToString("yyyy-MM-dd"))
                );

                // Välj tid på dagen
                TimeSpan requestedTime = AnsiConsole.Prompt(
                    new SelectionPrompt<TimeSpan>()
                        .Title("Välj önskad tid:")
                        .PageSize(10) // Se till att detta är större än antalet alternativ
                        .AddChoices(
                           new TimeSpan(9, 0, 0),
                           new TimeSpan(10, 0, 0),
                           new TimeSpan(11, 0, 0),
                           new TimeSpan(13, 0, 0),
                           new TimeSpan(14, 0, 0),
                           new TimeSpan(15, 0, 0)
                         )
                        .UseConverter(t => t.ToString(@"hh\:mm"))
);



                DateTime requestedDateTime = requestedDate.Add(requestedTime);

                // Kolla om kunden redan har en bokning som överlappar
                var customerBookings = db.Bookings
                    .Where(b => b.CustomerId == loggedInCustomer.CustomerId)
                    .Include(b => b.Service)
                    .ToList();

                foreach (var booking in customerBookings)
                {
                    DateTime existingStartTime = booking.DateTime;
                    DateTime existingEndTime = existingStartTime.AddMinutes(booking.Service.Duration);
                    DateTime newEndTime = requestedDateTime.AddMinutes(service.Duration);

                    if (requestedDateTime < existingEndTime && existingStartTime < newEndTime)
                    {
                        Console.WriteLine($"Du har redan en bokning ({booking.Service.Name}) vid denna tid. Välj en annan tid. Du skickas tillbaka till menyn.");
                        return;
                    }
                }

                // Kolla tillgänglig personal
                var availableStaff = db.Staff
                    .Where(s => !db.Bookings.Any(b =>
                        b.StaffId == s.StaffId &&
                        b.DateTime < requestedDateTime.AddMinutes(service.Duration) &&
                        b.DateTime.AddMinutes(b.Service.Duration) > requestedDateTime))
                    .ToList();

                if (availableStaff.Count == 0)
                {
                    Console.WriteLine("Ingen personal är tillgänglig vid den tiden. Välj en annan tid.");
                    return;
                }

                // Välj personal
                var assignedStaff = availableStaff.First();

                // Skapa bokningen
                var newBooking = new Booking
                {
                    CustomerId = loggedInCustomer.CustomerId,
                    ServiceId = serviceId,
                    StaffId = assignedStaff.StaffId,
                    DateTime = requestedDateTime,
                    Status = "Booked"
                };

                //Felhantering vid bokningssparning
                try
                {
                    db.Bookings.Add(newBooking);
                    db.SaveChanges();
                    Console.WriteLine($"Bokningen för {service.Name} har bekräftats med {assignedStaff.Name} den {requestedDateTime}.");
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("Fel vid sparning av bokning!");
                    Console.WriteLine($"Detaljer: {ex.InnerException?.Message}");
                }

            }
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
