using HairSalon_BusinessObject.Models;
using HairSalon_DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository
{
    public class BookingRepository : IBookingRepository
    {
        public bool AddBooking(Booking booking) => BookingManagementDAO.Instance.AddBooking(booking);

        public bool DeleteBooking(int bookingId) => BookingManagementDAO.Instance.DeleteBooking(bookingId);

        public Booking GetBookingById(int id) => BookingManagementDAO.Instance.GetBookingById(id);
        

        public List<Booking> GetBookingsList() => BookingManagementDAO.Instance.GetAllBookings();
     

        public bool UpdateBooking(Booking booking) => BookingManagementDAO.Instance.UpdateBooking(booking);
        
    }
}
