﻿using HairSalon_BusinessObject.Models;
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

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            // Use a new DbContext instance to ensure thread safety
            using (var context = new HairSalonServiceContext())
            {
                return await context.Booking.FindAsync(bookingId);
            }
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

        public List<Booking> GetBookingByUserId(int userId)
        {
            return dbContext.Booking.Where(b => b.UserId == userId).ToList();
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
            var endDate = toDate.Date.AddDays(0);

            return dbContext.Booking
                             .Where(b => b.UserId == userId && b.BookingDate >= fromDate && b.BookingDate < endDate)
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

        public List<Booking> GetPendingBookingsByUserId(int userId)
        {
            try
            {
                var pendingBookings = dbContext.Booking
                    .Where(b => b.UserId == userId && b.Status == "Pending")
                    .ToList();
                return pendingBookings;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving pending bookings: {ex.Message}");
                return new List<Booking>();
            }
        }

        public bool CancelBookingAndDetails(int bookingId)
        {
            var booking = dbContext.Booking.Include(b => b.BookingDetail)
                                           .FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return false;

            booking.Status = "Cancelled";
            foreach (var detail in booking.BookingDetail)
            {
                detail.Status = "Cancelled";
            }

            dbContext.SaveChanges();
            return true;
        }
      
        public async Task<bool> UpdateBookingStatusAsync(int bookingId, string newStatus)
        {
            bool isSuccess = false;
            try
            {
                var booking = await dbContext.Booking.SingleOrDefaultAsync(b => b.BookingId == bookingId);

                if (booking != null)
                {
                    booking.Status = newStatus;
                    await dbContext.SaveChangesAsync();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSuccess;
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
