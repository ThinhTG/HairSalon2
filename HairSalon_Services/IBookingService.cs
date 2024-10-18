using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services
{
    public interface IBookingService
    {
        public Booking GetBooking(int id);

        public List<Booking> GetBookings();

        public bool AddBooking(Booking booking);

        public bool DeleteBooking(int booking);

        public bool UpdateBooking(Booking booking);
    }
}
