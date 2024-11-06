using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DAO;
using HairSalon_DAO.DTO;
using HairSalon_Repository.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.REPOSITORY
{
    public class BookingDetailRepo : IBookingDetailRepo
    {
        public bool AddBookingDetail(BookingDetail bookingDetail)
            => BookingDetailDAO.Instance.AddBookingDetail(bookingDetail);

        public List<BookingDetail> GetPendingBookingDetails()
            => BookingDetailDAO.Instance.GetPendingBookingDetails();


        public bool UpdateBookingDetailStatus(int bookingDetailId, string newStatus)
            => BookingDetailDAO.Instance.UpdateBookingDetailStatus(bookingDetailId, newStatus);

        public bool SaveChanges()
          => BookingDetailDAO.Instance.SaveChanges();

        public List<BookingDetailDTO> GetBookingDetailsByBookingId(int bookingId)
          => BookingDetailDAO.Instance.GetBookingDetailsByBookingId((int)bookingId);

        public BookingDetail GetBookingDetailById(int bookingDetailId)
         => BookingDetailDAO.Instance.GetBookingDetailById((int)bookingDetailId);
    }
}
