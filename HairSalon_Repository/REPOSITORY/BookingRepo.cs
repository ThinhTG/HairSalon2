using HairSalon_BusinessObject.Models;
using HairSalon_DAO;
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
        public bool AddBooking(Booking booking)
            => BookingDAO.Instance.AddBooking(booking);

        public Booking GetBookingById(int bookingId)
            => BookingDAO.Instance.GetBookingById(bookingId);

        public bool UpdateBookingStatus(int bookingId, string newStatus)
            => BookingDAO.Instance.UpdateBookingStatus(bookingId, newStatus);

        public bool SaveChanges()
            => BookingDAO.Instance.SaveChanges();

        public List<Booking> GetBookingsList() 
            => BookingDAO.Instance.GetAll();

        public List<Booking> GetBookingsByUserId(int userId)
            => BookingDAO.Instance.GetBookingByUserId(userId);
        
        public List<Booking> SearchBookingByDate(int userId, DateTime fromDate, DateTime toDate)
            => BookingDAO.Instance.SearchBookingByDate(userId, fromDate, toDate);
       
       

        public List<Booking> GetPendingBookingsByUserId(int userId)
           => BookingDAO.Instance.GetPendingBookingsByUserId(userId);

        public bool CancelBookingAndDetails(int bookingId)
         => BookingDAO.Instance.CancelBookingAndDetails(bookingId);


        public async Task<Booking> GetBookingByIdAsync(int bookingId)
            => await BookingDAO.Instance.GetBookingByIdAsync(bookingId);
    }
}
