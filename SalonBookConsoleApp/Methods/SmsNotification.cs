using SalonBookConsoleApp.Models;
using System;

namespace SalonBookConsoleApp.Methods
{
    public class SmsNotification : IBookingObserver
    {
        public void Update(Booking booking)
        {
            Console.WriteLine($"SMS skickat: Din bokning för {booking.Service.Name} är bekräftad den {booking.DateTime}");
        }
    }
}
