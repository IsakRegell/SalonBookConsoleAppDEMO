using SalonBookConsoleApp.Models;
using System.Collections.Generic;

namespace SalonBookConsoleApp.Methods
{
    public class BookingNotifier
    {
        private readonly List<IBookingObserver> _observers = new List<IBookingObserver>();

        public void Attach(IBookingObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IBookingObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(Booking booking)
        {
            foreach (var observer in _observers)
            {
                observer.Update(booking);
            }
        }
    }
}
