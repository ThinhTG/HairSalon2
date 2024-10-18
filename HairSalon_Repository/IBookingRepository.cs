using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository
{
    public interface IBookingRepository
    {
        public Booking GetBookingById(int id);
        public List<Booking> GetBookingsList();

        public bool AddBooking(Booking booking);

        public bool DeleteBooking(int bookingId);

        public bool UpdateBooking(Booking booking);
    }
}
