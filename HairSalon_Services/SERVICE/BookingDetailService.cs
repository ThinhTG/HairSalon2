using HairSalon_BusinessObject.Models;
using HairSalon_Repository.INTERFACE;
using HairSalon_Repository.REPOSITORY;
using HairSalon_Services.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.SERVICE
{
    public class BookingDetailService : IBookingDetailService
    {
        private IBookingDetailRepo _bookingDetailRepo;
        public BookingDetailService()
        {
            _bookingDetailRepo = new BookingDetailRepo();
        }

        public bool AddBookingDetail(BookingDetail bookingDetail)
        {
          return _bookingDetailRepo.AddBookingDetail(bookingDetail);
        }

        public List<BookingDetail> GetBookingDetailsByBookingId(int bookingId)
        {
            return _bookingDetailRepo.GetBookingDetailsByBookingId(bookingId);
        }

        public List<BookingDetail> GetBookingDetailByBookingId(int bookingId)
        {
            return _bookingDetailRepo.GetBookingDetailByBookingId(bookingId);
        }




        public List<BookingDetail> GetPendingBookingDetails()
        {
            return _bookingDetailRepo.GetPendingBookingDetails();
        }

        public bool UpdateBookingDetailStatus(int bookingDetailId, string newStatus)
        {
            return _bookingDetailRepo.UpdateBookingDetailStatus(bookingDetailId, newStatus);
        }

        public bool SaveChanges()
        {
            return _bookingDetailRepo.SaveChanges();
        }

        //public List<BookingDetail> GetCompletedBookingDetails()
        //{
        //    return _bookingDetailRepo.GetAllBookingDetailCompleted();
        //}
    }

}
