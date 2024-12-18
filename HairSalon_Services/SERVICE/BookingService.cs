﻿using HairSalon_BusinessObject.Models;
using HairSalon_Repository;
using HairSalon_Repository.INTERFACE;
using HairSalon_Repository.REPOSITORY;
using HairSalon_Services.INTERFACE;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.SERVICE
{
    public class BookingService : IBookingService
    {
        private IBookingRepo _bookingRepo;
        public BookingService()
        {
            _bookingRepo = new BookingRepo();
        }

        public bool AddBooking(Booking booking)
        {
            return _bookingRepo.AddBooking(booking);
        }

        public List<Booking> GetBookings()
        {
            return _bookingRepo.GetBookingsList();
        }

        public Booking GetBookingById(int bookingId)
        {
            return _bookingRepo.GetBookingById(bookingId);
        }

        public bool SaveChanges()
        {
            return _bookingRepo.SaveChanges();
        }

        public bool UpdateBookingStatus(int bookingId, string newStatus)
        {
            return _bookingRepo.UpdateBookingStatus(bookingId, newStatus);
        }

        public List<Booking> GetBookingsByUserId(int userId)
        {
            return _bookingRepo.GetBookingsByUserId(userId);
            }
        public List<Booking> SearchBookingByDate(int userId, DateTime fromDate, DateTime toDate)
        {
            return _bookingRepo.SearchBookingByDate(userId, fromDate, toDate);
        }

        
        public List<Booking> GetPendingBookingsByUserId(int userId)
        {
            return _bookingRepo.GetPendingBookingsByUserId(userId);
        }

        public bool CancelBookingAndDetails(int bookingId)
        {
            return _bookingRepo.CancelBookingAndDetails(bookingId);
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingRepo.GetBookingByIdAsync(bookingId);
        }
    }
}


