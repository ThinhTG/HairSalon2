using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class BookingDAO
    {
        private HairSalonServiceContext dbContext;

        private static BookingDAO instance = null;

        public static BookingDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BookingDAO();
                }
                return instance;
            }
        }

        public BookingDAO()
        {
            dbContext = new HairSalonServiceContext();
        }

        public List<Booking> GetAll()
        {
            return dbContext.Booking.ToList();
        }

        public Booking GetBookingById(int bookingId)
        {
            return dbContext.Booking.SingleOrDefault(b => b.BookingId == bookingId);
        }

        public bool AddBooking(Booking booking)
        {
            bool isSuccess = false;
            try
            {
                if (booking != null)
                {

                    foreach (var detail in booking.BookingDetail)
                    {
                        detail.Booking = booking;
                    }

                    dbContext.Booking.Add(booking);
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding BookingDetail: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                }
                throw;
            }
            return isSuccess;
        }


        public bool UpdateBookingStatus(int bookingId, string newStatus)
        {
            bool isSuccess = false;
            try
            {
                var booking = dbContext.Booking.SingleOrDefault(b => b.BookingId == bookingId);

                if (booking != null)
                {
                    booking.Status = newStatus;
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSuccess;
        }

        public List<Booking> SearchBookingByDate(int userId, DateTime fromDate, DateTime toDate)
        {
            return dbContext.Booking
                            .Where(b => b.UserId == userId && b.BookingDate >= fromDate && b.BookingDate <= toDate)
                            .ToList();
        }

        public List<Booking> GetBookingsByUserId(int userId)
        {
            return dbContext.Booking
                .Where(b => b.UserId == userId)
                .Include(b => b.User) 
                .Include(b => b.BookingDetail)
                .ToList();
        }
        public bool CancelBookingAndDetails(int bookingId)
        {
            var booking = dbContext.Booking.Include(b => b.BookingDetail)
                                           .FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return false;

            booking.Status = "Canceled";
            foreach (var detail in booking.BookingDetail)
            {
                detail.Status = "Canceled";
            }

            dbContext.SaveChanges();
            return true;
        }
        public bool SaveChanges()
        {
            try
            {
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving changes: " + ex.Message);
            }
        }
    }
}
