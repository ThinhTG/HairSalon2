using Candidate_DAO;
using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO
{
    public class BookingManagementDAO
    {
        private GenericDAO<Booking> BookingDAO;
        private static BookingManagementDAO instance;

        public BookingManagementDAO()
        {
            BookingDAO = new GenericDAO<Booking>( new HairSalonContext());
        }

        public static BookingManagementDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BookingManagementDAO();
                }
                return instance;
            }
        }

        public Booking GetBookingById(int id)
        {
            return BookingDAO.GetById(id);
        }

        public List<Booking> GetAllBookings()
        {

            return BookingDAO.GetAll();

        }

        public bool AddBooking(Booking booking)
        {
            bool isSuccess = false;
            // neu id da ton tai roi thi khong cho them nua
            Booking booking1 = this.GetBookingById(booking.BookingId);
            try
            {
                if (booking1 == null)
                {
                    BookingDAO.Add(booking1);
                }
            }
            catch (Exception ex)
            {
                // Write Log
                throw new Exception(ex.Message);
            }
            return isSuccess;
        }

        public bool DeleteBooking(int bookingId)
        {
            bool isSuccess = false;
            Booking booking = this.GetBookingById(bookingId);
            try
            {
                if (booking != null)
                {
                    BookingDAO.Delete(bookingId);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSuccess;
        }

        public bool UpdateBooking(Booking booking)
        {
            bool isSuccess = false;
            // CandidateProfile candidateProfile1 = this.GetCandidateProfileById(candidateProfile.CandidateId);
            try
            {
                if (booking != null)
                {
                    BookingDAO.Update(booking);

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSuccess;
        }


    }
}
