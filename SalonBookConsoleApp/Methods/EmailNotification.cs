using SalonBookConsoleApp.Models;
using System;

namespace SalonBookConsoleApp.Methods
{
    public class EmailNotification : IBookingObserver
    {
        public void Update(Booking booking)
        {
            Console.WriteLine($"Mail skickad: Din bokning för {booking.Service.Name} är bekräftad den {booking.DateTime}");
        }
    }
}
