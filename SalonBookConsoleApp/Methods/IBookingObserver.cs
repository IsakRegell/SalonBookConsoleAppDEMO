using SalonBookConsoleApp.Models;

namespace SalonBookConsoleApp.Methods
{
    public interface IBookingObserver
    {
        void Update(Booking booking);
    }
}
