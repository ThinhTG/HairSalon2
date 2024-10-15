using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DAO;
using HairSalon_DAO.DTO;
using HairSalon_Repository.INTERFACE;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.REPOSITORY
{
    public class BookingRepo : IBookingRepo
    {
        private readonly BookingDAO _bookingDAO;

        public BookingRepo()
        {
            _bookingDAO = BookingDAO.Instance;
        }

        public bool AddBooking(Booking booking)
            => BookingDAO.Instance.AddBooking(booking);

        public Booking GetBookingById(int bookingId)
            => BookingDAO.Instance.GetBookingById(bookingId);

        public bool UpdateBookingStatus(int bookingId, string newStatus)
            => BookingDAO.Instance.UpdateBookingStatus(bookingId, newStatus);

        public bool SaveChanges()
            => BookingDAO.Instance.SaveChanges();

    }
}
