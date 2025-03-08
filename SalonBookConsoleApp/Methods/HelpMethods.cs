using Microsoft.EntityFrameworkCore;
using SalonBookConsoleApp.Models;
using Spectre.Console;

public static class HelpMethods
{
    public static Service SelectService(SalonBookContext db)
    {
        var services = db.Services.ToList();
        var serviceOptions = services
            .Select(s => new { Service = s, DisplayText = $"{s.ServiceId}. {s.Name} - {s.Price} kr" })
            .ToList();
        serviceOptions.Add(new { Service = (Service)null, DisplayText = "❌ Avbryt bokning" });

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<dynamic>()
                .Title("Välj en tjänst:\n")
                .PageSize(5)
                .AddChoices(serviceOptions)
                .UseConverter(s => s.DisplayText)
        );

        if (selection.Service == null)
        {
            Console.WriteLine("Bokningen har avbrutits.");
            return null;
        }

        return selection.Service;
    }

    public static DateTime SelectDateTime()
    {
        var availableDates = Enumerable.Range(0, 8)
                                       .Select(offset => DateTime.Today.AddDays(offset))
                                       .ToList();
        availableDates.Add(DateTime.MinValue); // Avbryt-alternativ

        DateTime selectedDate = AnsiConsole.Prompt(
            new SelectionPrompt<DateTime>()
                .Title("Välj önskat datum:")
                .PageSize(8)
                .AddChoices(availableDates)
                .UseConverter(date => date == DateTime.MinValue ? "❌ Avbryt bokning" : date.ToString("yyyy-MM-dd"))
        );

        if (selectedDate == DateTime.MinValue)
        {
            Console.Clear();
            Console.WriteLine("Bokningen har avbrutits.");
            return DateTime.MinValue;
        }

        var timeOptions = new List<TimeSpan>
        {
            new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0),
            new TimeSpan(11, 0, 0), new TimeSpan(13, 0, 0),
            new TimeSpan(14, 0, 0), new TimeSpan(15, 0, 0)
        };
        timeOptions.Add(TimeSpan.MinValue);

        TimeSpan selectedTime = AnsiConsole.Prompt(
            new SelectionPrompt<TimeSpan>()
                .Title("Välj önskad tid:")
                .PageSize(10)
                .AddChoices(timeOptions)
                .UseConverter(t => t == TimeSpan.MinValue ? "❌ Avbryt bokning" : t.ToString(@"hh\:mm"))
        );

        if (selectedTime == TimeSpan.MinValue)
        {
            Console.Clear();
            Console.WriteLine("Bokningen har avbrutits.");
            return DateTime.MinValue;
        }

        return selectedDate.Add(selectedTime);
    }

    public static bool ValidateNoOverlappingBooking(SalonBookContext db, int customerId, DateTime requestedDateTime, int serviceDuration)
    {
        var customerBookings = db.Bookings
            .Where(b => b.CustomerId == customerId)
            .Include(b => b.Service)
            .ToList();

        bool hasOverlap = customerBookings.Any(booking =>
        {
            DateTime existingStartTime = booking.DateTime;
            DateTime existingEndTime = existingStartTime.AddMinutes(booking.Service.Duration);
            DateTime newEndTime = requestedDateTime.AddMinutes(serviceDuration);

            return requestedDateTime < existingEndTime && existingStartTime < newEndTime;
        });

        if (hasOverlap)
        {
            Console.WriteLine("⚠️ Du har redan en bokning vid denna tid! Försök igen.");
            return false;
        }

        return true;
    }

    public static Staff FindAvailableStaff(SalonBookContext db, DateTime requestedDateTime, int serviceDuration)
    {
        var availableStaff = db.Staff
            .Where(s => !db.Bookings.Any(b =>
                b.StaffId == s.StaffId &&
                b.DateTime < requestedDateTime.AddMinutes(serviceDuration) &&
                b.DateTime.AddMinutes(b.Service.Duration) > requestedDateTime))
            .ToList();

        if (!availableStaff.Any())
        {
            Console.WriteLine("Ingen personal är tillgänglig vid den tiden. Välj en annan tid.");
            return null;
        }

        return availableStaff.First();
    }

    public static void CreateBooking(SalonBookContext db, int customerId, Service service, Staff assignedStaff, DateTime requestedDateTime)
    {
        var newBooking = new Booking
        {
            CustomerId = customerId,
            ServiceId = service.ServiceId,
            StaffId = assignedStaff.StaffId,
            DateTime = requestedDateTime,
            Status = "Booked"
        };

        try
        {
            db.Bookings.Add(newBooking);
            db.SaveChanges();
            Console.WriteLine("\nBokningsbekräftelse");
            Console.WriteLine($"Datum & Tid: {requestedDateTime}");
            Console.WriteLine($"Tjänst: {service.Name}");
            Console.WriteLine($"Personal: {assignedStaff.Name}");
            Console.WriteLine($"Kostnad: {service.Price} kr");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("Fel vid sparning av bokning!");
            Console.WriteLine($"Detaljer: {ex.InnerException?.Message}");
        }
    }

    public static string MainProgramMenu()
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
        return startMenuChoice;
    }
}
