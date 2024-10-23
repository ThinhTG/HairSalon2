using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.INTERFACE
{
    public interface IBookingRepo
    {
        bool AddBooking(Booking booking);
        Booking GetBookingById(int bookingId);
        bool UpdateBookingStatus(int bookingId, string newStatus);
        bool SaveChanges();
        public List<Booking> GetBookingsList();

        
    }

}
