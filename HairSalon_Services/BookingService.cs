    using HairSalon_BusinessObject.Models;
    using HairSalon_Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace HairSalon_Services
    {
        public class BookingService : IBookingService
        {
            private readonly IBookingRepository bookingRepository;


            public BookingService()
        {
            bookingRepository = new BookingRepository();
        }
            public bool AddBooking(Booking booking)
            {
                return bookingRepository.AddBooking(booking);
            }

            public bool DeleteBooking(int booking)
            {
                return bookingRepository.DeleteBooking(booking);
            }

            public Booking GetBooking(int id)
            {
               return bookingRepository.GetBookingById(id);
            }

            public List<Booking> GetBookings()
            {
                return bookingRepository.GetBookingsList();
            }

            public bool UpdateBooking(Booking booking)
            {
                return bookingRepository.UpdateBooking(booking);
            }
        }
    }
